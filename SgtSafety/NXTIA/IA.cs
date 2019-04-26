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
        protected NXTAction pStraight, pLeft, pRight, pUturn;

        public IA()
        {
            this.vehicule = new NXTVehicule();
            this.circuit = vehicule.Circuit;
            this.buffer = new NXTBuffer();
            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }

        public IA(NXTVehicule p_vehicule)
        {
            this.vehicule = p_vehicule;
            this.circuit = vehicule.Circuit;
            this.buffer = new NXTBuffer();
            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }

        protected bool addToBuffer(NXTAction action)
        {
            buffer.Add(action);
            return true;
        }

        protected bool sendToVehiculeBuffer()
        {
            while (!buffer.isEmpty())
                vehicule.addToBuffer(buffer.Pop());

            return true;
        }
    }
}
