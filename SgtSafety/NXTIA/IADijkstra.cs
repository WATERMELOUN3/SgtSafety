using Microsoft.Xna.Framework;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    public class IADijkstra : IA
    {
        private List<Point> accessibleCases;
        private List<NXTNode> nodes;
        private Dictionary<Point, bool> sptSet;

        public IADijkstra(NXTVehicule vehicule, bool p_simulation = false)
            : base(vehicule, p_simulation)
        {
            nodes = new List<NXTNode>();
            accessibleCases = new List<Point>();
            sptSet = new Dictionary<Point, bool>();
        }

        protected static int GetManhattanHeuristic(Point a, Point b)
        {
            return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
        }

        private void Initialize(Point start)
        {
            accessibleCases = circuit.getAllCases();
            NXTNode sNode = new NXTNode(start, 0);
            nodes.Add(sNode);
            accessibleCases.Remove(start);

            CreateChilds(start, sNode);
        }


        /*
         * Petit soucis de parent/voisins, sinon ça marche :)
         */
        private void CreateChilds(Point start, NXTNode current)
        {
            foreach (Point p in circuit.GetNeighbours(current.position))
            {
                if (accessibleCases.Contains(p))
                {
                    NXTNode node = new NXTNode(current, p, uint.MaxValue);
                    nodes.Add(node);
                    current.neighbours.Add(node);
                    accessibleCases.Remove(p);
                    sptSet.Add(node.position, false);
                }
            }

            foreach (NXTNode n in current.neighbours)
            {
                CreateChilds(start, n);
            }
        }

        private NXTNode GetMin(List<NXTNode> nodes)
        {
            uint mini = uint.MaxValue;
            NXTNode node = null;

            foreach (NXTNode n in nodes)
            {
                if (n.price < mini)
                {
                    mini = n.price;
                    node = n;
                }
            }

            return node;
        }

        private void UpdatePrice(NXTNode na, NXTNode nb)
        {
            uint nPrice = na.price + (uint)GetManhattanHeuristic(na.position, nb.position);
            if (!sptSet[nb.position] && na.price != uint.MaxValue && nPrice < nb.price)
            {
                nb.price = nPrice;
                nb.parent = na;
                this.circuit.getCase(nb.position).CaseColor = new Color(nb.price / 10f, (10 - nb.price) / 10f, 0f);
                Console.WriteLine("Parent updated " + na.position + " -> " + nb.position);
            }
        }

        public List<Point> ComputeDijkstra(Point start, Point goal)
        {
            Initialize(start);
            List<NXTNode> q = new List<NXTNode>(nodes);
            NXTNode s1 = null;

            while (q.Count > 0)
            {
                s1 = GetMin(q);
                q.Remove(s1);

                if (s1.position.Equals(goal))
                    break;

                sptSet[s1.position] = true;
                Console.WriteLine("s1 -> " + s1.position);
                foreach (NXTNode s2 in s1.neighbours)
                {
                    UpdatePrice(s1, s2);
                    Console.WriteLine("s2 -> " + s2.position);
                }
            }

            return ComputePath(s1);
        }

        private List<Point> ComputePath(NXTNode finalNode)
        {
            List<Point> path = new List<Point>();
            NXTNode cNode = finalNode;

            while (cNode.price != 0)
            {
                path.Add(cNode.position);
                cNode = cNode.parent;
            }

            path.Reverse();
            return path;
        }
    }
}
