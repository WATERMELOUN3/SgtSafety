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
            List<Point> allCases = circuit.getAllCases();

            while (allCases.Count > 0)
            {
                //curr = distMin(allCases);
                //allCases.Remove(curr);
                //foreach (Point neighbour in circuit.GetNeighbours(circuit.intToPoint(curr)))
                //    updateDistance(curr, circuit.getIntFromPoint(neighbour));
            }

            List<Point> path = intListToPointList(getPath(src, gl));

            Console.WriteLine(path);

            return path;
        }

        private List<Point> definePath(Point start)
        {
            List<Point> path1 = new List<Point>(),
                        path2 = new List<Point>();
            List<Point> patients = circuit.Patients;
            List<Point> hopitaux = circuit.Hopitaux;
            Point hopital = hopitaux.ElementAt(0);
            Point patient1 = patients.ElementAt(1),
                  patient2 = patients.ElementAt(0);

            path1.AddRange(Dijkstra(start, patient1));
            path1.AddRange(Dijkstra(patient1, hopital));
            path1.AddRange(Dijkstra(hopital, patient2));
            path1.AddRange(Dijkstra(patient2, hopital));

            path2.AddRange(Dijkstra(start, patient2));
            path2.AddRange(Dijkstra(patient2, hopital));
            path2.AddRange(Dijkstra(hopital, patient1));
            path2.AddRange(Dijkstra(patient1, hopital));

            if (path1.Count > path2.Count)
            {
                Console.WriteLine(path2.Count + " déplacements !");
                return path2;
            }
            Console.WriteLine(path1.Count + " déplacements !");
            return path1;
        }

        private NXTAction directionToAction(Point pos, Point direction, Point directionPrec)
        {
            NXTCase currentCase = circuit.getCase(pos);
            NXTAction action = new NXTAction(NXTMovement.STRAIGHT);
            if (currentCase.goThrough(pStraight, directionPrec).Equals(direction))
                action.Movement = NXTMovement.STRAIGHT;
            else if (currentCase.goThrough(pUturn, directionPrec).Equals(direction))
                action.Movement = NXTMovement.UTURN;
            else if (currentCase.goThrough(pLeft, directionPrec).Equals(direction))
                action.Movement = NXTMovement.INTER_LEFT;
            else if (currentCase.goThrough(pRight, directionPrec).Equals(direction))
                action.Movement = NXTMovement.INTER_RIGHT;

            if (circuit.hasPatient(pos + direction))
                action.Action = NXTAction.TAKE;
            else if (circuit.hasHopital(pos + direction))
                action.Action = NXTAction.DROP;

            return action;
        }

        private List<NXTAction> pointToAction(List<Point> path, Point start, Point directionStart)
        {
            List<NXTAction> liste = new List<NXTAction>();
            Point prec = start, direction, directionPrec = directionStart;

            foreach (Point p in path)
            {
                direction = new Point(p.X - prec.X, p.Y - prec.Y);
                liste.Add(directionToAction(p, direction, directionPrec));

                prec = p;
                directionPrec = direction;
            }

            return liste;
        }

        public List<Point> sendPathToVehicule(Point start)
        {
            List<Point> path = definePath(start);
            List<NXTAction> pathActions = pointToAction(path, start, vehicule.Direction);
            buffer.Clear();

            buffer.AddRange(pathActions);
            sendToVehiculeBuffer();

            return path;
        }

    }
}
