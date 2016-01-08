using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SldWorks;

namespace TravreseAll.Propriétés
{
    public class pComponent
    {
        public SldWorks.ModelDoc2 swModel;
        //public SldWorks.ModelDoc2 swModel { get; set; }

        public SldWorks.ConfigurationManager swConfMgr;
        //public SldWorks.ConfigurationManager swConfMgr { get; set; }

        public SldWorks.Configuration swConf;
        //public SldWorks.Configuration swConf { get; set; }

        public SldWorks.Component2 swRootComp;
        //public SldWorks.Component2 swRootComp { get; set; }

        public object[] vComponent;
        //public object vComponent { get; set; }

        public SldWorks.ModelDocExtension swModelDocExt;
        //public SldWorks.ModelDocExtension swModelDocExt { get; set; }

        public SldWorks.CustomPropertyManager swCustProp;
        //public SldWorks.CustomPropertyManager swCustProp { get; set; }

        public SldWorks.SelectionMgr swselmgr;
        //public SldWorks.SelectionMgr swselmgr { get; set; }

        public SldWorks.BodyFolder swBodyFolder;
        //public SldWorks.BodyFolder swBodyFolder { get; set; }

        public SldWorks.CustomPropertyManager swCustPropMgr;
        //public SldWorks.CustomPropertyManager swCustPropMgr { get; set; }

        public SldWorks.Component2 swComponent;
        //public SldWorks.Component2 swComponent { get; set; }

        public int I;
        //public int I { get; set; }

        public string LastConfiguration;
        //public string LastConfiguration { get; set; }

        public string Patch;
        //public string Patch { get; set; }

        public int Type;
        //public int Type { get; set; }

        public int Quantity;
        //public int Quantity { get; set; }

        public List<pProperty> Properties = new List<pProperty>();
        //public List<pProperties> Properties { get; set; }

        public List<pChildren> Children = new List<pChildren>();
        //public List<pChildren> Children { get; set; }

        public List<pConfigurations> Configuration = new List<pConfigurations>();

    }
}
