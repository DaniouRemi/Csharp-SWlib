using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SldWorks;
using SwConst;

namespace TravreseAll
{
    public class SolidWorksComponent : SolidWorksVar
    {
        private Check Check = new Check();
        

        public SolidWorksComponent()
        {
            Debug.Print("{0} - *********************************************************************************************", DateTime.Now.ToLongTimeString());
            Debug.Print("{0} - new()", DateTime.Now.ToLongTimeString());

            //Checks.SW_Open()

            try
            {
                swApp=new SldWorks.SldWorks();
                swModel=swApp.ActiveDoc;
                              
                if (swModel == null)
                {
                    Debug.Print("{0} - No file openned", DateTime.Now.ToLongTimeString());

                }
                else if (swModel.GetType() == swDocumentTypes_e.swDocASSEMBLY.GetHashCode() | swModel.GetType() == swDocumentTypes_e.swDocPART.GetHashCode())
                { }
                else
                {
                    Debug.Print("{0} - no file compatible", DateTime.Now.ToLongTimeString());
                }
            }
            catch (System.Exception ex)
                {
                    Debug.Print("{0} - ERROR : {1}", DateTime.Now.ToLongTimeString(),ex.ToString());
                }

        }

        void Traverse(string Options = null, bool MultiThread = false)
        {

        }
    }
}
