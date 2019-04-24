using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{
    // --------------------------------------------------------------------------
    // ENUMS / TYPES
    // --------------------------------------------------------------------------
    public enum Case { STRAIGHT, VIRAGE, INTERSECTION, EMPTY };
    public enum Orientation { TOP, RIGHT, BOTTOM, LEFT};

    [DataContract(Name = "Case", Namespace = "SgtSafety")]
    public class NXTCase : IExtensibleDataObject
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Case typeCase;
        private Orientation orientation;

        private ExtensionDataObject extensionData_Value;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        [DataMember]
        public Case TypeCase
        {
            get { return typeCase; }
            set { this.typeCase = value; }
        }
        [DataMember]
        public Orientation CaseOrientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }

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

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------
        public void NextCase()
        {
            typeCase = (Case)(((int)typeCase + 1) % 3);
        }

        public void NextOrientation()
        {
            orientation = (Orientation)(((int)orientation + 1) % 4);
        }

        public NXTCase Duplicate()
        {
            NXTCase c = new NXTCase(this.typeCase, this.orientation);
            return c;
        }
    }
}