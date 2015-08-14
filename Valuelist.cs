using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Valuelist : Baselist
    {
        protected List<float> valuesfloat;
        protected List<string> valuesstring;
        protected float maxvalue;
        protected float minvalue;
        protected float average;
        protected Dictionary<float, List<float>> slices;

        public Valuelist(List<string> stringvaluelist)
        {
            slices = new Dictionary<float, List<float>>();
            valuesfloat = new List<float>();
            valuesstring = stringvaluelist;
            ConvertToFloat(stringvaluelist);
            Average();
            Maxvalue();
            Minvalue();
        }

        override public void Populareslices(Dictionary<float, List<int>> slicedvalues)
        {
            foreach (KeyValuePair<float, List<int>> kv in slicedvalues)
            {
                slices.Add(kv.Key, valuesfloat.GetRange(kv.Value[0], kv.Value[1] - kv.Value[0]));
            }
        }

        override public void Populareslices(List<Slice> slice)
        {
            foreach (Slice s in slice)
            {
                if (s.phaseangle == 0.0f)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

         override public void Populareslices(List<Slice> slice, float deg)
        {
            if (slices.Count > 0)
                slices.Clear();
            foreach (Slice s in slice)
            {
                if (s.phaseangle == deg)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

        override public Dictionary<float, List<float>> GetSlices()
        {
            return slices;
        }


        public void Average()
        {
            average = valuesfloat.Average();
        }

        public void Maxvalue()
        {
            maxvalue = valuesfloat.Max();
        }

        public void Minvalue()
        {
            minvalue = valuesfloat.Min();
        }

        public void ConvertToFloat(List<string> stringvalue)
        {
             //floatcolvalues = new List<float>();
            float f;
            foreach (string value in stringvalue)
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
    }
}
