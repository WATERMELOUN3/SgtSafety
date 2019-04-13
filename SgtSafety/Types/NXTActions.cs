﻿using System;
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
        private char action;

        public NXTAction(char movement)
        {
            this.movement = movement;
        }

        public NXTAction(char movement, char action)
        {
            this.movement = movement;
            this.action = action;
            this.hasAction = true;
        }

        public override string ToString()
        {
            return movement + (hasAction ? action.ToString() : "") + "\n";
        }
    }
}
