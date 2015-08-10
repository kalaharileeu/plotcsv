using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private PlotVdcpowermeter plotvdcpowermeter;
        //private PlotVdcconfigured plotvdcconfigured;
        private PlotVdcpcu plotvdcpcu;
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
        private PlotVdcpcu plotvdcpcubl;
        private List<Column> wantedcolumnsbl;

        private string filenamebl;
        private string filenamedata;

        public Form1()
        {
            InitializeComponent();
            //Initialize the list of wanted columns
            wantedcolumns = new List<Column>();
            //values for the last measured result
            XmlManager<DataCol> columnloader = new XmlManager<DataCol>();
            datacolumns = columnloader.Load("Content/XMLFile1.xml");
            realpowerdict = new Dictionary<string, Column>();
            columnobjectlist = new List<Baselist>();
            //diff variable here baseline values and loaders
            XmlManager<DataCol> columnloaderbl = new XmlManager<DataCol>();
            datacolumnsbl = columnloaderbl.Load("Content/XMLFile1.xml");
            realpowerdictbl = new Dictionary<string, Column>();
            columnobjectlistbl = new List<Baselist>();
            wantedcolumnsbl = new List<Column>();

            filenamedata = "C:/2015y07m11d_04h48m53s_SN121519038545_S230_60_LN_RealPwrMap2.csv";
            populatedatatestunit(filenamedata);
            //****************************************populate the diff data******************************************************
            filenamebl = "C:/2015y07m11d_04h48m53s_SN121519038545_S230_60_LN_RealPwrMap2.csv";
            populatedatabaseline(filenamebl);
            //************************************************
            //test for some answers
            Richtextedit();
            //Xrid lines
            //this.chart1.ChartAreas[0].AxisX.Minimum = 13;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            this.chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            this.chart1.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //Xrid lines
            this.chart2.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            this.chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            this.chart2.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //Ygrid lines
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            this.chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            this.chart1.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //YGrid lines
            this.chart2.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            this.chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            this.chart2.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //plotIdc();
        }
        /// <summary>
        /// populating data for the baseline values
        /// </summary>
        /// <param name="filename"></param>
        private void populatedatabaseline(string filename)
        {
            //clear the values id needed
            if(wantedcolumnsbl.Count > 0)
                wantedcolumnsbl.Clear();
            if(realpowerdictbl.Count > 0)
                realpowerdictbl.Clear();
            if(columnobjectlistbl.Count > 0)
                columnobjectlistbl.Clear();

            if (File.Exists(filename))
            {
                
            }
            else
            {
                MessageBox.Show("The baseline file: !exist");
                return;
            }
            //datacolumnsbl come from xml so do not clear it
            foreach (Column c in datacolumnsbl.namealiaslist)
            {
                //Clear the values in the columns
                c.clearvalues();
                wantedcolumnsbl.Add(c);
            }
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(filename))
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
            //Plot stuff baseline values
            plotidcpowermeterbl = new PlotIdcpowermeter(columnobjectlistbl);
            plotidcpcubl = new PlotIdcpcu(columnobjectlistbl);
            plotidcconfigurebl = new PlotIdcconfigured(columnobjectlistbl);
            plotefficiencybl = new PlotEfficiency(columnobjectlistbl);
            plotvdcpcubl = new PlotVdcpcu(columnobjectlistbl);
        }
        //************************************Done populating data for baseline*********************
        /// <summary>
        /// Populate data for the unit under test
        /// </summary>
        /// <param name="filename"></param>
        private void populatedatatestunit(string filename)
        {
            if (wantedcolumns.Count > 0)
                wantedcolumns.Clear();
            if (realpowerdict.Count > 0)
                realpowerdict.Clear();
            if (columnobjectlist.Count > 0)
                columnobjectlist.Clear();

            if (File.Exists(filename))
            {

            }
            else
            {
                MessageBox.Show("The data file: !exist");
                return;
            }
            //datacolumnsbl come from xml so do not clear it
            foreach (Column c in datacolumns.namealiaslist)
            {
                //Clear the values in the columns
                c.clearvalues();
                wantedcolumns.Add(c);
            }
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(filename))
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

            //columnobjectlist = new List<Baselist>();
            foreach (var VAR in realpowerdict)
            {
                columnobjectlist.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
            }
            //Plot Data original
            plotidcpowermeter = new PlotIdcpowermeter(columnobjectlist);
            plotidcpcu = new PlotIdcpcu(columnobjectlist);
            plotidcconfigure = new PlotIdcconfigured(columnobjectlist);
            plotefficiency = new PlotEfficiency(columnobjectlist);
            plotvdcpowermeter = new PlotVdcpowermeter(columnobjectlist);
            plotvdcpcu = new PlotVdcpcu(columnobjectlist);
        }
