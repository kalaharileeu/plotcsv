using System;
using System.Collections.Generic;
using System.Text;


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

        public string Humantext(List<string> valueforreport)
        {
            //{"Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured", "Phaseconfigured", "Temperature", "Vacpowermeter" };
            StringBuilder builder = new StringBuilder();
            foreach (string s in valueforreport)
            {
                foreach (var i in rowkeyvalue)
                {
                    if (i.Key.Contains(s))
                    {
                        if ((i.Value is string) && (i.Value.Length > 1))//check if value is string
                        {
                            builder.Append(gettextshortform(i.Key) + ":" +
                                string.Format("{0:0.00}", Convert.ToDouble(i.Value)) + ", ");
                        }
                        else
                        {
                            builder.Append(i.Key + " : " + "default" + ", ");
                        }
                        break;
                    }
                }
            }
            return builder.ToString();
        }
        //Return a shorter form of the text so that it fits in the richtextbox1
        private string gettextshortform(string longname)
        {
            if (longname == "Temperature")
                return "Temp";
            if (longname == "Wacvarconfigured")
                return "VARcnf";
            if (longname == "Wdcconfigured")
                return "WDCcnf";
            if (longname == "Vdcconfigured")
                return "VDCcnf";
            if (longname == "Vacpowermeter")
                return "VACpm";
            if (longname == "Phaseconfigured")
                return "Phasecnf";
            if (longname == "Wacconfigured")
                return "WACcnf";
            else
                return longname;
        }

        Dictionary<string, string> rowkeyvalue;
    }
}
