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
        public Dictionary<float, List<float>> Slices;

        abstract public void Populareslices(Dictionary<float, List<int>> slicedvalues);
        abstract public void Populareslices(List<Slice> slice);
        abstract public void Populareslices(List<Slice> slice, float deg);

        abstract public Dictionary<float, List<float>> GetSlices();


    }
}
