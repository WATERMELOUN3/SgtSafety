using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTIA
{
    public class NXTNode
    {
        public NXTNode parent;
        public List<NXTNode> neighbours;
        public Point position;
        public uint price;

        public NXTNode(Point position, uint price)
        {
            this.parent = null;
            this.position = position;
            this.price = price;
            this.neighbours = new List<NXTNode>();
        }

        public NXTNode(NXTNode parent, Point position, uint price)
        {
            this.parent = parent;
            this.position = position;
            this.price = price;
            this.neighbours = new List<NXTNode>();
        }

        public bool Equals(NXTNode obj)
        {
            if (obj.position.Equals(this.position))
                return true;
            else
                return false;
        }
    }
}
