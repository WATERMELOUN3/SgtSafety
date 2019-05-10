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

        protected List<NXTNode> Nodes
        {
            get { return nodes; }
        }

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

        protected void Initialize(Point start)
        {
            nodes.Clear();
            sptSet.Clear();

            accessibleCases = circuit.getAllCases();
            NXTNode sNode = new NXTNode(start, 0);
            nodes.Add(sNode);
            accessibleCases.Remove(start);

            CreateChilds(start, sNode);

            foreach (NXTNode n in nodes)
            {
                foreach (Point p in circuit.GetNeighbours(n.position))
                {
                    NXTNode neighbour = FindNodeAt(p);
                    n.neighbours.Add(neighbour);
                }
            }
        }

        protected NXTNode FindNodeAt(Point p)
        {
            foreach (NXTNode n in nodes)
            {
                if (n.position.Equals(p))
                    return n;
            }

            Console.WriteLine("null @ " + p); // Si le noeud n'existe pas, arrive quand des voisins sont manquants
            return null;
        }

        private void CreateChilds(Point start, NXTNode current)
        {
            foreach (Point p in circuit.GetNeighbours(current.position))
            {
                if (accessibleCases.Contains(p))
                {
                    NXTNode node = new NXTNode(current, p, uint.MaxValue);
                    nodes.Add(node);
                    accessibleCases.Remove(p);
                    sptSet.Add(node.position, false);
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
            if (nb != null)
            {
                uint nPrice = na.price + (uint)GetManhattanHeuristic(na.position, nb.position);
                if (!sptSet[nb.position] && na.price != uint.MaxValue && nPrice < nb.price)
                {
                    nb.price = nPrice;
                    nb.parent = na;
                    //this.circuit.getCase(nb.position).CaseColor = new Color(nb.price / 10f, (10 - nb.price) / 10f, 0f); // Donne une couleur aux cases selon l'heuristique
                    //Console.WriteLine("Parent updated " + na.position + " -> " + nb.position);
                }
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

        //Retourne le patient le plus proche du robot, le seconde argument n'est à passer que dans le cas de l'IA, sinon lui passer NXTVehicule.ERROR
        public Point FindClosestPatient(Point targetTelec)
        {
            int distanceMin = int.MaxValue,
                distance;
            Point closestPatient = NXTVehicule.ERROR;
            foreach (Point p in this.vehicule.Circuit.Patients)
            {
                distance = GetManhattanHeuristic(p, vehicule.Position);
                if (distance < distanceMin && !p.Equals(targetTelec))
                {
                    distanceMin = distance;
                    closestPatient = p;
                }
            }

            return closestPatient;
        }

        //Retourne l'hopital le plus proche du robot
        public Point FindClosestHopital()
        {
            List<Point> hopitaux = vehicule.Circuit.Patients;
            int distanceMin = int.MaxValue,
                distance;
            Point closestHopital = NXTVehicule.ERROR;
            foreach (Point h in hopitaux)
            {
                distance = GetManhattanHeuristic(h, vehicule.Position);
                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    closestHopital = h;
                }
            }

            return closestHopital;
        }

    }
}
