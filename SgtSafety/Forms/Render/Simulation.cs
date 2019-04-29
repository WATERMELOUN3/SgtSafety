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
        private IAAStar ia;
        private IADijkstra bestIa;

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
            ia = new IAAStar(this.vehicule);
            bestIa = new IADijkstra(this.vehicule);
        }

        public void CalculatePath()
        {
            List<Point> chemin = bestIa.ComputeDijkstra(new Point(0, 3), new Point(2, 0)); //ia.sendPathToVehicule(new Point(0, 3));
            foreach (Point p in chemin)
            {
                vehicule.Circuit.getCase(p).CaseColor = Color.Blue;
            }
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
