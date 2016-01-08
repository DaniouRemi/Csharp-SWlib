using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SldWorks;

namespace TravreseAll.Propriétés
{
    public class pFunctions
    {
        public string Name;
        //public string Name { get; set; }

        public SldWorks.Feature Feature;
        //public SldWorks.Feature Feature { get; set; }

        public List<pFunctions> SubFunction = new List<pFunctions>();
        //public List<pFunctions> SubFunction { get; set; }
    }
}
