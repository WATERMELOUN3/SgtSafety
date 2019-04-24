using Microsoft.Xna.Framework;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    class IAAStar : IA
    {
        public IAAStar()
            : base()
        {
            
        }

        public IAAStar(NXTVehicule p_vehicule)
            : base(p_vehicule)
        {

        }

        public void computeTrajectory(Point objectif)
        {
            Point startingPoint = vehicule.Position;
            Point current;
            List<Point> exploredCases = new List<Point>();
            List<Point> notEvaluated = new List<Point>();
            List<Point> successors = new List<Point>();

            notEvaluated.Add(startingPoint);

            while (notEvaluated.Count > 0)
            {
                current = lowestFScorePoint();

                notEvaluated.Remove(current);

                exploredCases.Add(current);
            }
        }
    }
}
