using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{
    public enum Case { STRAIGHT, VIRAGE, INTERSECTION, EMPTY };
    public enum Orientation { TOP, BOTTOM, LEFT, RIGHT};

    public class NXTCase
    {
        private Case typeCase;
        private Orientation orientation;

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

        public Case getTypeCase()
        {
            return this.typeCase;
        }

        public Orientation getOrientation()
        {
            return this.orientation;
        }

        public void setOrientation(Orientation p_orientation)
        {
            this.orientation = p_orientation;
        }
    }
}