using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Forms.Render
{
    public static class RenderTools
    {
        public static Texture2D LoadTextureFromFile(GraphicsDevice graphicsDevice, string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            Texture2D spriteAtlas = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Dispose();

            return spriteAtlas;
        }
    }
}
