using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlotDVT
{
    public partial class Form1 : Form
    {
        List<Column> wantedcolumns;//stores a list of wanted data rows
        int rowcount;
        DataCol datacolumns;
        Dictionary<string, Column> realpowerdict;
        RealPowerAnswer realpoweranswers;
        private List<Baselist> columnobjectlist;
        private PlotIdcpowermeter plotidcpowermeter;

        public Form1()
        {
            InitializeComponent();
            //Initialize the list of wanted columns
            wantedcolumns = new List<Column>();
            XmlManager<DataCol> columnloader = new XmlManager<DataCol>();
            datacolumns = columnloader.Load("Content/XMLFile1.xml");
            realpowerdict = new Dictionary<string, Column>();

            foreach(Column c in datacolumns.namealiaslist)
            {
                wantedcolumns.Add(c);
            }
            //var result = engine.ReadFile("C:/2015y07m11d_04h48m53s_SN121519038545_S230_60_LN_RealPwrMap.csv");
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader("C:/2015y07m11d_04h48m53s_SN121519038545_S230_60_LN_RealPwrMap.csv"))
            {
                CsvRow row = new CsvRow();
                rowcount = 0;
                while (reader.ReadRow(row))
                {
                    if (rowcount == 0)
                    {
                        foreach (string s in row)
                        {
                            //the first row is the header
                            foreach (Column c in wantedcolumns)
                                //if the name is in the wanted columns save position
                                if (c.columnname == s)
                                    c.columnnumber = row.IndexOf(s);
                        }
                    }
                    else
                    {
                        foreach (Column c in wantedcolumns)
                        {
                            c.colvalues.Add(row[c.columnnumber]);

                        }
                    }
                    rowcount++;
                }
            }

            foreach (Column c in wantedcolumns)
            {
                realpowerdict.Add(c.alias, c);
            }

            columnobjectlist = new List<Baselist>();
            foreach (var VAR in realpowerdict)
            {
                columnobjectlist.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
            }
            plotidcpowermeter = new PlotIdcpowermeter(columnobjectlist);

            //test for some answers
           // realpoweranswers = new RealPowerAnswer(realpowerdict);

            //float maxcurrent = realpoweranswers.FindMaxPmcurrent();
            //string value = Convert.ToString(maxcurrent);
            //this.richTextBox1.AppendText("The maximum powermeter current is: ");
            //this.richTextBox1.AppendText(value + "A\n");
            //plotidcpowermeter = realpoweranswers.GetPlotIdcpowermeter;

            float maxidc = plotidcpowermeter.Max;
        }



        public Dictionary<string, Column> Realpowerdictionary
        {
            get { return realpowerdict; }
        }
    }
}
