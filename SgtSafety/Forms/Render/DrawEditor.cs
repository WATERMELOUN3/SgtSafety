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
    public class DrawEditor : MonoGame.Forms.Controls.UpdateWindow
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Texture2D background;
        private SpriteBatch spriteBatch;
        private CircuitRenderer cRend;
        private NXTCircuit circuit;
        private bool initialized = false;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public DrawEditor(NXTCircuit circuit)
            : base()
        {
            this.circuit = circuit;
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Initialise le Circuit et le CircuitRenderer
        public void InitializeCircuit(NXTCircuit c = null)
        {
            if (c != null)
                this.circuit = c;
            cRend = new CircuitRenderer(circuit, this.GraphicsDevice);
            initialized = true;
        }

        // Initialisation (de tout)
        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            background = RenderTools.LoadTextureFromFile(this.GraphicsDevice, "Data\\damier.png");
            InitializeCircuit();
        }

        // Appelé à chaque boucle logique
        protected override void Update(GameTime gameTime)
        {
            base.Update();
        }

        // Appelé à chaque boucle graphique
        protected override void Draw()
        {
            base.Draw();

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            if (circuit != null && initialized)
            {
                cRend.Render(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
