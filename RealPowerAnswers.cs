using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlotDVT
{
    class RealPowerAnswer
    {
        private List<Baselist> columnobjectlist;
        //private Wdcconfigured w;

        public RealPowerAnswer()
        {
            columnobjectlist = new List<Baselist>();
        }

        public float FindMaxPmcurrent(Dictionary<string, Column> dictionary)
        {
            //using var with dictionary more readible
            foreach (var VAR in dictionary)
            {
                //w = new Wdcconfigured(VAR.Value.Columnvalues);
                //Fix me:
                //maplistenemies.Add((EnemyGadget)Activator.CreateInstance(typeof(Glider) , mo));
                columnobjectlist.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
            }

            //Type = this.GetType();//Gets the type 
           // XmlPath = "Content/" + Type.ToString().Replace("fourcolors.", "") + ".xml";

    //if (!effectList.ContainsKey((effect.GetType().ToString().Replace("fourcolors.", ""))))
   // effectList.Add(effect.GetType().ToString().Replace("fourcolors.", ""), (effect as ImageEffect));


            foreach (Baselist bl in columnobjectlist)
            {
                //string value = ((V.GetType()).ToString()).Replace("PlotDVT.", "");
                string value = ((bl.GetType()).ToString());
                if (bl is Wdcconfigured)
                {
                    //downcast as returns reference, is return bool
                    float ave = (bl as Wdcconfigured).GetAverage;
                    float max = (bl as Wdcconfigured).GetMax;
                    float min = (bl as Wdcconfigured).GetMin;
                }

                if (bl is Idcpowermeter)
                {
                    try
                    {
                        foreach (Baselist bltwo in columnobjectlist)
                        {
                            if(bltwo is Vdcconfigured)
                                (bl as Idcpowermeter).Populareslices((bltwo as Vdcconfigured).Listslices);
                        }
                    }
                    catch (InvalidCastException)
                    {
                        
                    }


                }
                //value(Object).GetAverage;
                //V as typeof(value);
                //var casted = V as typeof(V);
            }

            if (dictionary.ContainsKey("Idcpowermeter"))
            {
                return dictionary["Idcpowermeter"].GetMaxValue();
            }


            return 0.0f;
        }
    }
}
