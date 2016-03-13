using System.Collections.Generic;

namespace PlotDVT
{
    /// <summary>
    /// This class horlds a list of bugs for a specific csv file or condition
    /// </summary>
    class Bugs
    {
        public Bugs()
        {
            bugrows = new List<CSVrow>();
        }

        public void Addbug(CSVrow row)
        {
            bugrows.Add(row);
        }

        public List<CSVrow> Bugrows
        {
            get { return bugrows; }
        }
        //The nameing convention comes from xml XMLFile1.xml
        List<CSVrow> bugrows;
    }
}
