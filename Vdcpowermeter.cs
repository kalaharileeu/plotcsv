using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Vdcpowermeter: Valuelist
    {
        public Vdcpowermeter(List<string> stringvaluelist) : base(stringvaluelist)
        {
            valuesstring = stringvaluelist;
        }
    }
}
