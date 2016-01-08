using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravreseAll.Propriétés;
using System.Diagnostics;
using SwConst;
using SldWorks;
using System.IO;

namespace TravreseAll
{
    public class TraverseAll : SolidWorksVar
    {
        private Check Check = new Check();
        //private PropertiesRecovery PropertiesRecovery = new PropertiesRecovery;

        protected SldWorks.Component2 swComponent;
        protected string ChemComp;
        protected string NomConf;
        protected string NomComp;
        protected long TypeFile;

        protected List<pTempParent> TempParent = new List<pTempParent>();
        protected int ChildID;
        protected int TreatmentID;

        internal void Traverse(string Options = null)
        {
            Debug.Print("{0} - DEBUT définition du type de composant d'entrée", DateTime.Now.ToLongTimeString());

            switch (swModel.GetPathName())
            {
                case ".sldasm":
                    Debug.Print("{0} -  Composant type 'assemblage'", DateTime.Now.ToLongTimeString());
                    TraverseASM(Options);
                    break;

                case ".sldprt" :
                    Debug.Print("{0} -  Composant type 'pièce'", DateTime.Now.ToLongTimeString());

                    PRT_BOM.Patch = swModel.GetPathName();
                    PRT_BOM.Type = swDocumentTypes_e.swDocPART.GetHashCode();
                    PRT_BOM.swConfMgr = swModel.ConfigurationManager;
                    PRT_BOM.swConf = PRT_BOM.swConfMgr.ActiveConfiguration;
                    PRT_BOM.swRootComp = PRT_BOM.swConf.GetRootComponent3(true);
                    PRT_BOM.vComponent = PRT_BOM.swRootComp.GetChildren();
                    PRT_BOM.LastConfiguration = PRT_BOM.swConf.Name;

                    TraverseFunctions();
                    TraverseBodies();

                    break;
                case ".slddrw" :
                    Debug.Print("{0} -  Composant type 'mise en plan'", DateTime.Now.ToLongTimeString());
                    break;

                default:
                    Debug.Print("{0} -  Composant type inconnu", DateTime.Now.ToLongTimeString());
                    break;
            }
        }

