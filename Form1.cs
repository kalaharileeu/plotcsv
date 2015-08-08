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
using System.Windows.Forms.DataVisualization.Charting;


namespace PlotDVT
{
    public partial class Form1 : Form
    {
        List<Column> wantedcolumns;//stores a list of wanted data rows
        int rowcount;
        DataCol datacolumns;
        Dictionary<string, Column> realpowerdict;
        //RealPowerAnswer realpoweranswers;
        private List<Baselist> columnobjectlist;
        private PlotIdcpowermeter plotidcpowermeter;
        private PlotIdcpcu plotidcpcu;
        private PlotIdcconfigured plotidcconfigure;
        private PlotEfficiency plotefficiency;
        /// <summary>
        /// Baseline variables below to be used to create the diff
        /// </summary>
        private DataCol datacolumnsbl;
        Dictionary<string, Column> realpowerdictbl;
        private List<Baselist> columnobjectlistbl;
        private PlotIdcpowermeter plotidcpowermeterbl;
        private PlotIdcpcu plotidcpcubl;
        private PlotIdcconfigured plotidcconfigurebl;
        private PlotEfficiency plotefficiencybl;
        private List<Column> wantedcolumnsbl;

        public Form1()
        {
            InitializeComponent();
            //Initialize the list of wanted columns
            wantedcolumns = new List<Column>();

            //values for the last measured result
            XmlManager<DataCol> columnloader = new XmlManager<DataCol>();
            datacolumns = columnloader.Load("Content/XMLFile1.xml");
            realpowerdict = new Dictionary<string, Column>();

            //diff variable here baseline values and loaders
            XmlManager<DataCol> columnloaderbl = new XmlManager<DataCol>();
            datacolumnsbl = columnloaderbl.Load("Content/XMLFile1.xml");
            realpowerdictbl = new Dictionary<string, Column>();
            columnobjectlistbl = new List<Baselist>();
            wantedcolumnsbl = new List<Column>();

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
            //****************************************populate the diff data******************************************************
            populatedata();
            //************************************************

            //Plot stuff baseline values
            plotidcpowermeterbl = new PlotIdcpowermeter(columnobjectlistbl);
            plotidcpcubl = new PlotIdcpcu(columnobjectlistbl);
            plotidcconfigurebl = new PlotIdcconfigured(columnobjectlistbl);
            plotefficiencybl = new PlotEfficiency(columnobjectlistbl);

            //Plot stuff original
            plotidcpowermeter = new PlotIdcpowermeter(columnobjectlist);
            plotidcpcu = new PlotIdcpcu(columnobjectlist);
            plotidcconfigure = new PlotIdcconfigured(columnobjectlist);
            plotefficiency = new PlotEfficiency(columnobjectlist);
            //test for some answers
            Richtextedit();
            plotIdcconf();
            //PlotIdcPm();
            plotIdcPcu();
            PlotIdcPm();

            plotdiff();

            //ploteff();


        }

        private void populatedata()
        {
            foreach (Column c in datacolumnsbl.namealiaslist)
            {
                wantedcolumnsbl.Add(c);
            }
            //var result = engine.ReadFile("C:/2015y07m11d_04h48m53s_SN121519038545_S230_60_LN_RealPwrMap.csv");
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader("C:/baseline/2015y06m06d_00h38m53s_SN121519038558_S230_60_LN_RealPwrMapBurst.csv"))
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
                            foreach (Column c in wantedcolumnsbl)
                                //if the name is in the wanted columns save position
                                if (c.columnname == s)
                                    c.columnnumber = row.IndexOf(s);
                        }
                    }
                    else
                    {
                        foreach (Column c in wantedcolumnsbl)
                        {
                            c.colvalues.Add(row[c.columnnumber]);
                        }
                    }
                    rowcount++;
                }
            }

            foreach (Column c in wantedcolumnsbl)
            {
                realpowerdictbl.Add(c.alias, c);
            }

            //columnobjectlistbl = new List<Baselist>();
            foreach (var VAR in realpowerdictbl)
            {
                columnobjectlistbl.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ploteff();
        }


        /// <summary>
        /// Plot the power meter value on chart area
        /// </summary>
        public void PlotIdcPm()
        {
            foreach (var kv in plotidcpowermeter.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_pm";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart1.Series[chartseries].MarkerSize = 7;
                this.chart1.Series[chartseries].Color = Color.DarkOrange;
                //Scale the x axis
                this.chart1.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                   this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }

        public void plotIdcPcu()
        {
            foreach (var kv in plotidcpcu.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_pcu";
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart1.Series[chartseries].MarkerSize = 8;
                this.chart1.Series[chartseries].Color = Color.Black;
                //Scale the x axis
                //this.chart1.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }

        public void plotIdcconf()
        {
            foreach (var kv in plotidcconfigure.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if(chartseries.Length > 4)
                    chartseries = chartseries.Substring(0,4);
                chartseries += "_conf";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Line;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.SemiTransparent;
                this.chart1.Series[chartseries].Color = Color.DarkGray;
                this.chart1.Series[chartseries].BorderWidth = 12;
                //Scale the x axis
                //this.chart1.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }

        private void plotdiff()
        {
            foreach (var kv in plotidcpcu.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_baseline";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart2.Series[chartseries].MarkerSize = 9;
                this.chart2.Series[chartseries].Color = Color.Black;
                this.chart2.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }

            foreach (var kv in plotidcpcubl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_testpcu";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart2.Series[chartseries].MarkerSize = 9;
                this.chart2.Series[chartseries].Color = Color.DarkOrange;
                //this.chart1.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }

        private void ploteff()
        {
            this.chart2.Series.Clear();
            foreach (var kv in plotefficiency.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_baseline";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart2.Series[chartseries].MarkerSize = 9;
                this.chart2.Series[chartseries].Color = Color.Black;
                this.chart2.ChartAreas[0].AxisX.Minimum = 13;
                this.chart2.ChartAreas[0].AxisY.Minimum = 80;
                foreach (var v in kv.Value)
                {
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }

            foreach (var kv in plotefficiencybl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_testpcu";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart2.Series[chartseries].MarkerSize = 9;
                this.chart2.Series[chartseries].Color = Color.DarkOrange;
                //this.chart1.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }

        public void Richtextedit()
        {
            float maxidc = plotidcpowermeter.GetMax;
            string value = Convert.ToString(maxidc);
            this.richTextBox1.AppendText("Idc max (powermeter): ");
            this.richTextBox1.AppendText(value + "A\n");
            value = Convert.ToString(plotidcpowermeter.GetMin);
            this.richTextBox1.AppendText("Idc min (powermeter): ");
            this.richTextBox1.AppendText(value + "A\n");
        }

        public Dictionary<string, Column> Realpowerdictionary
        {
            get { return realpowerdict; }
        }


    }
}
