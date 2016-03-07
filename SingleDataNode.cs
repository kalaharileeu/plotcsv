using System.Collections.Generic;

namespace PlotDVT
{
    /// <summary>
    /// Represent a single data row name value pair in dictionary
    /// Dictionary with the wanted data and their alias/description
    /// used by CSVrowManager
    /// </summary>
    public class CSVrow
    {
        public CSVrow()
        {
            rowkeyvalue = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Rowkeyvalue
        {
            get { return rowkeyvalue; }
        }

        public void Addvlaue(string key, string value)
        {
            rowkeyvalue.Add(key, value);
        }
        
        Dictionary<string, string> rowkeyvalue;
    }
}
