using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    interface IBaselist
    {
        //Inteface cannot contain fields
        void Populareslices(Dictionary<float, List<int>> slicedvalues);
        void Populareslices(List<Slice> slice);
        void Populareslices(List<Slice> slice, float deg);
        Dictionary<float, List<float>> GetSlices();
        void ConvertToFloat();
        List<float> GetFloats();
    }
}
