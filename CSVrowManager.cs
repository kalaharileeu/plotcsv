using System.Collections.Generic;

namespace PlotDVT
{
    class CSVrowManager
    {
        public CSVrowManager()
        {
        }

        public void Load(List<Column> selectedcolumns)
        {
            this.selectedcolumns = selectedcolumns;
            if(checkcolumnlengths())
            {
                populaterowdata();
            }
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
            rows.Add(new CSVrow());
            for(int i = 0; i < selectedcolumns[0].Columnvalues.Count; i++)
            {
                //get reference to last inserted row
                foreach (Column c in selectedcolumns)
                    rows[rows.Count - 1].Addvlaue(c.alias, c.Columnvalues[i]);
            }
        }

        private List<Column> selectedcolumns;
        private List<CSVrow> rows;
    }
}
