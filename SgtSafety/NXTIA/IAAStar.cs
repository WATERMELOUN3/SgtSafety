using Microsoft.Xna.Framework;
using SgtSafety.NXTEnvironment;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    class IAAStar : IA
    {
        private int nbCasesCircuit;
        private int[] distance;
        private int[] parent;

        public IAAStar()
            : base()
        {
            nbCasesCircuit = circuit.getNbCases();
        }

        public IAAStar(NXTVehicule p_vehicule)
            : base(p_vehicule)
        {
            nbCasesCircuit = circuit.getNbCases();
        }

        private void initialise(int source)
        {
            int i;
            distance = new int[nbCasesCircuit];
            parent = new int[nbCasesCircuit];
            for (i = 0; i < nbCasesCircuit; ++i)
            {
                distance[i] = int.MaxValue;
            }
            distance[source] = 0;
        }

        private int distMin(List<int> allCases)
        {
            int minimum = int.MaxValue;
            int sommet = -1;

            foreach (int c in allCases)
            {
                if (distance[c] < minimum)
                {
                    minimum = distance[c];
                    sommet = c;
                }
            }
            return sommet;
        }

        private int weight(int src, int neighbour)
        {
            return 1;
        }

        private void updateDistance(int s1, int s2)
        {
            if (distance[s2] > distance[s1] + weight(s1, s2))
            {
                distance[s2] = distance[s1] + weight(s1, s2);
                parent[s2] = s1;
            }
        }
        private List<int> getPath(int src, int goal)
        {
            List<int> path = new List<int>();
            int s = goal;
            while (s != src)
            {
                path.Add(s);
                s = parent[s];
            }
            return path;
        }

        public List<Point> intListToPointList(List<int> liste)
        {
            List<Point> newListe = new List<Point>();
            int i;

            for(i=0; i < liste.Count; ++i)
                newListe.Add(circuit.intToPoint(liste.ElementAt(i)));

            return newListe;
        }

        private List<Point> Dijkstra(Point source, Point goal)
        {
            int curr;
            int src = circuit.getIntFromPoint(source),
                gl = circuit.getIntFromPoint(goal);
            initialise(src);
            List<int> allCases = circuit.getAllCases();

            while (allCases.Count > 0)
            {
                curr = distMin(allCases);
                allCases.Remove(curr);
                foreach (Point neighbour in circuit.GetNeighbours(circuit.intToPoint(curr)))
                    updateDistance(curr, circuit.getIntFromPoint(neighbour));
            }

            List<Point> path = intListToPointList(getPath(src, gl));

            return path;
        }

        private List<double> getDistancesPatients(Point start, List<Point> patients)
        {
            List<double> dist = new List<double>();
            double d;
            foreach(Point p in patients)
            {
                d = (Math.Abs(Math.Sqrt(Math.Pow(start.X - p.X, 2) - Math.Pow(start.Y - p.Y, 2))));
                dist.Add(d);
            }

            return dist;
        }

        private List<Point> definePath(Point start)
        {
            List<Point> path = new List<Point>();
            List<Point> patients = circuit.Patients;
            List<Point> hopitaux = circuit.Hopitaux;
            Point hopital = hopitaux.ElementAt(0);
            List<double> distancesPatients = getDistancesPatients(start, patients);
            Point patient1, patient2;

            if (distancesPatients.ElementAt(0) > distancesPatients.ElementAt(1))
            {
                patient1 = patients.ElementAt(1);
                patient2 = patients.ElementAt(0);
            }
            else
            {
                patient1 = patients.ElementAt(0);
                patient2 = patients.ElementAt(1);
            }

            path.AddRange(Dijkstra(start, patient1));
            path.AddRange(Dijkstra(patient1, patient2));
            path.AddRange(Dijkstra(patient2, hopital));


            return path;
        }

        private NXTAction directionToAction(Point pos, Point direction, Point directionPrec)
        {
            NXTCase currentCase = circuit.getCase(pos);
            if (currentCase.goThrough(pStraight, directionPrec).Equals(direction))
                return pStraight;
            else if (currentCase.goThrough(pUturn, directionPrec).Equals(direction))
                return pUturn;
            else if (currentCase.goThrough(pLeft, directionPrec).Equals(direction))
                return pLeft;
            else if (currentCase.goThrough(pRight, directionPrec).Equals(direction))
                return pRight;

            return pStraight;
        }

        private List<NXTAction> pointToAction(List<Point> path, Point start)
        {
            List<NXTAction> liste = new List<NXTAction>();
            Point prec = start, direction, directionPrec;

            foreach (Point p in path)
            {
                direction = new Point(p.X - prec.X, p.Y - prec.Y);
                directionPrec = direction;
                liste.Add(directionToAction(p, direction, directionPrec));
                prec = p;
                directionPrec = direction;
            }

            return liste;
        }

        public void sendPathToVehicule(Point start)
        {
            List<Point> path = new List<Point>();
            path.AddRange(definePath(start));
            List<NXTAction> pathActions = pointToAction(path, start);
            buffer.Clear();

            buffer.AddRange(pathActions);
            sendToVehiculeBuffer();
        }

    }
}
