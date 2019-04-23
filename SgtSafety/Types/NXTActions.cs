using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Types
{
    // --------------------------------------------------------------------------
    // TYPE ABSTRAIT NXTMovement
    // --------------------------------------------------------------------------
    public abstract class NXTMovement
    {
        // --------------------------------------------------------------------------
        // CONSTANTS
        // --------------------------------------------------------------------------
        public const char STRAIGHT = 's';
        public const char INTER_RIGHT = 'r';
        public const char INTER_LEFT = 'l';
        public const char UTURN = 'u';
    }

    public class NXTAction : NXTMovement
    {
        // --------------------------------------------------------------------------
        // CONSTANTS
        // --------------------------------------------------------------------------
        public const char DROP = 'd';
        public const char TAKE = 't';

        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private char movement;

        private bool hasAction = false;
        private char action = ' ';

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        public char Movement
        {
            get { return movement; }
            set { movement = value; }
        }
        public char Action
        {
            get
            {
                if (hasAction)
                    return action;
                else
                    return ' ';
            }
            set
            {
                if (value == ' ')
                {
                    hasAction = false;
                }
                else
                {
                    hasAction = true;
                    action = value;
                }
            }
        }

        // --------------------------------------------------------------------------
        // CONSTRUCTORS
        // --------------------------------------------------------------------------
        public NXTAction(char movement)
        {
            this.movement = movement;
        }

        public NXTAction(char movement, char action)
        {
            this.movement = movement;

            if (action != ' ')
            {
                this.action = action;
                this.hasAction = true;
            }
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Duplique l'instance actuelle dans une nouvelle instance
        public NXTAction Duplicate()
        {
            NXTAction a = new NXTAction(this.movement, this.action);
            return a;
        }

        // Donne la chaîne de caractères (pour envoyer le paquet)
        public override string ToString()
        {
            return movement + (hasAction ? action.ToString() : "") + "\n";
        }

        // Donne la chaîne de caractères rédigés, pour l'affichage
        public string ToFancyString()
        {
            string t = "";
            switch (this.movement)
            {
                case NXTMovement.STRAIGHT:
                    t = "Tout droit";
                    break;
                case NXTMovement.UTURN:
                    t = "Demi-tour";
                    break;
                case NXTMovement.INTER_LEFT:
                    t = "Tourner à gauche";
                    break;
                case NXTMovement.INTER_RIGHT:
                    t = "Tourner à droite";
                    break;
                default:
                    t = "Donnée inconnue";
                    break;
            }

            if (hasAction)
            {
                t += " puis ";
                switch (this.action)
                {
                    case NXTAction.TAKE:
                        t += "prendre patient";
                        break;
                    case NXTAction.DROP:
                        t += "poser patient";
                        break;
                    default:
                        t += "donnée inconnue";
                        break;
                }
            }

            return t;
        }
    }
}
