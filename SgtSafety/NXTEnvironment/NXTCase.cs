using SgtSafety.Types;
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

        //Retourne le point correspondant à un déplacement dans un virage selon la direction du vehicule
        private Point goThroughVirage(Point direction)
        {
            switch (this.orientation)
            {
                case Orientation.BOTTOM:
                    if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.LEFT;
                    return NXTVehicule.ERROR;

                case Orientation.LEFT:
                    if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.TOP)
                        return NXTVehicule.LEFT;
                    return NXTVehicule.ERROR;

                case Orientation.RIGHT:
                    if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.RIGHT;
                    return NXTVehicule.ERROR;

                case Orientation.TOP:
                    if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.TOP)
                        return NXTVehicule.RIGHT;
                    return NXTVehicule.ERROR;
            }

            return NXTVehicule.ERROR;
        }

        //Retourne le point correspondant à un déplacement STRAIGHT dans la case
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

        //Retourne le point correspondant à un déplacement UTURN
        private Point goUTurn(Point direction)
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

        //Retourne le point correspondant à une intersection prise à gauche
        private Point goThroughInterLeft(Point direction)
        {
            switch (this.orientation)
            {
                case Orientation.BOTTOM:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.RIGHT;
                    return NXTVehicule.ERROR;

                case Orientation.LEFT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.TOP;
                    return NXTVehicule.ERROR;

                case Orientation.RIGHT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;

                case Orientation.TOP:
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

        //Retourne le point correspondant à une intersection prise à droite
        private Point goThroughInterRight(Point direction)
        {
            switch (this.orientation)
            {
                case Orientation.BOTTOM:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;

                case Orientation.LEFT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.TOP;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.LEFT;
                    else if (direction == NXTVehicule.RIGHT)
                        return NXTVehicule.BOTTOM;
                    return NXTVehicule.ERROR;

                case Orientation.RIGHT:
                    if (direction == NXTVehicule.TOP)
                        return NXTVehicule.RIGHT;
                    else if (direction == NXTVehicule.BOTTOM)
                        return NXTVehicule.BOTTOM;
                    else if (direction == NXTVehicule.LEFT)
                        return NXTVehicule.TOP;
                    return NXTVehicule.ERROR;

                case Orientation.TOP:
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

        //Retourne le point correspondant à une intersection prise selon le coté donné en paramètre
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

        //Retourne le point correspondant au déplacement effectué dans la case selon l'action donnée en paramètre
        public Point goThrough(NXTAction action, Point direction)
        {
            char movement = action.Movement;

            if (movement == NXTMovement.STRAIGHT)
                return goStraight(direction);
            else if (movement == NXTMovement.INTER_LEFT || movement == NXTMovement.INTER_RIGHT)
                return turnInter(direction, movement);
            else if (movement == NXTMovement.UTURN)
                return goUTurn(direction);

            return new Point(1, 1);
        }
    }
}