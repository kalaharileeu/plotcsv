using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class RealPowerAnswers
    {
        public float FindMaxPmcurrent(Dictionary<string, Column> dictionary)
        {
            if (dictionary.ContainsKey("pm_dc_amps_1"))
            {
                return dictionary["pm_dc_amps_1"].GetMaxValue();
            }

            return 0.0f;
        }
    }
}
