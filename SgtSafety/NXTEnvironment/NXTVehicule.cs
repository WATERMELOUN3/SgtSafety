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
        public static readonly Point ERROR = new Point(int.MinValue, int.MinValue);

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
            set { position = value; }
        }
        public int Patients
        {
            get { return patients; }
        }
        public Point Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public NXTCircuit Circuit
        {
            get { return circuit; }
            set { circuit = value; }
        }
        public Orientation ToOrientation
        {
            get
            {
                if (this.direction.Equals(TOP))
                    return Orientation.TOP;
                else if (this.direction.Equals(RIGHT))
                    return Orientation.RIGHT;
                else if (this.direction.Equals(LEFT))
                    return Orientation.LEFT;
                else
                    return Orientation.BOTTOM;
            }
        }
        public int MAX_PATIENTS
        {
            get { return 1; }
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

        //Retourne l'oppos� d'une direction
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

        // Prend un patient (renvoie true si le patient a �t� pris, sinon renvoie false)
        public bool takePatient(Point p){
            if (patients < MAX_PATIENTS)
            {
                patients++;
                circuit.RemovePatient(p);
                return true;
            }

            return false;
        }

        // Lache un patient (renvoie le nombre de patients apr�s)
        public int dropPatient(){
            return (this.patients = 0);
        }

        // Ajoute une action au buffer du vehicule
        public void addToBuffer(NXTAction action)
        {
            this.buffer.Add(action, false);
        }

        //Execute l'action envoy�e en param�tre
        private void executeAction(NXTAction action)
        {
            char actiontd = action.Action;
            NXTCase caseCur = currentCase();
            Point newDir = ERROR;
            if (actiontd != NXTMovement.PAUSE)
                newDir = caseCur.goThrough(action, this.direction);

            //Console.WriteLine(this.position);
            if (newDir != ERROR && action.Movement != NXTMovement.UTURN)
                this.position = this.position + newDir;

            if (actiontd != NXTMovement.PAUSE)
                this.direction = newDir;

            if (actiontd == NXTAction.TAKE)
                this.takePatient(this.position);
            else if (actiontd == NXTAction.DROP)
                this.dropPatient();
        }

        // Retourne la prochaine action � executer, ou null si il n'y a pas d'action
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

        // Envoie le paquet de la prochaine action � effectuer (true), ou renvoie false si il n'y a plus d'actions
        public bool SendNextAction(bool simulation = false)
        {
            NXTAction action = this.executeCommand();
            if (action != null)
            {
                Console.WriteLine("Ordre envoy�: " + action.ToString());

                if (action.Movement == NXTMovement.PAUSE)
                    Sleep(action.Temporisation);

                if (!simulation && action.Action != NXTAction.PAUSE)
                {
                    NXTPacket packet = new NXTPacket(action);
                    nxtHelper.SendNTXPacket(packet);
                }

                return true;
            }
            else
                return false;
        }

        private async void Sleep(int ms)
        {
            await Task.Delay(ms);
        }

        public void ClearBuffer()
        {
            this.buffer.Clear();
        }

    }
}