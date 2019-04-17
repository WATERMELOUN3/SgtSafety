using SgtSafety.NXTBluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{
    public class NXTBuffer : List<NXTPacket>
    {
        // CONSTRUCTOR
        public NXTBuffer()
            : base()
        { }

        // METHODS
        public void Add(NXTPacket p, bool FIFO = true)
        {
            if (FIFO)
                base.Insert(0, p);
            else
                base.Add(p);
        }

        public NXTPacket Pop()
        {
            NXTPacket p = base[0];
            base.RemoveAt(0);

            return p;
        }
    }
}
