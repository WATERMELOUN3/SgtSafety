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

        public IADijkstra(NXTVehicule vehicule)
            : base(vehicule)
        {
            nodes = new List<NXTNode>();
            accessibleCases = new List<Point>();
        }

        private static int GetManhattanHeuristic(Point a, Point b)
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

        private void CreateChilds(Point start, NXTNode current)
        {
            foreach (Point p in circuit.GetNeighbours(current.position))
            {
                if (accessibleCases.Contains(p))
                {
                    NXTNode node = new NXTNode(current, p, (uint)GetManhattanHeuristic(start, p));
                    nodes.Add(node);
                    current.neighbours.Add(node);
                    accessibleCases.Remove(p);
                    CreateChilds(start, node);
                }
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
            if (nb.price > nPrice)
            {
                nb.price = nPrice;
                nb.parent = na;
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

                foreach (NXTNode s2 in s1.neighbours)
                {
                    UpdatePrice(s1, s2);
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
