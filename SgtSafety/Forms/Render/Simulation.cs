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
    public class Simulation : MonoGame.Forms.Controls.UpdateWindow
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Texture2D tPixel;
        private Texture2D background;
        private NXTVehicule vehicule;
        private KeyboardState oldKeyState;

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


        }
    }
}
