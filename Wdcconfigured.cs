using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Wdcconfigured : Valuelist
    {
        public Wdcconfigured(List<string> stringvaluelist) : base(stringvaluelist)
        {
            //valuesstring = stringvaluelist;
        }
        //new keyword to overide the baselist GetAverage
        public new float GetAverage
        {
            get { return average; }
        }

        public new float GetMax
        {
            get { return maxvalue;}
        }

        public new float GetMin
        {
            get { return minvalue;}
        }

        public new List<float> GetFloats
        {
            get { return valuesfloat; }
        }
    }
}
