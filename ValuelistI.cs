using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    public class ValuelistI : IBaselist
    {
        protected List<float> valuesfloat;
        protected List<string> valuesstring;
        protected Dictionary<float, List<float>> slices;
        protected string name;

        public ValuelistI(List<string> stringvaluelist, string columnname)
        {
            name = columnname;
            slices = new Dictionary<float, List<float>>();
            valuesfloat = new List<float>();
            valuesstring = stringvaluelist;
            ConvertToFloat();
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
                if (s.phaseangle == 0.0f)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

        public void Populareslices(List<Slice> slice, float deg)
        {
            if (slices.Count > 0)
                slices.Clear();
            foreach (Slice s in slice)
            {
                if (s.phaseangle == deg)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

        public Dictionary<float, List<float>> GetSlices()
        {
            return slices;
        }

        public void ConvertToFloat()
        {
            //floatcolvalues = new List<float>();
            float f;
            foreach (string value in valuesstring)
            {
                f = 0.0f;
                try
                {
                    f = float.Parse(value);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                valuesfloat.Add(f);
            }
        }

        public List<float> GetFloats()
        {
            return valuesfloat;
        }

        public string GetName()
        {
            return name;
        }
    }
}