//**********************************************Done populating data for unit undertest*********************
        //Efficiency button
        private void button1_Click(object sender, EventArgs e)
        {
            ploteff();
        }

        public void plotIdc()
        {
            this.chart1.Series.Clear();
            //this.chart1.ChartAreas[0].AxisX.Minimum = 13;
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;

            this.chart2.Series.Clear();
            //this.chart2.ChartAreas[0].AxisX.Minimum = 13;
            this.chart2.ChartAreas[0].AxisY.Minimum = 0;
            //plot diagonal grey conf line
            foreach (var kv in plotidcconfigure.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_conf";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Line;
                this.chart1.Series[chartseries].Color = Color.CadetBlue;
                this.chart1.Series[chartseries].BorderWidth = 8;
                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
            //plot the power meter on chart1
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
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart1.Series[chartseries].MarkerSize = 7;
                this.chart1.Series[chartseries].Color = Color.Black;
                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
            // plot the DIFF V pcu on chart 2
            foreach (var kv in plotidcpcubl.GetSlices)
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
            //plot the pcu on chart 1
            foreach (var kv in plotidcpcu.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_pcu";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart1.Series[chartseries].MarkerSize = 8;
                this.chart1.Series[chartseries].Color = Color.DarkOrange;
                // plot the pcu on chart 2
                //Add a series
                this.chart2.Series.Add(chartseries);
                //set the chart type
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart2.Series[chartseries].MarkerSize = 8;
                this.chart2.Series[chartseries].Color = Color.DarkOrange;
                //Scale the x axis

                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }
        /// <summary>
        /// Diff plotting starts
        /// </summary>
        private void ploteff()
        {
            this.chart1.Series.Clear();
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
//***************************************Diff plotting ends**********************************

//***************************************DC vOLT plot start here*****************************
        /// <summary>
        /// Plot the dc voltage power meter value on chart area
        /// </summary>
        public void PlotVdccompare()
        {
            this.chart1.Series.Clear();
            this.chart1.ChartAreas[0].AxisX.Minimum = 13;
            this.chart1.ChartAreas[0].AxisY.Minimum = 13;

            this.chart2.Series.Clear();
            this.chart2.ChartAreas[0].AxisX.Minimum = 13;
            this.chart2.ChartAreas[0].AxisY.Minimum = 13;
            //plot diagonal grey conf line
            string series = "_conf";
            //Add a series
            this.chart1.Series.Add(series);
            //set the chart type
            this.chart1.Series[series].ChartType = SeriesChartType.Line;
            this.chart1.Series[series].Color = Color.CadetBlue;
            this.chart1.Series[series].BorderWidth = 8;
            foreach (var kv in plotvdcpcu.GetSlices)
            {
                //Name the series
                this.chart1.Series[series].Points.AddXY(kv.Key, kv.Key);
            }
            //plot the power meter on chart1
            foreach (var kv in plotvdcpowermeter.GetSlices)
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
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart1.Series[chartseries].MarkerSize = 7;
                this.chart1.Series[chartseries].Color = Color.Black;

                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
            // plot the DIFF V pcu on chart 2
            foreach (var kv in plotvdcpcubl.GetSlices)
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
            //plot the pcu on chart 1
            foreach (var kv in plotvdcpcu.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_pcu";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart1.Series[chartseries].MarkerSize = 8;
                this.chart1.Series[chartseries].Color = Color.DarkOrange;
                // plot the pcu on chart 2
                //Add a series
                this.chart2.Series.Add(chartseries);
                //set the chart type
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart2.Series[chartseries].MarkerSize = 8;
                this.chart2.Series[chartseries].Color = Color.DarkOrange;
                //Scale the x axis

                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }
//************************************DC VOLT plot STOPS HERE***********************************
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
        //Load the baseline file
        private void button3_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            //string filename = "nothing";
            if (result == DialogResult.OK) // Test result.
            {
                filenamebl = openFileDialog1.FileName;
            }
            populatedatabaseline(filenamebl);
            //Console.WriteLine(filenamebl); // <-- For debugging use.
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlotVdccompare();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            plotIdc();
        }
        //load the unit under test file
        private void button5_Click(object sender, EventArgs e)
        {
            this.openFileDialog2.Filter = "csv files (*.csv)|*.csv";
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            //string filename = "nothing";
            if (result == DialogResult.OK) // Test result.
            {
                filenamedata = openFileDialog1.FileName;
            }
            populatedatatestunit(filenamedata);

        }
    }
}
