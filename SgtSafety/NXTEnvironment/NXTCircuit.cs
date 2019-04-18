using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{

    public class NXTCircuit
    {
        // FIELDS
        private int height;
        private int width;
        private NXTCase[] circuit; // i = x + width * y

        private List<Point> hopitaux;
        private List<Point> patients;

        // GETTERS & SETTERS
        public string Nom { get; set; }
        public int getWidth()
        {
            return this.width;
        }

        public int getHeight()
        {
            return this.height;
        }

        public NXTCase[] getCircuit()
        {
            return this.circuit;
        }

        public List<Point> getHopitaux()
        {
            return this.hopitaux;
        }

        public List<Point> getPatients()
        {
            return this.patients;
        }


        // CONSTRUCTORS
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


        // METHODS
        private void initialiseCircuit(){
            for (int i=0; i < this.height; i++)
                for (int j=0; j < this.width; j++)
                    circuit[i + j * this.width] = new NXTCase(Case.EMPTY);

        }

        public List<Point> addHopital(Point newHopital){
            if (!hopitaux.Contains(newHopital))
                hopitaux.Add(newHopital);

            return this.hopitaux;
        }

        public List<Point> addPatient(Point newPatient){
            if (!patients.Contains(newPatient))
                patients.Add(newPatient);

            return this.patients;
        }

        public NXTCase[] setCase(NXTCase newCase, Point coordCase){
            this.circuit[coordCase.X + this.width * coordCase.Y] = newCase;

            return this.circuit;
        }

        public NXTCase[] setCase(NXTCase newCase, int x, int y)
        {
            this.circuit[x + this.width * y] = newCase;

            return this.circuit;
        }

        public NXTCase getCase(Point position)
        {
            return this.circuit[position.X + this.width * position.Y];
        }

        public NXTCase getCase(int x, int y)
        {
            return this.circuit[x + this.width * y];
        }
    }
}