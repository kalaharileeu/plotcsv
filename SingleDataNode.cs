using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    public class SingleDataNode
    {
        public float Wacvarconfigured { get; set; }
        public float Wacconfigured { get; set; }
        public float Wdcconfigured { get; set; }
        public float Vdcconfigured { get; set; }
        public float Phaseconfigured { get; set; }
        public float Idcconfigured { get; set; }

        public float Wdcpcu { get; set; }
        public float Wacpcu { get; set; }
        public float Wacimagpcu { get; set; }
        public float Vacpcu { get; set; }
        public float Idcpcu { get; set; }
        public float Vdcpcu { get; set; }

        public float Idcpowermeter { get; set; }
        public float Vdcpowermeter { get; set; }
        public float Wdcpowermeter { get; set; }
        public float Wacpowermeter { get; set; }
        public float Efficiency { get; set; }
        public float ACvapowermeter { get; set; }
        public float ACvarpowermeter { get; set; }
        public float Vacpowermeter { get; set; }

        public SingleDataNode()
        {

        }
    }
}
