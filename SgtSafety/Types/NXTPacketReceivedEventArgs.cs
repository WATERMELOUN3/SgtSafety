using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Types
{
    public class NXTPacketReceivedEventArgs : EventArgs
    {
        private readonly byte[] data;

        public NXTPacketReceivedEventArgs(byte[] data)
        {
            this.data = data;
        }

        public byte[] Data
        {
            get { return data; }
        }
    }
}
