using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTEnvironment
{
    public enum Case {STRAIGHT, TURNLEFT, TURNRIGHT, INTERSECTION, EMPTY};

    public class NXTCircuit
    {
        private int height;
        private int width;
        private Case[] circuit; //i = x + width * y

        private List<Point> hopitaux;

        private List<Point> patients;

        public NXTCircuit(){
            this.height = 10;
            this.width = 10;
            this.circuit = new Case[this.height * this.width];
            this.hopitaux = new List<Point>();
            this.patients = new List<Point>();

            initialiseCircuit();
        }

        public NXTCircuit(int p_height, int p_width){
            this.height = p_height;
            this.width = p_width;
            this.circuit = new Case[this.height * this.width];
            this.hopitaux = new List<Point>();
            this.patients = new List<Point>();

            initialiseCircuit();
        }

        public NXTCircuit(int p_height, int p_width, Case[] p_circuit){
            this.height = p_height;
            this.width = p_width;
            this.circuit = p_circuit;
            this.hopitaux = new List<Point>();
            this.patients = new List<Point>();
        } 

        public NXTCircuit(int p_height, int p_width, Case[] p_circuit, List<Point> p_hopitaux, List<Point> p_patients){
            this.height = p_height;
            this.width = p_width;
            this.circuit = p_circuit;
            this.hopitaux = p_hopitaux;
            this.patients = p_patients;
        }

        private void initialiseCircuit(){
            for (int i=0; i < this.height; i++)
                for (int j=0; j < this.width; j++)
                    circuit[i + j * this.width] = Case.EMPTY;

        }
        public int getWidth(){
            return this.width;
        }

        public int getHeight(){
            return this.height;
        }    

        public Case[] getCircuit(){
            return this.circuit;
        }

        public List<Point> getHopitaux(){
            return this.hopitaux;
        } 

        public List<Point> getPatients(){
            return this.patients;
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

        public Case[] setCase(Case typeCase, Point coordCase){
            this.circuit[coordCase.X + this.width * coordCase.Y] = typeCase;

            return this.circuit;
        }
    }
}