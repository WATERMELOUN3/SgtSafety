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
            return 0;
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

        private void Dijkstra(Point source, Point goal)
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

            List<int> path = getPath(src, gl);
        }


    }
}
