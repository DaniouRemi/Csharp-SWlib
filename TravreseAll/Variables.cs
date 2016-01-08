using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravreseAll.Propriétés;


namespace TravreseAll
{
    public class SolidWorksVar
    {
        protected internal SldWorks.SldWorks swApp;
        protected internal SldWorks.ModelDoc2 swModel;

        protected SldWorks.Feature SubFeat_Obj;
        protected SldWorks.Feature Feat_Obj;
        protected SldWorks.Feature SubSubFeat_Obj;

        protected int nErrors;
        protected int nWarnnings;

        public List<pComponent> ASM_BOM = new List<pComponent>();
        public pComponent PRT_BOM = new pComponent();
        public List<pReport> Report = new List<pReport>();
        public int ID;
    }

    //public class TraverAllVariables : SolidWorksVar
    //{
        
        
    //}
}
