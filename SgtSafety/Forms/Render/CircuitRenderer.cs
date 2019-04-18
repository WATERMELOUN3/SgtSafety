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
        // FIELDS
        private NXTCircuit circuit;

        private Texture2D tStraight;
        private Texture2D tTurn;
        private Texture2D tIntersec;

        // CONSTRUCTOR
        public CircuitRenderer(NXTCircuit circuit, ContentManager cMan)
        {
            this.circuit = circuit;

            tStraight = cMan.Load<Texture2D>("Content\\droit.png");
            tTurn = cMan.Load<Texture2D>("Content\\virage.png");
            tIntersec = cMan.Load<Texture2D>("Content\\intersection.png");
        }


        // METHODS
        public void Render(SpriteBatch sb)
        {
            for (int x = 0; x < circuit.getWidth(); x++)
            {
                for (int y = 0; y < circuit.getHeight(); y++)
                {
                    
                }
            }
        }
    }
}
