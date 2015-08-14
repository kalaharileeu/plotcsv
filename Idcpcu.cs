using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Idcpcu : Valuelist
    {

        public Idcpcu(List<string> stringvaluelist) : base(stringvaluelist)
        {
        }

        public new float GetAverage
        {
            get { return average; }
        }

        public new float GetMax
        {
            get { return maxvalue; }
        }

        public new float GetMin
        {
            get { return minvalue; }
        }

        public new List<float> GetFloats
        {
            get { return valuesfloat; }
        }

        public Dictionary<float, List<float>> Slices
        {
            get { return slices; }
        }
    }
}
