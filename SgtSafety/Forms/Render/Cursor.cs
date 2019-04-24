using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Forms.Render
{
    public class Cursor
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Vector2 cursorLocation;
        private bool inBounds = true;

        private NXTCase cCase;

        private Texture2D tStraight;
        private Texture2D tTurn;
        private Texture2D tIntersec;
        private Texture2D tPixel;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        public NXTCase SelectedCase
        {
            get { return cCase; }
        }
        public Point Location
        {
            get
            {
                return new Point((int)cursorLocation.X, (int)cursorLocation.Y);
            }
        }

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public Cursor(CircuitRenderer cRend, Texture2D pixel)
        {
            cCase = new NXTCase(Case.STRAIGHT);
            this.tStraight = cRend.Texture_Straight;
            this.tTurn = cRend.Texture_Turn;
            this.tIntersec = cRend.Texture_Intersection;
            this.tPixel = pixel;
            this.cursorLocation = new Vector2(0);
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------
        public void NextCursor()
        {
            cCase.NextCase();
        }

        public void Rotate()
        {
            cCase.NextOrientation();
        }

        public void UpdateCursor(Camera camera)
        {
            MouseState m = Mouse.GetState();
            KeyboardState k = Keyboard.GetState();

            cursorLocation = Vector2.Transform(new Vector2(m.X, m.Y), Matrix.Invert(camera.Transform));
            cursorLocation = new Vector2((int)cursorLocation.X / 32, (int)cursorLocation.Y / 32);

            if (cursorLocation.X < 0 || cursorLocation.Y < 0)
                inBounds = false;
            else
                inBounds = true;
        }

        public void DrawCursor(SpriteBatch spriteBatch, Camera camera)
        {
            Texture2D t = tStraight;
            Color c = Color.White;
            switch (cCase.TypeCase)
            {
                case Case.EMPTY:
                    c = Color.Black;
                    break;
                case Case.VIRAGE:
                    t = tTurn;
                    break;
                case Case.INTERSECTION:
                    t = tIntersec;
                    break;
            }

            // spriteBatch.Draw(t, cursorLocation * 32, c);
            spriteBatch.Draw(t, (cursorLocation * 32) + new Vector2(16, 16), new Rectangle(0, 0, t.Width, t.Height), c, (((float)cCase.CaseOrientation) / 2)*(float)Math.PI, new Vector2(t.Width / 2, t.Height / 2), 1.0f, SpriteEffects.None, 0); // Deprecated but OK

            if (!inBounds)
                spriteBatch.Draw(tPixel, new Rectangle((int)cursorLocation.X * 32, (int)cursorLocation.Y * 32, 32, 32), Color.Red);
        }
    }
}
