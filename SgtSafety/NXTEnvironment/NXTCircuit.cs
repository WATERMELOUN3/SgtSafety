using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{
    [DataContract(Name = "Circuit", Namespace = "SgtSafety")]
    public class NXTCircuit : IExtensibleDataObject
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private int height;
        private int width;
        private NXTCase[] circuit; // i = x + width * y

        private List<Point> hopitaux;
        private List<Point> patients;

        private ExtensionDataObject extensionData_Value;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        [DataMember]
        public string Nom { get; set; }
        [DataMember]
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }
        [DataMember]
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }
        [DataMember]
        public NXTCase[] Circuit
        {
            get { return this.circuit; }
            set { this.circuit = value; }
        }
        [DataMember]
        public List<Point> Hopitaux
        {
            get { return this.hopitaux; }
            set { this.hopitaux = value; }
        }
        [DataMember]
        public List<Point> Patients
        {
            get { return this.patients; }
            set { this.patients = value; }
        }
        public ExtensionDataObject ExtensionData
        {
            get
            {
                return extensionData_Value;
            }
            set
            {
                extensionData_Value = value;
            }
        }

        // --------------------------------------------------------------------------
        // CONSTRUCTORS
        // --------------------------------------------------------------------------
        public NXTCircuit(){
            this.height = 10;
            this.width = 10;
            this.circuit = new NXTCase[this.height * this.width];
            this.hopitaux = new List<Point>();
            this.patients = new List<Point>();

            initialiseCircuit();
        }

        public NXTCircuit(int p_width, int p_height)
        {
            this.height = p_height;
            this.width = p_width;
            this.circuit = new NXTCase[this.height * this.width];
            this.hopitaux = new List<Point>();
            this.patients = new List<Point>();

            initialiseCircuit();
        }

        public NXTCircuit(int p_width, int p_height, NXTCase[] p_circuit){
            this.height = p_height;
            this.width = p_width;
            this.circuit = p_circuit;
            this.hopitaux = new List<Point>();
            this.patients = new List<Point>();
        } 

        public NXTCircuit(int p_width, int p_height, NXTCase[] p_circuit, List<Point> p_hopitaux, List<Point> p_patients){
            this.height = p_height;
            this.width = p_width;
            this.circuit = p_circuit;
            this.hopitaux = p_hopitaux;
            this.patients = p_patients;
        }


        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Initialise le circuit avec des cases (vides par défaut)
        private void initialiseCircuit(Case c = Case.EMPTY){
            for (int i=0; i < this.height; i++)
                for (int j=0; j < this.width; j++)
                    circuit[i + j * this.width] = new NXTCase(c);

        }

        // Ajoute un hopital
        public List<Point> addHopital(Point newHopital){
            if (!hopitaux.Contains(newHopital))
                hopitaux.Add(newHopital);

            return this.hopitaux;
        }

        // Ajoute un patient
        public List<Point> addPatient(Point newPatient){
            if (!patients.Contains(newPatient))
                patients.Add(newPatient);

            return this.patients;
        }

        // Défini la case voulu
        public NXTCase[] setCase(Point coordCase, NXTCase newCase){
            this.circuit[coordCase.X + this.width * coordCase.Y] = newCase;

            return this.circuit;
        }

        // Surcharge avec int à la place de Point
        public NXTCase[] setCase(int x, int y, NXTCase newCase)
        {
            this.circuit[x + this.width * y] = newCase;

            return this.circuit;
        }

        // Obtient la case aux coordonnées choisis
        public NXTCase getCase(Point position)
        {
            return this.circuit[position.X + this.width * position.Y];
        }

        // Surcharge avec int à la place de Point
        public NXTCase getCase(int x, int y)
        {
            return this.circuit[x + this.width * y];
        }

        // Vérifie si des coordonnées sont compris dans la dimension du circuit
        public bool IsWithinBounds(Point p)
        {
            if (p.X >= 0 && p.X < this.width && p.Y >= 0 && p.Y < this.height)
                return true;
            else
                return false;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return IsWithinBounds(new Point(x, y));
        }
    }
}