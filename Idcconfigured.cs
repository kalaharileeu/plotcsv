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
        private Dictionary<int, Slice> claslices;

        public Idcconfigured(List<string> stringvaluelist) : base(stringvaluelist)
        {
            //valuesstring = stringvaluelist;
            slices = new Dictionary<float, List<float>>();
            claslices = new Dictionary<int, Slice>();
        }

        public void Populareslices(Dictionary<float, List<int>> slicedvalues)
        {
            foreach (KeyValuePair<float, List<int>> kv in slicedvalues)
            {
                slices.Add(kv.Key, valuesfloat.GetRange(kv.Value[0], kv.Value[1] - kv.Value[0]));
            }
        }

        public void Populareslices(List<Slice> slice)
        {
            foreach (Slice s in slice)
            {
                if(s.phaseangle == 0.0f)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

        public void Populareslices(List<Slice> slice, float deg)
        {
            if(slices.Count > 0)
                slices.Clear();
            foreach (Slice s in slice)
            {
                if (s.phaseangle == deg)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
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

        public Dictionary<float, List<float>> Slices
        {
            get { return slices; }
        }

        public Dictionary<int, Slice> GetClaslices
        {
            get { return claslices; }
        }
    }
}
