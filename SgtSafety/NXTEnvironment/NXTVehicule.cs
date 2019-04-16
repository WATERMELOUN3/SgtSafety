using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTVehicule
{
    public class NXTVehicule
    {
        private Point position;
        private int patients;
        private Point direction;

        public NXTVehicule(){
            this.position = Point(0);
            this.patients = 0;        
            this.direction = Point(0);    
        }

        public NXTVehicule(Point p_position, Point p_direction){
            this.position = p_position;
            this.patients = 0;
            this.direction = p_direction;
        }

        public Point getPosition(){
            return this.position;
        }
        
        public int getPatients(){
            return this.patients;
        }
    
        public Point getDirection(){
            return this.direction;
        }

        public int takePatient(){
            return (++this.patients);
        }

        public void dropPatient(){
            return (--this.patients);
        }

        public Point setDirection(Point newDirection){
            this.direction = newDirection;
            return this.direction;
        }

        public Point move(){

        }
    }
}