using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using SgtSafety.NXTEnvironment;
using SgtSafety.NXTIA;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Forms.Render
{
    public class Simulation : UpdateWindow
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Texture2D background;
        private Texture2D robot;
        private NXTVehicule vehicule;
        private KeyboardState oldKeyState;
        private IADijkstra ia;

        private Camera camera;

        private SpriteBatch spriteBatch;
        private CircuitRenderer cRend;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public Simulation(NXTVehicule vehicule)
            : base()
        {
            this.vehicule = vehicule;
            oldKeyState = new KeyboardState();
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Initialisation (de tout)
        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            background = RenderTools.LoadTextureFromFile(this.GraphicsDevice, "Data\\damier.png");
            robot = RenderTools.LoadTextureFromFile(this.GraphicsDevice, "Data\\robot.png");
            cRend = new CircuitRenderer(vehicule.Circuit, this.GraphicsDevice);
            camera = new Camera(this.GraphicsDevice.Viewport);
            ia = new IADijkstra(this.vehicule);
        }

        public bool CalculatePath()
        {
            vehicule.ClearBuffer();
            if (vehicule.Circuit.Patients.Count > 0 || vehicule.Patients > 0)
            {
                if (vehicule.Patients >= vehicule.MAX_PATIENTS)
                {
                    List<Point> chemin = ia.ComputeDijkstra(vehicule.Position, vehicule.Circuit.Hopitaux[0]);

                    ia.AddToIABuffer(chemin);
                    PaintPath(chemin);
                    return true;
                }
                else
                {
                    List<Point> cheminHopital = ia.ComputeDijkstra(vehicule.Position, ia.FindClosestHopital());
                    List<Point> cheminPatient = ia.ComputeDijkstra(vehicule.Position, ia.FindClosestPatient(NXTVehicule.ERROR));

                    List<Point> chemin = cheminPatient.Count >= cheminHopital.Count ? cheminPatient : cheminHopital;

                    ia.AddToIABuffer(chemin);
                    PaintPath(chemin);
                    return true;
                }
            }

            return false;
        }

        private void PaintPath(List<Point> path)
        {
            vehicule.Circuit.FillColor(Color.White);
            foreach (Point p in path)
                vehicule.Circuit.Paint(Color.BlueViolet, p);
        }

        // Appelé à chaque boucle logique
        protected override void Update(GameTime gameTime)
        {
            base.Update();

            MouseState m = Mouse.GetState();
            KeyboardState k = Keyboard.GetState();

            camera.UpdateCamera();

            oldKeyState = k;
        }

        // Appelé à chaque boucle graphique
        protected override void Draw()
        {
            base.Draw();

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);
            cRend.Render(spriteBatch);

            // Deprecated but OK
            spriteBatch.Draw(robot, new Vector2(vehicule.Position.X * 32 + 16, vehicule.Position.Y * 32 + 16),
                new Rectangle(0, 0, robot.Width, robot.Height), Color.White, (((float)vehicule.ToOrientation) / 2) * (float)Math.PI,
                new Vector2(robot.Width / 2, robot.Height / 2), 1.0f, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