        private void TraverseASM(string Options = null)
        {
            Debug.Print("{0} - Début du TraverseAll de l'assemblage", DateTime.Now.ToLongTimeString());

            //pParent vParent;
            //int vID = 0;
            SldWorks.ConfigurationManager swConfMgr;
            SldWorks.Configuration swConf;
            SldWorks.Component2 swRootComp;
            object[] vComponent;

            ASM_BOM.Add(new pComponent());

            ID ++;
            swConfMgr = swModel.ConfigurationManager;
            swConf = swConfMgr.ActiveConfiguration;
            swRootComp = swConf.GetRootComponent3(true);
            vComponent = swRootComp.GetChildren();

            ASM_BOM[ID].swConfMgr = swConfMgr;
            ASM_BOM[ID].swConf = swConf;
            ASM_BOM[ID].swRootComp = swRootComp;
            ASM_BOM[ID].vComponent = vComponent;

            if (ASM_BOM.Count <= 1)
            {
                Debug.Print("{0} - Définition du composant initial (type ASM)", DateTime.Now.ToLongTimeString());
                ASM_BOM[ID].Patch = swModel.GetPathName();
                ASM_BOM[ID].LastConfiguration = swConf.Name;
                ASM_BOM.Add(new pComponent());
                ID ++;
            }

            TempParent.Add(new pTempParent());
            TempParent[TempParent.Count - 1].Adress = swModel.GetPathName();
            TempParent[TempParent.Count - 1].ConfigurationName = swConf.Name;
            TempParent[TempParent.Count - 1].ID = ID --;

            Debug.Print("{0} - Initialisation du parent temporaire :", DateTime.Now.ToLongTimeString());
            Debug.Print("{0} -     - Chemin : '{1}'", DateTime.Now.ToLongTimeString(), swModel.GetPathName());
            Debug.Print("{0} -     - Configuration : '{1}'", DateTime.Now.ToLongTimeString(), swConf.Name);
            Debug.Print("{0} -     - Nombre de composants : '{1}'", DateTime.Now.ToLongTimeString(), vComponent);

            //for (int i = 0; i < 10; i++)
            for (int i = 0; i < vComponent.Length-1; i++)
            {
                //string ShortName = swModel.GetPathName().Split(new string[]{ "\" })[swModel.GetPathName().Split(new string[]{"\"}).Count()]);
                //Debug.Print("{0} - composant ID {1}/{2} (dans '{3}' :", DateTime.Now.ToLongTimeString(),i,10,ShortName);

                //ASM_BOM[ID].swComponent = vComponent[i];
                ASM_BOM[ID].Patch=ASM_BOM[ID].swComponent.ReferencedConfiguration;
                ASM_BOM[ID].LastConfiguration=ASM_BOM[ID].swComponent.ReferencedConfiguration;

                Debug.Print("{0} -     - Adresse : '{1}'", DateTime.Now.ToLongTimeString(),  ASM_BOM[ID].Patch);
                Debug.Print("{0} -     - Configuration : '{1}'", DateTime.Now.ToLongTimeString(), ASM_BOM[ID].LastConfiguration);

                ChildID=ID;

                //================== VERIFICATION DOUBLON ==================
                Check.DoubloonCheck(ASM_BOM, TempParent[TempParent.Count - 1], ID);
                ASM_BOM = Check.BOM_Checked;
                bool NewConfiguration = Check.NewConfiguration;
                bool NewComponent = Check.NewComponent;

                if (!NewConfiguration)
                {
                    ASM_BOM = Check.BOM_Checked; //Utile (Voir 5 lignes plus haut)?
                }

                //================== Création composant ==================
                if (NewConfiguration)
                {
                    Debug.Print("{0} -     DEBUT de l'insertion de la configuration dans la liste", DateTime.Now.ToLongTimeString());

                    if (!NewComponent)
                    {
                        Debug.Print("{0} -         - Composant déjà existant (Index = {1})", DateTime.Now.ToLongTimeString(),Check.ComponentID);
                        ASM_BOM.RemoveAt(ASM_BOM.Count-1);
                        ID--;
                        TreatmentID=Check.ComponentID;
                    }
                    else
                    {
                        Debug.Print("{0} -         - Nouveau composant (Index = {1})", DateTime.Now.ToLongTimeString(),ID);
                        TreatmentID=ID;
                    }

                    //ASM_BOM[TreatmentID].swComponent=vComponent[i];
                    ASM_BOM[TreatmentID].LastConfiguration=ASM_BOM[TreatmentID].swComponent.ReferencedConfiguration;
                    ASM_BOM[TreatmentID].Configuration.Add(new pConfigurations());

                    ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count-1].Name=ASM_BOM[TreatmentID].LastConfiguration;
                    ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count-1].Quantity=1;

                    Debug.Print("{0} -         - Nom de la configuration : '{1}' (Index = {2})", DateTime.Now.ToLongTimeString(), ASM_BOM[TreatmentID].LastConfiguration, ASM_BOM[TreatmentID].Configuration.Count - 1);
                    Debug.Print("{0} -     FIN de l'insertion de la configuration dans la liste", DateTime.Now.ToLongTimeString());

                    //Insertion du parent dans le composant
                    Debug.Print("{0} -     DEBUT de l'insertion du parent dans la configuration", DateTime.Now.ToLongTimeString());
                    ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent.Add(new pParent());
                    ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent[ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent.Count - 1].Adress = TempParent[TempParent.Count - 1].Adress;
                    ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent[ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent.Count - 1].ConfigurationName = TempParent[TempParent.Count - 1].ConfigurationName;
                    ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent[ASM_BOM[TreatmentID].Configuration[ASM_BOM[TreatmentID].Configuration.Count - 1].Parent.Count - 1].Quantity=1;

                    Debug.Print("{0} -         - Adresse : '{1}'", DateTime.Now.ToLongTimeString(), TempParent[TempParent.Count - 1].Adress);
                    Debug.Print("{0} -         - Configuration : '{1}'", DateTime.Now.ToLongTimeString(), TempParent[TempParent.Count - 1].ConfigurationName);
                    Debug.Print("{0} -     FIN de l'insertion du parent dans la configuration", DateTime.Now.ToLongTimeString());

                    //================== Définition du type de composant ==================
                    Debug.Print("{0} -     DEBUT de la définition du type du composant", DateTime.Now.ToLongTimeString());
                    string ExtentionFile;
                    //ExtentionFile = LCase(Right(ASM_BOM.Item(TreatmentID).Patch, 7))
                    ExtentionFile = ASM_BOM[TreatmentID].Patch.Substring(ASM_BOM[TreatmentID].Patch.Length - 7).ToLower();
                    
                    switch (ExtentionFile)
                    {
                        case ".sldasm" :
                            Debug.Print("{0} -         - Type du composant : Assemblage (*.SLDASM)", DateTime.Now.ToLongTimeString());
                            ASM_BOM[TreatmentID].Type = swDocumentTypes_e.swDocASSEMBLY.GetHashCode();
                            break;
                        case ".sldprt" :
                            Debug.Print("{0} -         - Type du composant : Pièce (*.SLDPRT)", DateTime.Now.ToLongTimeString());
                            ASM_BOM[TreatmentID].Type = swDocumentTypes_e.swDocPART.GetHashCode();
                            break;
                        default:
                            break;
                    }
                    Debug.Print("{0} -     FIN de la définition du type du composant", DateTime.Now.ToLongTimeString());

                    //================== Traitement du composant ==================
                    Debug.Print("{0} -     DEBUT du traitemnt du composant", DateTime.Now.ToLongTimeString());
                    Debug.Print("{0} -          DEBUT ouverture du fichier", DateTime.Now.ToLongTimeString());

                    if (File.Exists(ASM_BOM[TreatmentID].Patch))
                    {
                        swModel = swApp.OpenDoc6(ASM_BOM[TreatmentID].Patch, ASM_BOM[TreatmentID].Type, 0, ASM_BOM[TreatmentID].LastConfiguration, nErrors, nWarnnings);
                        ASM_BOM[TreatmentID].swModel = swModel;
                        Debug.Print("{0} -               - Fichier ouvert", DateTime.Now.ToLongTimeString());
                        Debug.Print("{0} -          FIN ouverture du fichier", DateTime.Now.ToLongTimeString());
                    }
                    else
                    {
                        PRT_BOM = ASM_BOM[TreatmentID];
                        TraverseFunctions();
                        TraverseBodies();
                        ASM_BOM[TreatmentID] = PRT_BOM;
                        ASM_BOM.Add(new pComponent());

                        ID ++;
                        swApp.QuitDoc(ASM_BOM[TreatmentID].Patch);
                    }
                }
            }
        }

        public SldWorks.Feature BobiesFeature;
        private void TraverseFunctions()
        {
            Debug.Print("{0} -          DEBUT Traversée des fonctions", DateTime.Now.ToLongTimeString());

            Feat_Obj = PRT_BOM.swModel.FirstFeature();

            while (Feat_Obj==null)
            {
                PRT_BOM.Configuration[PRT_BOM.Configuration.Count-1].Functions.Add(new pFunctions());
                PRT_BOM.Configuration[PRT_BOM.Configuration.Count - 1].Functions[PRT_BOM.Configuration[PRT_BOM.Configuration.Count - 1].Functions.Count - 1].Name = Feat_Obj.GetTypeName2();
                PRT_BOM.Configuration[PRT_BOM.Configuration.Count - 1].Functions[PRT_BOM.Configuration[PRT_BOM.Configuration.Count - 1].Functions.Count - 1].Feature = Feat_Obj;

                Debug.Print("{0} -              - {1} ('{2}')", DateTime.Now.ToLongTimeString(),Feat_Obj.Name,Feat_Obj.GetTypeName2());

                if (Feat_Obj.GetTypeName2() == "SolidBodyFolder")
                {
                    Debug.Print("{0} -                  - Mise à jour de {1})", DateTime.Now.ToLongTimeString(), Feat_Obj.Name);
                    UpdateCutList(Feat_Obj);
                    BobiesFeature = Feat_Obj;
                }

                TraverseSubFunctions(Feat_Obj, PRT_BOM.Configuration[PRT_BOM.Configuration.Count - 1].Functions);
                Feat_Obj = Feat_Obj.GetNextFeature();
            }

            Debug.Print("{0} -          FIN Traversée des fonctions", DateTime.Now.ToLongTimeString());

        }

        private void TraverseSubFunctions(SldWorks.Feature Feat_Obj, List<pFunctions> Features, string SpaceForDebug = "   ")
        {
            SldWorks.Feature SubFeat;
            SubFeat = Feat_Obj.GetFirstSubFeature();

            while (SubFeat == null)
            {
                Features[Features.Count - 1].SubFunction.Add(new pFunctions());
                Features[Features.Count - 1].SubFunction[Features[Features.Count - 1].SubFunction.Count - 1].Name = SubFeat.GetTypeName2();
                Features[Features.Count - 1].SubFunction[Features[Features.Count - 1].SubFunction.Count - 1].Feature = SubFeat;

                Debug.Print("{0} -             {1}{2} ('{3}')", DateTime.Now.ToLongTimeString(), SpaceForDebug, SubFeat.Name, SubFeat.GetTypeName2());

                SpaceForDebug = SpaceForDebug + "   ";
                TraverseSubFunctions(SubFeat, Features[Features.Count - 1].SubFunction, SpaceForDebug);


                SubFeat = SubFeat.GetNextSubFeature();
                //SpaceForDebug = Left(SpaceForDebug, SpaceForDebug.Length - 3);
            }
        }



        private void TraverseBodies()
        {

        }

        private void UpdateCutList(SldWorks.Feature _swFeature)
        {

        }
    }
}
