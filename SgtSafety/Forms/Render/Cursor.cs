using Microsoft.Xna.Framework;
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
        private Vector2 realLocation;
        private Vector2 cursorLocation;

        private NXTCase cCase;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public Cursor()
        {
            cCase = new NXTCase();
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------
    }
}
