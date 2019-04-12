using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTBluetooth
{
    public class NXTPacket
    {
        // FIELDS
        List<NXTAction> actions;

        // CONSTRUCTOR
        public NXTPacket()
        {
            actions = new List<NXTAction>();
        }

        public NXTPacket(List<NXTAction> actions)
        {
            this.actions = actions;
        }

        public NXTPacket(NXTAction[] actions)
        {
            this.actions = new List<NXTAction>();
            foreach (NXTAction a in actions)
            {
                this.actions.Add(a);
            }
        }

        // METHODS
        public bool addAction(NXTAction a)
        {
            actions.Add(a);

            return true;
        }


    }
}
