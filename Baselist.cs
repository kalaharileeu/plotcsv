using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    abstract class Baselist
    {
        protected string columnname;
        protected string alias;
        protected string from;
        protected int columnnumber;
        //below should be virtual pssoibly
        public float GetAverage;
        public float GetMin;
        public float GetMax;
        public List<float> GetFloats;
    }
}
