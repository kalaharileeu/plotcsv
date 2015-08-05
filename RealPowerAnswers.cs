using System;
using System.CodeDom;
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
        private PlotIdcpowermeter plotidcpowermeter;
        //private Wdcconfigured w;

        public RealPowerAnswer(Dictionary<string, Column> dictionary)
        {
            columnobjectlist = new List<Baselist>();
            foreach (var VAR in dictionary)
            {
                columnobjectlist.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
            }
            plotidcpowermeter = new PlotIdcpowermeter(columnobjectlist);
        }

        public PlotIdcpowermeter GetPlotIdcpowermeter
        {
            get { return plotidcpowermeter; }
        }
/*
        public float FindMaxPmcurrent()
        {
            //using var with dictionary more readible
            foreach (var VAR in dictionary)
            {
                columnobjectlist.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
            }

            return plotidcpowermeter.Max;
        }
 */
    }
}
