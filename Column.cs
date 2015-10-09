using System.Collections.Generic;

namespace PlotDVT
{
    public class Column
    {
        public string columnname;
        public string alias;
        public string from;
        public int columnnumber;
        public List<string> colvalues;

        public void clearvalues()
        {
            if(colvalues.Count > 0)
                colvalues.Clear();
        }

        public List<string> Columnvalues
        {
            get
            {
                return colvalues;
            }
        }

        public Column()
        {

        }
    }

}
