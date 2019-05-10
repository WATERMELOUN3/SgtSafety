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
            this.buffer = p_vehicule.Buffer;
            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }

        public void AddToIABuffer(List<Point> path)
        {
            NXTAction action;
            Point oldPos = vehicule.Position;
            Point direction = vehicule.Direction;

            NXTCase currentCase = circuit.getCase(oldPos);
            if (!((currentCase.goThrough(new NXTAction(NXTMovement.STRAIGHT), direction) + oldPos).Equals(path[0])
                || (currentCase.goThrough(new NXTAction(NXTMovement.INTER_LEFT), direction) + oldPos).Equals(path[0])
                || (currentCase.goThrough(new NXTAction(NXTMovement.INTER_RIGHT), direction) + oldPos).Equals(path[0])))
            {
                this.buffer.Add(new NXTAction(NXTMovement.UTURN), false);
                direction = Rotate90Clockwise(Rotate90Clockwise(direction));
            }

            foreach (Point p in path)
            {
                action = MovementToAction(circuit.getCase(oldPos), oldPos, direction, p, out direction);

                if (circuit.hasPatient(p) && vehicule.Patients < vehicule.MAX_PATIENTS)
                    action.Action = NXTAction.TAKE;
                else if (circuit.hasHopital(p) && vehicule.Patients > 0)
                    action.Action = NXTAction.DROP;

                this.buffer.Add(action, false);
                oldPos = p;
            }
        }

        protected NXTAction MovementToAction(NXTCase currentCase, Point currentPosition, Point currentDirection, Point destination, out Point newDirection)
        {
            NXTAction outInstance = null;
            newDirection = currentDirection;
            Point deplacement = destination - currentPosition;
            Point caseDirection = OrientationToDirection(currentCase.CaseOrientation);

            //Si le robot doit marquer une pause
            if (destination.Equals(currentPosition))
                outInstance = new NXTAction(NXTMovement.PAUSE);

            // Si destination derriere direction actuelle, demi tour
            if (destination.Equals(currentPosition - currentDirection))
                outInstance = new NXTAction(NXTMovement.UTURN);
            // Sinon si virage ou tout droit envoyer straight
            else if (currentCase.TypeCase == Case.STRAIGHT || currentCase.TypeCase == Case.VIRAGE)
            {
                outInstance = new NXTAction(NXTMovement.STRAIGHT);

                if (currentCase.TypeCase == Case.VIRAGE)
                {
                    newDirection = currentCase.goThrough(new NXTAction(NXTMovement.STRAIGHT), currentDirection);
                }
            }
            // sinon (intersection)
            else
            {
                //Console.WriteLine("[INTERSECTION] Deplacement= " + deplacement);
                // Si la case est dans la meme direction que le vehicule
                if (caseDirection.Equals(currentDirection))
                {
                    if (deplacement.Equals(Rotate90Clockwise(currentDirection)))
                    {
                        outInstance = new NXTAction(NXTMovement.INTER_RIGHT);
                        newDirection = Rotate90Clockwise(currentDirection);
                    }
                    else
                    {
                        outInstance = new NXTAction(NXTMovement.INTER_LEFT);
                        newDirection = Rotate90AntiClockwise(currentDirection);
                    }
                }
                else if (caseDirection.Equals(Rotate90Clockwise(currentDirection))) // r = tout droit
                {
                    if (deplacement.Equals(Rotate90AntiClockwise(currentDirection)))
                    {
                        outInstance = new NXTAction(NXTMovement.INTER_LEFT);
                        newDirection = Rotate90AntiClockwise(currentDirection);
                    }
                    else
                        outInstance = new NXTAction(NXTMovement.INTER_RIGHT);

                }
                else if (caseDirection.Equals(Rotate90AntiClockwise(currentDirection))) // l = tout droit
                {
                    if (deplacement.Equals(Rotate90Clockwise(currentDirection)))
                    {
                        outInstance = new NXTAction(NXTMovement.INTER_RIGHT);
                        newDirection = Rotate90Clockwise(currentDirection);
                    }
                    else
                        outInstance = new NXTAction(NXTMovement.INTER_LEFT);
                }
            }

            Console.WriteLine(currentCase + " @ " + currentPosition + " -> " + destination + " _ " + DirectionToOrientation(newDirection) + "\nResult= " + outInstance);

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
