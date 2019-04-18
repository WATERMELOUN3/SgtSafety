using Microsoft.Xna.Framework.Content;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Forms.Render
{
    public class DrawEditor : MonoGame.Forms.Controls.DrawWindow
    {
        // FIELDS
        private CircuitRenderer cRend;
        private NXTCircuit circuit;

        // METHODS
        public void InitializeCircuit(ContentManager content, NXTCircuit c)
        {
            this.circuit = c;
            cRend = new CircuitRenderer(c, content);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Draw()
        {
            if (circuit != null)
            {

            }

            base.Draw();
        }
    }
}
