using Microsoft.Xna.Framework;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
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
        private Color color;

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
        [DataMember]
        public Color CaseColor
        {
            get { return color; }
            set { color = value; }
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
            this.color = Color.White;
            this.typeCase = Case.EMPTY;
            this.orientation = Orientation.TOP;
        }

        public NXTCase(Case p_type)
        {
            this.color = Color.White;
            this.typeCase = p_type;
            this.orientation = Orientation.TOP;
        }

        public NXTCase(Case p_type, Orientation p_orientation)
        {
            this.color = Color.White;
            this.typeCase = p_type;
            this.orientation = p_orientation;
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------
        public override string ToString()
        {
            return "[NXTCase Color=" + color + ", TypeCase=" + typeCase + ", Orientation=" + orientation + "]";
        }

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
        //Retourne le point correspondant � un d�placement dans un virage selon la direction du vehicule
        private Point goThroughVirage(Point direction)
        {
            switch (this.orientation)
            {
                case Orientation.BOTTOM:
                    if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.TOP) // weird
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.LEFT;
                    return NXTVehicule.ERROR;

                case Orientation.LEFT:
                    if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.RIGHT) // weird
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.TOP)
                        return NXTVehicule.TOP;
                    return NXTVehicule.ERROR;

                case Orientation.RIGHT:
                    if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.TOP)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.BOTTOM) // weird
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.LEFT;
                    return NXTVehicule.ERROR;

                case Orientation.TOP:
                    if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.TOP)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.RIGHT) // weird
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;
            }

            return NXTVehicule.ERROR;
        }

        //Retourne le point correspondant � un d�placement STRAIGHT dans la case
        private Point goStraight(Point direction)
        {
            switch (this.typeCase)
            {
                case Case.STRAIGHT:
                    return direction;
                case Case.VIRAGE:
                    return goThroughVirage(direction);
                case Case.INTERSECTION:
                    return NXTVehicule.ERROR;
                default:
                    return NXTVehicule.ERROR;
            }
        }

        //Retourne le point correspondant � un d�placement UTURN
        private Point goUTurn(Point direction)
        {
            Point newDirection = NXTVehicule.oppositeDirection(direction);
            switch (this.typeCase)
            {
                case Case.STRAIGHT:
                    return newDirection;
                case Case.VIRAGE:
                    return newDirection;
                case Case.INTERSECTION:
                    Console.WriteLine("/!\\ Demi tour sur intersection /!\\");
                    return NXTVehicule.ERROR;
                default:
                    return NXTVehicule.ERROR;
            }
        }

        //Retourne le point correspondant � une intersection prise � gauche
        private Point goThroughInterLeft(Point direction)
        {
            switch (this.orientation)
            {
                case Orientation.TOP:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.RIGHT;
                    return NXTVehicule.ERROR;

                case Orientation.RIGHT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.TOP;
                    return NXTVehicule.ERROR;

                case Orientation.LEFT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;

                case Orientation.BOTTOM:
                    if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.TOP;
                    return NXTVehicule.ERROR;
            }

            return NXTVehicule.ERROR;
        }

        //Retourne le point correspondant � une intersection prise � droite
        private Point goThroughInterRight(Point direction)
        {
            switch (this.orientation)
            {
                case Orientation.TOP:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;

                case Orientation.RIGHT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;

                case Orientation.LEFT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.TOP;
                    return NXTVehicule.ERROR;

                case Orientation.BOTTOM:
                    if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.RIGHT;
                    return NXTVehicule.ERROR;
            }

            return NXTVehicule.ERROR;
        }

        //Retourne le point correspondant à une intersection prise selon le cot� donn� en param�tre
        private Point turnInter(Point direction, char side)
        {
            switch (this.typeCase)
            {
                case Case.STRAIGHT:
                    return NXTVehicule.ERROR;
                case Case.VIRAGE:
                    return NXTVehicule.ERROR;
                case Case.INTERSECTION:
                    if (side == NXTMovement.INTER_LEFT)
                        return goThroughInterLeft(direction);
                    else if (side == NXTMovement.INTER_RIGHT)
                        return goThroughInterRight(direction);
                    return NXTVehicule.ERROR;
                default:
                    return NXTVehicule.ERROR;
            }
        }

        //Retourne le point correspondant au déplacement effectu� dans la case selon l'action donn�e en param�tre
        public Point goThrough(NXTAction action, Point direction)
        {
            char movement = action.Movement;

            if (movement == NXTMovement.STRAIGHT)
                return goStraight(direction);
            else if (movement == NXTMovement.INTER_LEFT || movement == NXTMovement.INTER_RIGHT)
                return turnInter(direction, movement);
            else if (movement == NXTMovement.UTURN)
                return goUTurn(direction);

            return NXTVehicule.ERROR;
        }
    }
}