using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Services;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Forms.Render
{
    public class DrawEditor : MonoGame.Forms.Controls.UpdateWindow
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Texture2D background;
        private Texture2D tOrigin;
        private Texture2D tPixel;
        private NXTCircuit circuit;
        private bool initialized = false;
        private KeyboardState oldKeyState;

        private Camera camera;

        private SpriteBatch spriteBatch;
        private CircuitRenderer cRend;
        private Cursor cursor;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public DrawEditor(NXTCircuit circuit)
            : base()
        {
            this.circuit = circuit;
            oldKeyState = new KeyboardState();
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Initialise le Circuit, le CircuitRenderer, la Camera et le Cursor
        public void InitializeCircuit(NXTCircuit c = null)
        {
            if (c != null)
                this.circuit = c;

            tPixel = new Texture2D(this.GraphicsDevice, 1, 1);
            tPixel.SetData<Color>(new Color[] { Color.White });

            cRend = new CircuitRenderer(circuit, this.GraphicsDevice);
            cursor = new Cursor(cRend, tPixel);
            camera = new Camera(this.GraphicsDevice.Viewport);
            initialized = true;
        }

        // Initialisation (de tout)
        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            background = RenderTools.LoadTextureFromFile(this.GraphicsDevice, "Data\\damier.png");
            tOrigin = RenderTools.LoadTextureFromFile(this.GraphicsDevice, "Data\\origine.png");
            InitializeCircuit();
        }

        // Appelé à chaque boucle logique
        protected override void Update(GameTime gameTime)
        {
            base.Update();

            KeyboardState k = Keyboard.GetState();
            MouseState m = Mouse.GetState();

            if (m.X > 0 && m.Y > 0)
            {
                camera.UpdateCamera();
                cursor.UpdateCursor(camera);

                if (k.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
                {
                    cursor.NextCursor();
                    Console.WriteLine("Next cursor");
                }

                if (k.IsKeyDown(Keys.R) && oldKeyState.IsKeyUp(Keys.R))
                {
                    cursor.Rotate();
                    Console.WriteLine("Rotated");
                }

                if (circuit.IsWithinBounds(cursor.Location))
                {
                    if (m.LeftButton == ButtonState.Pressed)
                    {
                        circuit.setCase(cursor.Location, cursor.SelectedCase.Duplicate());
                        Console.WriteLine("Click !");
                    }
                    else if (m.RightButton == ButtonState.Pressed)
                    {
                        circuit.setCase(cursor.Location, new NXTCase(Case.EMPTY));
                    }
                }
            }

            oldKeyState = k;
        }

        // Appelé à chaque boucle graphique
        protected override void Draw()
        {
            base.Draw();

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            if (circuit != null && initialized)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
                cRend.Render(spriteBatch);
                spriteBatch.Draw(tOrigin, new Vector2(-16, -16), Color.White);
                cursor.DrawCursor(spriteBatch, camera);
                spriteBatch.End();
            }
            
        }
    }
}
