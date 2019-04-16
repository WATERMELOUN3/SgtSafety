using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTVehicule
{
    public class NXTVehicule
    {
        // FIELDS
        private Point position;
        private int patients;
        private Point direction;

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
        public NXTVehicule(){
            this.position = new Point(0);
            this.patients = 0;        
            this.direction = new Point(0);    
        }

        public NXTVehicule(Point p_position, Point p_direction){
            this.position = p_position;
            this.patients = 0;
            this.direction = p_direction;
        }

        // METHODS
        public int takePatient(){
            return (++this.patients);
        }

        public int dropPatient(){
            return (--this.patients);
        }

        public Point setDirection(Point newDirection){
            this.direction = newDirection;
            return this.direction;
        }

        public Point move(){ // peut être prendre la case qui arrive en argument ?
            // calculer la nouvelle position
            return this.direction;
        }
    }
}