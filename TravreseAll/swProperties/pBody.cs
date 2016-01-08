using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravreseAll.Propriétés
{
    public class pBody
    {
        public string Name;
        //public string Name { get; set; }

        public int Count;
        //public int Count { get; set; }

        public List<pProperty> Properties = new List<pProperty>();
        //public List<pProperties> Properties { get; set; }
    }
}
