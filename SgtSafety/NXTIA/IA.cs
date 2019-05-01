using Microsoft.Xna.Framework;
using SgtSafety.NXTEnvironment;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    public class IA
    {
        protected NXTVehicule vehicule;
        protected NXTCircuit circuit;
        protected NXTBuffer buffer;
        protected NXTAction pStraight, pLeft, pRight, pUturn;
        protected bool simulation;

        public IA(bool p_simulation = false)
        {
            this.vehicule = new NXTVehicule();
            this.circuit = vehicule.Circuit;
            this.buffer = new NXTBuffer();
            this.simulation = p_simulation;
            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }

        public IA(NXTVehicule p_vehicule, bool p_simulation = false)
        {
            this.vehicule = p_vehicule;
            this.circuit = vehicule.Circuit;
            this.buffer = new NXTBuffer();
            this.simulation = p_simulation;
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
            if (!simulation)
                while (!buffer.isEmpty())
                    vehicule.addToBuffer(buffer.Pop());

            return true;
        }

        protected void AddToIABuffer(List<Point> path)
        {
            NXTAction action;
            foreach(Point p in path)
            {
                action = pStraight;
                addToBuffer(action);
            }
        }
    }
}
