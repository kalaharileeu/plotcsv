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
        private Plotwacpowermeter plotwacpowermeter;
        private Plotwdcpowermeter plotwdcpowermeter;
        private PlotPhaseconfigured plotphaseconfigured;
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
        private PlotPhaseconfigured plotphaseconfiguredbl;
        private Plotwacpowermeter plotwacpowermeterbl;
        private Plotwdcpowermeter plotwdcpowermeterbl;
        private List<Column> wantedcolumnsbl;
        private PlotVdcpowermeter plotvdcpowermeterbl;

        private string filenamebl;
        private string filenamedata;
        private int numberofrows;
        private int numberofrowsbl;

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

            filenamedata = "C:/values/2015y06m19d_16h55m35s_SN121519038551_S230_60_LN_ReactivePwrMap.csv";
            populatedatatestunit(filenamedata);
            //****************************************populate the diff data******************************************************
            filenamebl = "C:/values/2015y07m10d_15h24m11s_SN121519038545_S230_60_LN_ReactivePwrMap.csv";
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
            this.textBox1.Text = "0°";
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
                Richtextedit("Baseline rows imported: " + Convert.ToString(rowcount));
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
            plotphaseconfiguredbl = new PlotPhaseconfigured(columnobjectlistbl);
            plotidcpowermeterbl = new PlotIdcpowermeter(columnobjectlistbl);
            plotidcpcubl = new PlotIdcpcu(columnobjectlistbl);
            plotidcconfigurebl = new PlotIdcconfigured(columnobjectlistbl);
            plotefficiencybl = new PlotEfficiency(columnobjectlistbl);
            plotvdcpcubl = new PlotVdcpcu(columnobjectlistbl);
            plotwacpowermeterbl = new Plotwacpowermeter(columnobjectlistbl);
            plotwdcpowermeterbl = new Plotwdcpowermeter(columnobjectlistbl);
            plotvdcpowermeterbl = new PlotVdcpowermeter(columnobjectlistbl);

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
                Richtextedit("Datarows imported: " + Convert.ToString(rowcount));
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
            plotphaseconfigured = new PlotPhaseconfigured(columnobjectlist);
            plotidcpowermeter = new PlotIdcpowermeter(columnobjectlist);
            plotidcpcu = new PlotIdcpcu(columnobjectlist);
            plotidcconfigure = new PlotIdcconfigured(columnobjectlist);
            plotefficiency = new PlotEfficiency(columnobjectlist);
            plotvdcpowermeter = new PlotVdcpowermeter(columnobjectlist);
            plotvdcpcu = new PlotVdcpcu(columnobjectlist);
            plotwacpowermeter = new Plotwacpowermeter(columnobjectlist);
            plotwdcpowermeter = new Plotwdcpowermeter(columnobjectlist);

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
            this.chart1.Titles.Clear();

            this.chart1.ChartAreas[0].AxisY.Minimum = 0;

            this.chart2.Series.Clear();
            this.chart2.Titles.Clear();
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
            this.chart2.Titles.Clear();
            this.chart1.Titles.Clear();
            foreach (var kv in plotefficiencybl.GetSlices)
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

            foreach (var kv in plotefficiency.GetSlices)
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

        private void dcpowerdiff()
        {
            this.chart1.Series.Clear();
            this.chart2.Series.Clear();
            this.chart2.Titles.Clear();
            this.chart1.Titles.Clear();
            foreach (var kv in plotwdcpowermeterbl.GetSlices)
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

            foreach (var kv in plotwdcpowermeter.GetSlices)
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

        private void acpowerdiff()
        {
            this.chart1.Series.Clear();
            this.chart2.Series.Clear();
            this.chart2.Titles.Clear();
            this.chart1.Titles.Clear();
            foreach (var kv in plotwacpowermeterbl.GetSlices)
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

            foreach (var kv in plotwacpowermeter.GetSlices)
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

            this.chart1.Titles.Clear();
            Title title = new Title("Vdc compare: Vdc configured, powermeter and PCU", 
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            this.chart1.Titles.Add(title);
            title.DockedToChartArea = this.chart1.ChartAreas[0].Name;

            this.chart2.Series.Clear();
            this.chart2.ChartAreas[0].AxisX.Minimum = 13;
            this.chart2.ChartAreas[0].AxisY.Minimum = 13;

            this.chart2.Titles.Clear();
            Title title2 = new Title("Vdc reported: Test pm vs Baseline pm",
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            this.chart2.Titles.Add(title2);
            title.DockedToChartArea = this.chart2.ChartAreas[0].Name;

            //plot diagonal grey conf line
            string series = "_conf";
            //Add a series
            this.chart1.Series.Add(series);
            //set the chart type
            this.chart1.Series[series].ChartType = SeriesChartType.Point;
            this.chart1.Series[series].MarkerStyle = MarkerStyle.Star6;
            this.chart1.Series[series].MarkerSize = 10;
            this.chart1.Series[series].Color = Color.CadetBlue;
            //this.chart1.Series[series].BorderWidth = 8;
            foreach (var kv in plotvdcpcu.GetSlices)
            {
                //Name the series
                this.chart1.Series[series].Points.AddXY(kv.Key, kv.Key);
            }
            series = "conf2";
            //Add a series
            this.chart1.Series.Add(series);
            //set the chart type
            this.chart1.Series[series].ChartType = SeriesChartType.Line;
            this.chart1.Series[series].BorderWidth = 1;
            this.chart1.Series[series].Color = Color.CadetBlue;
            //this.chart1.Series[series].BorderWidth = 8;
            foreach (var kv in plotvdcpcu.GetSlices)
            {
                //Name the series
                this.chart1.Series[series].Points.AddXY(kv.Key, kv.Key);
            }

            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotvdcpowermeterbl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_basel_pm";
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

            //plot the power meter on chart1 and chart 2
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

                // plot the pcu on chart 2
                //Add a series
                this.chart2.Series.Add(chartseries);
                //set the chart type
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart2.Series[chartseries].MarkerSize = 7;
                this.chart2.Series[chartseries].Color = Color.DarkOrange;
                //Scale the x axis

                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
            //plot the pcu on chart 1 and 2
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
                //this.chart2.Series.Add(chartseries);
                ////set the chart type
                //this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                //this.chart2.Series[chartseries].MarkerSize = 8;
                //this.chart2.Series[chartseries].Color = Color.DarkOrange;
                ////Scale the x axis

                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                    //this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
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

        public void Richtextedit(string text)
        {
            this.richTextBox1.AppendText(text + "\n");
        }

        public Dictionary<string, Column> Realpowerdictionary
        {
            get { return realpowerdict; }
        }

        //Load DC voltage compare tables
        private void button2_Click(object sender, EventArgs e)
        {
            PlotVdccompare();
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
            //get the file info from the path
            FileInfo info = new FileInfo(filenamebl);
            //check file info
            if (IsFileLocked(info))
            {
                MessageBox.Show("The file is in use or locked!");
            }
            else
            {
                populatedatabaseline(filenamebl);
            }
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
            if (result == DialogResult.OK) // Test result.
            {
                filenamedata = openFileDialog1.FileName;
            }
            //get the file info from the path
            FileInfo info = new FileInfo(filenamedata);
            //check file info
            if (IsFileLocked(info))
            {
                MessageBox.Show("The file is in use or locked!");
            }
            else
            {
                populatedatatestunit(filenamedata);
            }
        }
        //Check of the file is in use
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            float deg = -45.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button6.Text;
            changeplotdegrees(deg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            float deg = -30.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button7.Text;
            changeplotdegrees(deg);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            float deg = -15.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button8.Text;
            changeplotdegrees(deg);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            float deg = 0.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button9.Text;
            changeplotdegrees(deg);
        }

        private void changeplotdegrees(float deg)
        {
            plotidcconfigure.CreatSlices(deg);
            plotidcpowermeter.CreatSlices(deg);
            plotidcpcu.CreatSlices(deg);

            plotvdcpcu.CreatSlices(deg);
            plotvdcpowermeter.CreatSlices(deg);

            plotefficiency.CreatSlices(deg);
            plotwacpowermeter.CreatSlices(deg);
            plotwdcpowermeter.CreatSlices(deg);

            plotidcconfigurebl.CreatSlices(deg);
            plotidcpowermeterbl.CreatSlices(deg);
            plotidcpcubl.CreatSlices(deg);
            plotvdcpcubl.CreatSlices(deg);
            plotefficiencybl.CreatSlices(deg);
            plotwacpowermeterbl.CreatSlices(deg);
            plotwdcpowermeterbl.CreatSlices(deg);
            plotvdcpowermeterbl.CreatSlices(deg);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            float deg = 15.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button10.Text;
            changeplotdegrees(deg);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            float deg = 30.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button11.Text;
            changeplotdegrees(deg);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            float deg = 45.0f;
            chart1.Series.Clear();
            chart2.Series.Clear();
            textBox1.Clear();
            textBox1.Text = button12.Text;
            changeplotdegrees(deg);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dcpowerdiff();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            acpowerdiff();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Accuracy();
        }

        public void Accuracy()
        {
            this.chart2.Series.Clear();
            this.chart2.Titles.Clear();
            this.chart2.ChartAreas[0].AxisX.Minimum = 13;
            this.chart2.ChartAreas[0].AxisY.Minimum = -1;

            Dictionary<float, List<float>> dictVdcpm = new Dictionary<float, List<float>>(plotvdcpowermeterbl.GetSlices);
            List<float> values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotvdcpcubl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "accuracybl";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart2.Series[chartseries].MarkerSize = 7;
                this.chart2.Series[chartseries].Color = Color.Black;

                if (dictVdcpm.ContainsKey(kv.Key))
                    values = dictVdcpm[kv.Key];

                if (values.Count == kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = ((kv.Value[i] - values[i])/kv.Value[i])*100;
                        float accuracy2 = (float) (Math.Round((double) accuracy, 2));
                        this.chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }

            dictVdcpm = new Dictionary<float, List<float>>(plotvdcpowermeter.GetSlices);
            values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotvdcpcu.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "accuracy";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                this.chart2.Series[chartseries].MarkerSize = 7;
                this.chart2.Series[chartseries].Color = Color.DarkOrange;

                if (dictVdcpm.ContainsKey(kv.Key))
                    values = dictVdcpm[kv.Key];

                if (values.Count == kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = ((kv.Value[i] - values[i]) / kv.Value[i]) * 100;
                        float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                        this.chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }
        }
    }
}
