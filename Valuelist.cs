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

        public Valuelist(List<string> stringvaluelist)
        {
            valuesfloat = new List<float>();
            valuesstring = stringvaluelist;
            ConvertToFloat(stringvaluelist);
            Average();
            Maxvalue();
            Minvalue();
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
