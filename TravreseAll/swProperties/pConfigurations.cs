using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravreseAll.Propriétés
{
    public class pConfigurations
    {
        public string Name;
        //public string Name { get; set; }

        public List<pParent> Parent = new List<pParent>();
        //public List<pParent> Parent { get; set; }

        public List<pChildren> Children = new List<pChildren>();
        //public List<pChildren> Children { get; set; }

        public pBodies Bodies;
        //public pBodies Bodies { get; set; }

        public List<pProperty> Properties = new List<pProperty>();
        //public List<pProperties> Properties { get; set; }

        public List<pFunctions> Functions = new List<pFunctions>();
        //public List<pFunctions> Functions { get; set; }

        public int Quantity;
        //public int Quantity { get; set; }
    }
}
