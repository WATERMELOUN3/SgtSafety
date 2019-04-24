using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
        public NXTCircuit Circuit
        {
            get { return circuit; }
            set { circuit = value; }
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
        public static Point addPoint(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        //Retourne l'opposé d'une direction
        public static Point oppositeDirection(Point pDirection)
        {
            if (pDirection == RIGHT)
                return NXTVehicule.LEFT;
            else if (pDirection == LEFT)
                return NXTVehicule.RIGHT;
            else if (pDirection == TOP)
                return NXTVehicule.BOTTOM;
            else if (pDirection == BOTTOM)
                return NXTVehicule.TOP;

            return NXTVehicule.ERROR;
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
            Point newDir = caseCur.goThrough(action, this.direction);

            Console.WriteLine(this.position);
            this.position = addPoint(this.position, newDir);
            this.direction = newDir;
            Console.WriteLine(this.position);
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

    }
}