using System.Collections.Generic;

namespace PlotDVT
{
    /// <summary>
    /// This class horlds a list of bugs for a specific csv file or condition
    /// </summary>
    class Bugs
    {
        public Bugs(int chartarea_no)
        {
            bugrows = new List<CSVrow>();
            this.chartarea_no = chartarea_no;
        }

        public void Addbug(CSVrow row)
        {
            if (row != null)
            {
                bugrows.Add(row);
            }
        }

        public List<CSVrow> Bugrows
        {
            get { return bugrows; }
        }

        public int Getchartno()
        {
            return chartarea_no;
        }

        //The nameing convention comes from xml XMLFile1.xml
        private List<CSVrow> bugrows;
        //in what chart area does my plots live
        private int chartarea_no;
    }
}
