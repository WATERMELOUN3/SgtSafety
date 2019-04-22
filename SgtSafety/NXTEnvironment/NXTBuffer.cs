using SgtSafety.NXTBluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgtSafety.Types;

namespace SgtSafety.NXTEnvironment
{
    public class NXTBuffer : List<NXTAction>
    {
        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public NXTBuffer()
            : base()
        { }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Ajoute un élément au buffer, contrairement à List, on choisit si on l'ajoute au début ou à la fin)
        public void Add(NXTAction p, bool debut = true)
        {
            if (debut)
                base.Insert(0, p);
            else
                base.Add(p);
        }

        // Fait sauter le premier index de la liste
        public NXTAction Pop()
        {
            NXTAction p = base[0];
            base.RemoveAt(0);

            return p;
        }
        
        // Retourne true si le buffer est vide, sinon false
        public bool isEmpty()
        {
            return (this.Count() == 0);
        }
    }
}
