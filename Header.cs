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
    }
}
