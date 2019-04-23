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
        // --------------------------------------------------------------------------
        // CONSTANTS
        // --------------------------------------------------------------------------
        public static readonly Point TOP = new Point(0, -1);
        public static readonly Point BOTTOM = new Point(0, 1);
        public static readonly Point LEFT = new Point(-1, 0);
        public static readonly Point RIGHT = new Point(1, 0);
        public static readonly Point ERROR = new Point(-5, -5);

        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private Point position;
        private int patients;
        private Point direction;
        private NXTBuffer buffer;
        private NXTCircuit circuit;
        private NXTBluetoothHelper nxtHelper;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        public NXTBluetoothHelper NxtHelper
        {
            get { return nxtHelper; }
        }
        public NXTBuffer Buffer
        {
            get { return buffer; }
        }
        public bool IsBusy { get; set; }
        public Point Position
        {
            get { return position; }
        }
        public int Patients
        {
            get { return patients; }
        }
        public Point Direction
        {
            get { return direction; }
        }

        // --------------------------------------------------------------------------
        // CONSTRUCTORS
        // --------------------------------------------------------------------------
        public NXTVehicule()
        {
            this.position = new Point(0);
            this.patients = 0;        
            this.direction = new Point(0);
            this.buffer = new NXTBuffer();
            this.circuit = new NXTCircuit();
            IsBusy = false;
            nxtHelper = new NXTBluetoothHelper();
        }

        public NXTVehicule(Point p_position, Point p_direction)
        {
            this.position = p_position;
            this.patients = 0;
            this.direction = p_direction;
            this.buffer = new NXTBuffer();
            this.circuit = new NXTCircuit();
            IsBusy = false;
            nxtHelper = new NXTBluetoothHelper();
        }

        public NXTVehicule(Point p_position, Point p_direction, NXTCircuit p_circuit)
        {
            this.position = p_position;
            this.patients = 0;
            this.direction = p_direction;
            this.buffer = new NXTBuffer();
            this.circuit = p_circuit;
            IsBusy = false;
            nxtHelper = new NXTBluetoothHelper();
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Addition de points, à déplacer dans classe statique "MathTools"
        private static Point addPoint(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        // Retourne la case sur laquelle de vehicule est
        private NXTCase currentCase()
        {
            return circuit.getCase(this.position);
        }

        // Prend un patient (renvoie le nombre de patients après)
        private int takePatient(){
            return (++this.patients);
        }

        // Lache un patient (renvoie le nombre de patients après)
        private int dropPatient(){
            return (--this.patients);
        }

        // Ajoute une action au buffer du vehicule
        public void addToBuffer(NXTAction action)
        {
            this.buffer.Add(action, false);
        }

        //Execute l'action envoyée en paramètre
        private void executeAction(NXTAction action)
        {
            char actiontd = action.Action;
            NXTCase caseCur = currentCase();

            this.position = addPoint(this.position, caseCur.goThrough(action, this.direction));
            if (actiontd == NXTAction.TAKE)
                this.takePatient();
            else if (actiontd == NXTAction.DROP)
                this.dropPatient();
        }

        // Retourne la prochaine action à executer, ou null si il n'y a pas d'action
        public NXTAction executeCommand()
        {
            NXTAction action = null;
            if (!buffer.isEmpty())
            {
                action = buffer.Pop();
                executeAction(action);
            }

            return action;
        }

        // Envoie le paquet de la prochaine action à effectuer (true), ou renvoie false si il n'y a plus d'actions
        public bool SendNextAction()
        {
            NXTAction action = this.executeCommand();
            if (action != null)
            {
                Console.WriteLine("Ordre envoyé: " + action.ToString());
                NXTPacket packet = new NXTPacket(action);
                nxtHelper.SendNTXPacket(packet);

                return true;
            }
            else
                return false;
        }

        /* private void moveStraight()
         {
             NXTCase curCase = currentCase();

             switch (curCase.getTypeCase())
             {
                 case Case.STRAIGHT:
                     this.position = addPoint(this.position, this.direction);
                     break;

                 case Case.VIRAGE:
                     //computeVirage(curCase);
                     break;

                 case Case.INTERSECTION:
                     //computeIntersection_STRAIGHT(curCase);
                     break;

                 default:
                     break;
             }
         }*/

        /*private void computeVirage(NXTCase curCase)
        {
            Orientation orient = curCase.getOrientation();

            switch (orient)
            {
                case Orientation.BOTTOM:
                    break;

                case Orientation.TOP:
                    break;

                case Orientation.LEFT:
                    break;

                case Orientation.RIGHT:
                    break;
            }
        }*/

        /*private void computeIntersection_STRAIGHT(NXTCase curCase)
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