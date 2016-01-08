using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravreseAll.Propriétés

namespace TravreseAll
{
    public class Check : SolidWorksVar
    {
        internal List<pComponent> BOM_Checked;
        internal bool NewConfiguration;
        internal bool NewComponent;
        internal int ComponentID;

        internal void DoubloonCheck(List<pComponent> BOM, pTempParent TempParent, int ID)
        {

        }
    }
}
