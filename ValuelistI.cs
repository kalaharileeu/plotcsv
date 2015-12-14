using System;
using System.Collections.Generic;

namespace PlotDVT
{
    public class ValuelistI : IBaselist
    {
        protected List<float> valuesfloat;
        protected List<string> valuesstring;
        protected Dictionary<float, List<float>> slices;//first dictio
        protected Dictionary<float, List<float>> slices2;//Second dictionary for second set of values
        protected string name;

        public ValuelistI(List<string> stringvaluelist, string columnname)
        {
            name = columnname;
            slices = new Dictionary<float, List<float>>();
            slices2 = new Dictionary<float, List<float>>();
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
        /// <summary>
        /// this function populates two dictionaries slices  and slices2
        /// they hold the key value pairs for different degrees slices
        /// </summary>
        /// <param name="slice"></param>
        public void Populareslices(List<Slice> slice)
        {
            foreach (Slice s in slice)
            {
                if (s.phaseangle == 0.0f)
                {
                    if (!slices.ContainsKey(s.vfloat))
                        slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
                    else
                        slices2.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
                }

            }
        }
        /// <summary>
        /// This function populates a dictionary of slices and slices2 for different degrees
        /// they hold the key value pairs for different degrees slices
        /// </summary>
        /// <param name="slice"></param>
        /// <param name="deg"></param>
        public void Populareslices(List<Slice> slice, float deg)
        {
            ///Clear all the values in Slices so that it can repopulated with other values
            ///in different degrees. These dictionaries hold the plot data
            if (slices.Count > 0)
                slices.Clear();
            if (slices2.Count > 0)
                slices2.Clear();
            foreach (Slice s in slice)
            {
                if (s.phaseangle == deg)
                {
                    if (!slices.ContainsKey(s.vfloat))
                        slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
                    else
                        slices2.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
                }
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
