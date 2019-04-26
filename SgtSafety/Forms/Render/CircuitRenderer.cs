using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Forms.Render
{
    public class CircuitRenderer
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTCircuit circuit;

        private Texture2D tStraight;
        private Texture2D tTurn;
        private Texture2D tIntersec;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        public Texture2D Texture_Straight { get { return tStraight; } }
        public Texture2D Texture_Turn { get { return tTurn; } }
        public Texture2D Texture_Intersection { get { return tIntersec; } }

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public CircuitRenderer(NXTCircuit circuit, GraphicsDevice graphicsDevice)
        {
            this.circuit = circuit;

            tStraight = RenderTools.LoadTextureFromFile(graphicsDevice, "Data\\droit.png");
            tTurn = RenderTools.LoadTextureFromFile(graphicsDevice, "Data\\virage.png");
            tIntersec = RenderTools.LoadTextureFromFile(graphicsDevice, "Data\\intersection.png");
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------
        public void Render(SpriteBatch sb)
        {
            for (int x = 0; x < circuit.Width; x++)
            {
                for (int y = 0; y < circuit.Height; y++)
                {
                    NXTCase c = circuit.getCase(x, y);
                    Texture2D t;
                    Color col = c.CaseColor;

                    // On trouve la texture (et couleur) adaptée
                    switch (c.TypeCase)
                    {
                        case (Case.EMPTY):
                            t = tStraight;
                            col = Color.Black;
                            break;
                        case (Case.STRAIGHT):
                            t = tStraight;
                            break;
                        case (Case.VIRAGE):
                            t = tTurn;
                            break;
                        case (Case.INTERSECTION):
                            t = tIntersec;
                            break;
                        default:
                            t = tStraight;
                            col = Color.Black;
                            break;
                    }
                    Console.WriteLine(col);
                    //sb.Draw(t, new Vector2(x * 32, y * 32), col);
                    sb.Draw(t, new Vector2(x * 32 + 16, y * 32 + 16), new Rectangle(0, 0, t.Width, t.Height), col, (((float)c.CaseOrientation) / 2) * (float)Math.PI, new Vector2(t.Width / 2, t.Height / 2), 1.0f, SpriteEffects.None, 0); // Deprecated but OK
                }
            }
        }
    }
}
