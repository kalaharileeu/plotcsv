using System.Collections.Generic;

namespace PlotDVT
{
    /// <summary>
    /// Takes the data from selected rows and creates rows
    /// as that reflects the CSV data, without unwanted data
    /// </summary>
    class CSVrowManager
    {
        public CSVrowManager()
        {
            rows = new List<CSVrow>();
        }

        public void Load(List<Column> selectedcolumns)
        {
            this.selectedcolumns = selectedcolumns;
            if(checkcolumnlengths())
            {
                populaterowdata();
            }
        }
        //Get value
        public CSVrow GetaCSVrow(int rownumber)
        {
            if(rownumber < rows.Count)
            {
                return rows[rownumber];
            }
            return null;
        }

        //Return all the rows
        public List<CSVrow> Rows
        {
            get { return rows; }
        }
        //all the data counts must be the same
        private bool checkcolumnlengths()
        {
            int length = selectedcolumns[0].Columnvalues.Count;
            foreach (Column c in selectedcolumns)
            {
                if (length != c.Columnvalues.Count)
                    return false;
            }
            return true;
        }

        private void populaterowdata()
        {
            // int i = selectedcolumns[0].Columnvalues.Count;
            for (int i = 0; i < selectedcolumns[0].Columnvalues.Count - 1; i++)
            {
                rows.Add(new CSVrow());
                //get reference to last inserted row
                foreach (Column c in selectedcolumns)
                    rows[rows.Count - 1].Addvlaue(c.alias, c.Columnvalues[i]);
            }
        }

        private List<Column> selectedcolumns;
        private List<CSVrow> rows;
    }
}
