using SgtSafety.NXTEnvironment;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    class IA
    {
        protected NXTVehicule vehicule;
        protected NXTCircuit circuit;
        protected NXTBuffer buffer;

        public IA()
        {
            this.vehicule = new NXTVehicule();
            this.circuit = vehicule.Circuit;
        }

        public IA(NXTVehicule p_vehicule)
        {
            this.vehicule = p_vehicule;
            this.circuit = vehicule.Circuit;
        }

        public bool addToBuffer(NXTAction action)
        {
            buffer.Add(action);
            return true;
        }

        public bool sendToVehiculeBuffer()
        {
            while (!buffer.isEmpty())
                vehicule.addToBuffer(buffer.Pop());

            return true;
        }
    }
}
