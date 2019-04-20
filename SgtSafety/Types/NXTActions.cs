using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Types
{
    public class NXTMovement
    {
        public const char STRAIGHT = 's';
        public const char INTER_RIGHT = 'r';
        public const char INTER_LEFT = 'l';
        public const char UTURN = 'u';
    }

    public class NXTAction : NXTMovement
    {
        public const char DROP = 'd';
        public const char TAKE = 't';

        private char movement;

        private bool hasAction = false;
        private char action = ' ';

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

        public void SetAction(char a)
        {
            if (a == ' ')
            {
                hasAction = false;
            }
            else
            {
                hasAction = true;
                action = a;
            }

        }

        public NXTAction Duplicate()
        {
            NXTAction a = new NXTAction(this.movement, this.action);
            return a;
        }

        public override string ToString()
        {
            return movement + (hasAction ? action.ToString() : "") + "\n";
        }

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
