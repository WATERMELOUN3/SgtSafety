using SgtSafety.NXTBluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgtSafety.Types;

namespace SgtSafety.NXTEnvironment
{
    public class NXTBuffer : List<NXTAction>
    {
        // CONSTRUCTOR
        public NXTBuffer()
            : base()
        { }

        // METHODS
        public void Add(NXTAction p, bool FIFO = true)
        {
            if (FIFO)
                base.Insert(0, p);
            else
                base.Add(p);
        }

        public NXTAction Pop()
        {
            NXTAction p = base[0];
            base.RemoveAt(0);

            return p;
        }

        public bool isEmpty()
        {
            return (this.Count() == 0);
        }
    }
}
