using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Idcconfigured : Valuelist
    {
        private Dictionary<float, List<float>> slices;

        public Idcconfigured(List<string> stringvaluelist) : base(stringvaluelist)
        {
            valuesstring = stringvaluelist;
            slices = new Dictionary<float, List<float>>();
        }

        public Dictionary<float, List<float>> Slices 
        {
            get{ return slices; }
        }

        public void Populareslices(Dictionary<float, List<int>> slicedvalues)
        {
            foreach (KeyValuePair<float, List<int>> kv in slicedvalues)
            {
                slices.Add(kv.Key, valuesfloat.GetRange(kv.Value[0], kv.Value[1] - kv.Value[0]));
            }
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
    }
}
