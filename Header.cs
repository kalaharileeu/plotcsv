using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Header : Baselist
    {
        List<string> headerlist;
 
        public Header(List<string> headerlist)
        {
            headerlist = new List<string>(headerlist);    
        }

        public List<string> Headerlist
        {
            get { return headerlist;}
        }

        override public void Populareslices(Dictionary<float, List<int>> slicedvalues)
        {}

        override public void Populareslices(List<Slice> slice)
        {}

        override public void Populareslices(List<Slice> slice, float deg)
        {}

        override public Dictionary<float, List<float>> GetSlices()
        {
            return new Dictionary<float, List<float>>();
        }
    }
}
