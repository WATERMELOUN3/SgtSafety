using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgtSafety.NXTBluetooth;


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
        public NXTVehicule(NXTBuffer p_buffer){
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

        /*public Point executeCommand()
        {
            NXTPacket packet = buffer.Pop();
            string data = System.Text.Encoding.UTF8.GetString(packet.getPacketData());

            if (data.Equals(""))
            {

            }
        }*/

        /*private Point moveStraight(){

            switch (currentCase())
            {
                case STRAIGHT:
                    this.position += this.direction;
                    break;

                case TURNLEFT:
                    if (this.direction.X == 1 && this.direction.Y == 0)
                    {
                        this.position += new Point(1, -1);
                        this.direction = new Point(0, -1);
                    } else if (this.direction.X == -1 && this.direction.Y == 0)
                    {
                        this.position += new Point(-1, 1);
                        this.direction = new Point(0, 1);
                    } else if(this.direction.X == 0 && this.direction.Y == 1)
                    {
                        this.position += new Point(1, 1);
                        this.direction = new Point(1, 0);
                    } else if(this.direction.X == 0 && this.direction.Y == -1)
                    {
                        this.position += new Point(-1, -1);
                        this.direction = new Point(-1, 0);
                    }
                    else
                    {
                        //erreur jsp quoi mettre
                    }
                    break;

                case TURNRIGHT:
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
                    else
                    {
                        //erreur jsp quoi mettre
                    }
                    break;

                case INTERSECTION:
                    break;

                case default:
                    //erreur jsp quoi mettre
                    break;
            }

            return this.position;
        }*/
    }
}