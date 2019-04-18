using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgtSafety.NXTBluetooth;
using SgtSafety.Types;


namespace SgtSafety.NXTEnvironment
{
    public class NXTVehicule
    {
        // FIELDS
        private Point position;
        private int patients;
        private Point direction;
        private NXTBuffer buffer;
        private NXTCircuit circuit;

        // GETTERS & SETTERS
        public Point getPosition()
        {
            return this.position;
        }

        public int getPatients()
        {
            return this.patients;
        }

        public Point getDirection()
        {
            return this.direction;
        }

        // CONSTRUCTORS
        public NXTVehicule(NXTBuffer p_buffer)
        {
            this.position = new Point(0);
            this.patients = 0;        
            this.direction = new Point(0);
            this.buffer = p_buffer;
            this.circuit = new NXTCircuit();
        }

        public NXTVehicule(Point p_position, Point p_direction, NXTBuffer p_buffer)
        {
            this.position = p_position;
            this.patients = 0;
            this.direction = p_direction;
            this.buffer = p_buffer;
            this.circuit = new NXTCircuit();
        }

        public NXTVehicule(Point p_position, Point p_direction, NXTBuffer p_buffer, NXTCircuit p_circuit)
        {
            this.position = p_position;
            this.patients = 0;
            this.direction = p_direction;
            this.buffer = p_buffer;
            this.circuit = p_circuit;
        }

        // METHODS

        private NXTCase currentCase()
        {
            return circuit.getCase(this.position);
        }

        public int takePatient(){
            return (++this.patients);
        }

        public int dropPatient(){
            return (--this.patients);
        }

        public void addToBuffer(NXTAction action)
        {
            this.buffer.Add(action, true);
        }

        public NXTAction executeCommand()
        {
            NXTAction action = buffer.Pop();

            return action;
        }

        /*private void computeVirage(NXTCase curCase)
        {
            Orientation orient = curCase.getOrientation();

            if (orient == Orientation.TOP || orient == Orientation.BOTTOM) //turn left
            {
                if (this.direction.X == 1 && this.direction.Y == 0)
                {
                    this.position += new Point(1, -1);
                    this.direction = new Point(0, -1);
                }
                else if (this.direction.X == -1 && this.direction.Y == 0)
                {
                    this.position += new Point(-1, 1);
                    this.direction = new Point(0, 1);
                }
                else if (this.direction.X == 0 && this.direction.Y == 1)
                {
                    this.position += new Point(1, 1);
                    this.direction = new Point(1, 0);
                }
                else if (this.direction.X == 0 && this.direction.Y == -1)
                {
                    this.position += new Point(-1, -1);
                    this.direction = new Point(-1, 0);
                }

            }
            else //turn right
            {
                if (this.direction.X == 1 && this.direction.Y == 0)
                {
                    this.position += new Point(1, 1);
                    this.direction = new Point(0, 1);
                }
                else if (this.direction.X == -1 && this.direction.Y == 0)
                {
                    this.position += new Point(-1, -1);
                    this.direction = new Point(0, -1);
                }
                else if (this.direction.X == 0 && this.direction.Y == 1)
                {
                    this.position += new Point(-1, 1);
                    this.direction = new Point(-1, 0);
                }
                else if (this.direction.X == 0 && this.direction.Y == -1)
                {
                    this.position += new Point(1, -1);
                    this.direction = new Point(1, 0);
                }

            }
        }

        private void computeIntersection_STRAIGHT(NXTCase curCase)
        {
            if (orient == Orientation.TOP || orient == Orientation.LEFT || orient == Orientation.RIGHT) 
            {
                this.position += this.direction;
            }
        }

        private void computeIntersection_LEFT(NXTCase curCase)
        {
            if (orient == Orientation.TOP || orient == Orientation.LEFT || orient == Orientation.BOTTOM)
            {
                this.direction = curCase.getOrientation();
                this.position += this.direction;
            }
            else
            {

            }
        }

        private void computeIntersection_RIGHT(NXTCase curCase)
        {
            if (orient == Orientation.TOP || orient == Orientation.BOTTOM || orient == Orientation.RIGHT)
            {
                this.position += this.direction;
            }
            else
            {

            }
        }

        private void moveStraight()
        {
            NXTCase curCase = currentCase();

            switch (curCase.getTypeCase())
            {
                case Case.STAIGHT:
                    this.position += this.direction;
                    break;

                case Case.VIRAGE:
                    computeVirage(curCase);
                    break;

                case Case.INTERSECTION:
                    computeIntersection_STRAIGHT(curCase);
                    break;

                default:
                    break;
            }
        }

        private void moveInterLeft()
        {
            NXTCase curCase = currentCase();

            switch (curCase.getTypeCase())
            {
                case Case.STAIGHT:
                    break;

                case Case.VIRAGE:
                    break;

                case Case.INTERSECTION:
                    computeIntersection_LEFT(curCase);
                    break;

                default:
                    break;
            }
        }

        private void moveInterRight()
        {
            NXTCase curCase = currentCase();

            switch (curCase.getTypeCase())
            {
                case Case.STAIGHT:
                    break;

                case Case.VIRAGE:
                    break;

                case Case.INTERSECTION:
                    computeIntersection_RIGHT(curCase);
                    break;

                default:
                    break;
            }
        }

        private void moveUTurn()
        {
            NXTCase curCase = currentCase();

            switch (curCase.getTypeCase())
            {
                case Case.STAIGHT:
                    this.position += this.direction;
                    break;

                case Case.VIRAGE:
                    computeVirage(curCase);
                    break;

                case Case.INTERSECTION:
                    computeIntersection_STRAIGHT(curCase);
                    break;

                default:
                    break;
            }
        }*/

    }
}