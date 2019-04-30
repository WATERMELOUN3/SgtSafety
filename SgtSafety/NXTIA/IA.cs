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

        protected void AddToIABuffer(List<Point> path)
        {
            NXTAction action;
            foreach(Point p in path)
            {
                action = pStraight;
                addToBuffer(action);
            }
        }

        protected NXTAction MovementToAction(NXTCase currentCase, Point currentPosition, Point currentDirection, Point destination)
        {
            NXTAction outInstance = null;
            if (destination.Equals(currentPosition - currentDirection))
                outInstance = new NXTAction(NXTMovement.UTURN);
            else if (currentCase.TypeCase == Case.STRAIGHT || currentCase.TypeCase == Case.VIRAGE)
                outInstance = new NXTAction(NXTMovement.STRAIGHT);
            else
            {
                Point caseDirection = OrientationToDirection(currentCase.CaseOrientation);
                Point deplacement = destination - currentPosition;

                // Si la case est dans la meme direction que le vehicule
                if (caseDirection.Equals(currentDirection))
                {
                    if (deplacement.Equals(Rotate90Clockwise(currentDirection)))
                        outInstance = new NXTAction(NXTMovement.INTER_RIGHT);
                    else
                        outInstance = new NXTAction(NXTMovement.INTER_LEFT);
                }
                else if (caseDirection.Equals(Rotate90Clockwise(currentDirection))) // r = tout droit
                {
                    if (deplacement.Equals(Rotate90AntiClockwise(currentDirection)))
                        outInstance = new NXTAction(NXTMovement.INTER_LEFT);
                    else
                        outInstance = new NXTAction(NXTMovement.INTER_RIGHT);
                        
                }
                else if (caseDirection.Equals(Rotate90AntiClockwise(currentDirection))) // l = tout droit
                {
                    if (deplacement.Equals(Rotate90Clockwise(currentDirection)))
                        outInstance = new NXTAction(NXTMovement.INTER_RIGHT);
                    else
                        outInstance = new NXTAction(NXTMovement.INTER_LEFT);
                }
            }

            return outInstance;
        }

        protected Point OrientationToDirection(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.TOP:
                    return NXTVehicule.TOP;
                case Orientation.RIGHT:
                    return NXTVehicule.RIGHT;
                case Orientation.BOTTOM:
                    return NXTVehicule.BOTTOM;
                case Orientation.LEFT:
                    return NXTVehicule.LEFT;
                default:
                    return NXTVehicule.TOP;
            }
        }

        protected Orientation DirectionToOrientation(Point direction)
        {
            if (direction.Equals(NXTVehicule.TOP))
                return Orientation.TOP;
            else if (direction.Equals(NXTVehicule.RIGHT))
                return Orientation.RIGHT;
            else if (direction.Equals(NXTVehicule.LEFT))
                return Orientation.LEFT;
            else
                return Orientation.BOTTOM;
        }

        protected Point Rotate90Clockwise(Point p)
        {
            Orientation o = DirectionToOrientation(p);
            o = (Orientation)(((int)o + 1) % 4);

            return OrientationToDirection(o);
        }

        protected Point Rotate90AntiClockwise(Point p)
        {
            Orientation o = DirectionToOrientation(p);
            o = (Orientation)(((int)o + 3) % 4);

            return OrientationToDirection(o);
        }
    }
}
