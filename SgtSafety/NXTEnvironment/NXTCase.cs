using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{
    // --------------------------------------------------------------------------
    // ENUMS / TYPES
    // --------------------------------------------------------------------------
    public enum Case { STRAIGHT, VIRAGE, INTERSECTION, EMPTY };
    public enum Orientation { TOP, BOTTOM, LEFT, RIGHT};

    public class NXTCase
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Case typeCase;
        private Orientation orientation;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        public Case TypeCase { get { return typeCase; } }
        public Orientation CaseOrientation { get { return orientation; } set { orientation = value; } }

        // --------------------------------------------------------------------------
        // CONSTRUCTORS
        // --------------------------------------------------------------------------
        public NXTCase()
        {
            this.typeCase = Case.EMPTY;
            this.orientation = Orientation.TOP;
        }

        public NXTCase(Case p_type)
        {
            this.typeCase = p_type;
            this.orientation = Orientation.TOP;
        }

        public NXTCase(Case p_type, Orientation p_orientation)
        {
            this.typeCase = p_type;
            this.orientation = p_orientation;
        }
    }
}