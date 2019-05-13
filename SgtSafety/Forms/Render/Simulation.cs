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
        private NXTVehicule iaVehicule;
        private NXTVehicule remoteVehicule;
        private KeyboardState oldKeyState;
        private SimulationWindow window;
        private IADijkstra ia;

        private Camera camera;

        private SpriteBatch spriteBatch;
        private CircuitRenderer cRend;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public Simulation(NXTVehicule iaVehicule, NXTVehicule remoteVehicule, SimulationWindow window)
            : base()
        {
            this.iaVehicule = iaVehicule;
            this.remoteVehicule = remoteVehicule;
            this.window = window;
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
            cRend = new CircuitRenderer(iaVehicule.Circuit, this.GraphicsDevice);
            camera = new Camera(this.GraphicsDevice.Viewport);
            ia = new IADijkstra(this.iaVehicule);
        }

        public bool CalculatePath()
        {
            iaVehicule.ClearBuffer();
            if (iaVehicule.Circuit.Patients.Count > 0 || iaVehicule.Patients > 0)
            {
                if (iaVehicule.Patients >= iaVehicule.MAX_PATIENTS)
                {
                    List<Point> chemin = ia.ComputeDijkstra(iaVehicule.Position, iaVehicule.Circuit.Hopitaux[0]);

                    ia.AddToIABuffer(chemin);
                    PaintPath(chemin);
                    return true;
                }
                else
                {
                    List<Point> chemin = ia.ComputeDijkstra(iaVehicule.Position, ia.FindClosestPatient(NXTVehicule.ERROR));

                    if (iaVehicule.Patients > 0)
                    {
                        List<Point> cheminHopital = ia.ComputeDijkstra(iaVehicule.Position, ia.FindClosestHopital());
                        chemin = chemin.Count >= cheminHopital.Count ? chemin : cheminHopital;
                    }

                    ia.AddToIABuffer(chemin);
                    PaintPath(chemin);
                    return true;
                }
            }

            return false;
        }

        private void PaintPath(List<Point> path)
        {
            iaVehicule.Circuit.FillColor(Color.White);
            foreach (Point p in path)
                iaVehicule.Circuit.Paint(Color.BlueViolet, p);
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

            // Robot  autonome
            spriteBatch.Draw(robot, new Vector2(iaVehicule.Position.X * 32 + 16, iaVehicule.Position.Y * 32 + 16),
                new Rectangle(0, 0, robot.Width, robot.Height), Color.White, (((float)iaVehicule.ToOrientation) / 2) * (float)Math.PI,
                new Vector2(robot.Width / 2, robot.Height / 2), 1.0f, SpriteEffects.None, 0);

            // Robot télécommandé
            if (window.RemoteEnabled)
            {
                spriteBatch.Draw(robot, new Vector2(remoteVehicule.Position.X * 32 + 16, remoteVehicule.Position.Y * 32 + 16),
                    new Rectangle(0, 0, robot.Width, robot.Height), Color.White, (((float)remoteVehicule.ToOrientation) / 2) * (float)Math.PI,
                    new Vector2(robot.Width / 2, robot.Height / 2), 1.0f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }
    }
}
