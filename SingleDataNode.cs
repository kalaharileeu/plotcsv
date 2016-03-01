using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    public class CSVrow
    {
        public CSVrow()
        {
            rowkeyvalue = new Dictionary<string, string>();
        }

        public void Addvlaue(string key, string value)
        {
            rowkeyvalue.Add(key, value);
        }

        Dictionary<string, string> rowkeyvalue;

        //float Wacvarconfigured { get; set; }
        //float Wacconfigured { get; set; }
        //float Wdcconfigured { get; set; }
        //float Vdcconfigured { get; set; }
        //float Phaseconfigured { get; set; }
        //float Idcconfigured { get; set; }

        //float Wdcpcu { get; set; }
        //float Wacpcu { get; set; }
        //float Wacimagpcu { get; set; }
        //float Vacpcu { get; set; }
        //float Idcpcu { get; set; }
        //float Vdcpcu { get; set; }

        //float Idcpowermeter { get; set; }
        //float Vdcpowermeter { get; set; }
        //float Wdcpowermeter { get; set; }
        //float Wacpowermeter { get; set; }
        //float Efficiency { get; set; }
        //float ACvapowermeter { get; set; }
        //float ACvarpowermeter { get; set; }
        //float Vacpowermeter { get; set; }
    }
}
