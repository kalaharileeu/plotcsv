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
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace PlotDVT
{
    public partial class Form1 : Form
    {
        List<Column> wantedcolumns;//stores a list of wanted data rows
        int rowcount;
        bool reverse = false;
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
        private Plotwdcpcu plotwdcpcu;
        private Plotacvarpowermeter plotacvarpowermeter;
        private PlotPhaseconfigured plotphaseconfigured;
        private Plotwacvarconfigured plotwacvarconfigured;
        private Plotwacconfigured plotwacconfigured;
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
        private Plotwdcpcu plotwdcpcubl;
        private Plotacvarpowermeter plotacvarpowermeterbl;
        private Plotwacvarconfigured plotwacvarconfiguredbl;
        private Plotwacconfigured plotwacconfiguredbl;

        private string filenamebl;
        private string filenamedata;
        private int numberofrows;
        private int numberofrowsbl;

        public Form1()
        {
            InitializeComponent();
            textBox2.Text = "1000";

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
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //Xrid lines
            chart2.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //Ygrid lines
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            //YGrid lines
            chart2.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
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
            plotwdcpcubl = new Plotwdcpcu(columnobjectlistbl);
            plotacvarpowermeterbl = new Plotacvarpowermeter(columnobjectlistbl);
            plotwacvarconfiguredbl = new Plotwacvarconfigured(columnobjectlistbl);
            plotwacconfiguredbl = new Plotwacconfigured(columnobjectlistbl);

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
            plotwdcpcu = new Plotwdcpcu(columnobjectlist);
            plotwdcpowermeter = new Plotwdcpowermeter(columnobjectlist);
            plotacvarpowermeter = new Plotacvarpowermeter(columnobjectlist);
            plotwacvarconfigured = new Plotwacvarconfigured(columnobjectlist);
            plotwacconfigured = new Plotwacconfigured(columnobjectlist);

        }

        private void chartdefaults()
        {
            chart1.ChartAreas[0].BackColor = Color.White;
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.ChartAreas[0].AxisX.Minimum = Double.NaN;
            chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
            chart1.ChartAreas[0].AxisX.Maximum = Double.NaN;
            chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
            //chart1.ChartAreas.Clear();

            chart2.ChartAreas[0].BackColor = Color.White;
            chart2.Series.Clear();
            chart2.Titles.Clear();
            chart2.ChartAreas[0].AxisX.Minimum = Double.NaN;
            chart2.ChartAreas[0].AxisY.Minimum = Double.NaN;
            chart2.ChartAreas[0].AxisX.Maximum = Double.NaN;
            chart2.ChartAreas[0].AxisY.Maximum = Double.NaN;
            //chart2.ChartAreas.Clear();
        }
//**********************************************Done populating data for unit undertest*********************
        //Efficiency button
        private void button1_Click(object sender, EventArgs e)
        {
            ploteff();
        }


        public void plotIdc()
        {
            chartdefaults();

            //chart1.ChartAreas[0].AxisY.Minimum = 0;
            //chart2.ChartAreas[0].AxisY.Minimum = 0;
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
            //    this.chart2.ChartAreas[0].AxisX.Minimum = 13;
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
            chartdefaults();

            foreach (var kv in plotefficiencybl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_baseline";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 9;
                chart2.Series[chartseries].Color = Color.Black;
                //chart2.ChartAreas[0].AxisX.Minimum = 13;
                //chart2.ChartAreas[0].AxisY.Minimum = 80;
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
            chartdefaults();
            foreach (var kv in plotwdcpowermeterbl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "_baseline";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 9;
                chart2.Series[chartseries].Color = Color.Black;
             //   chart2.ChartAreas[0].AxisX.Minimum = 13;
            //    chart2.ChartAreas[0].AxisY.Minimum = 0;
                foreach (var v in kv.Value)
                {
                    chart2.Series[chartseries].Points.AddXY(kv.Key, v);
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
                foreach (var v in kv.Value)
                {
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }

        private void acpowerdiff()
        {
            chartdefaults();
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
             //   this.chart2.ChartAreas[0].AxisX.Minimum = 13;
             //   this.chart2.ChartAreas[0].AxisY.Minimum = 0;
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
            chartdefaults();

    //        chart1.ChartAreas[0].AxisX.Minimum = 13;
     //       chart1.ChartAreas[0].AxisY.Minimum = 13;

            Title title = new Title("Vdc compare: Vdc configured, powermeter and PCU", 
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            this.chart1.Titles.Add(title);
            title.DockedToChartArea = this.chart1.ChartAreas[0].Name;
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
                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
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
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                filenamedata = openFileDialog2.FileName;
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
            plotwdcpcu.CreatSlices(deg);
            plotacvarpowermeter.CreatSlices(deg);
            plotwacvarconfigured.CreatSlices(deg);
            plotwacconfigured.CreatSlices(deg);

            plotidcconfigurebl.CreatSlices(deg);
            plotidcpowermeterbl.CreatSlices(deg);
            plotidcpcubl.CreatSlices(deg);
            plotvdcpcubl.CreatSlices(deg);
            plotefficiencybl.CreatSlices(deg);
            plotwacpowermeterbl.CreatSlices(deg);
            plotwdcpowermeterbl.CreatSlices(deg);
            plotvdcpowermeterbl.CreatSlices(deg);
            plotwdcpcubl.CreatSlices(deg);
            plotacvarpowermeterbl.CreatSlices(deg);
            plotwacvarconfiguredbl.CreatSlices(deg);
            plotwacconfiguredbl.CreatSlices(deg);
        }

        public void Accuracydcv()
        {
            chartdefaults();

            Title title = new Title("PCU Vdc reporting accurancy. Phase andgle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            chart2.Titles.Add(title);
            title.DockedToChartArea = chart2.ChartAreas[0].Name;

            Dictionary<float, List<float>> dictVdcpm =
                new Dictionary<float, List<float>>(plotvdcpowermeterbl.GetSlices);
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
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.Black;

                if (dictVdcpm.ContainsKey(kv.Key))
                    values = dictVdcpm[kv.Key];

                if (values.Count <= kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = kv.Value[i] - values[i];
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
                        float accuracy = kv.Value[i] - values[i];
                        float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                        this.chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }
        }
        //plot the accuracy of idc
        public void Accuracyicv()
        {
            chartdefaults();

            Title title = new Title("PCU Idc reporting accurancy. Phase andgle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            chart2.Titles.Add(title);
            title.DockedToChartArea = chart2.ChartAreas[0].Name;

            Dictionary<float, List<float>> dictidcpm =
                new Dictionary<float, List<float>>(plotidcpowermeterbl.GetSlices);
            List<float> values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotidcpcubl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "accuracybl";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.Black;

                if (dictidcpm.ContainsKey(kv.Key))
                    values = dictidcpm[kv.Key];

                if (values.Count <= kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = kv.Value[i] - values[i];
                        float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                        chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }

            dictidcpm = new Dictionary<float, List<float>>(plotidcpowermeter.GetSlices);
            values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotidcpcu.GetSlices)
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

                if (dictidcpm.ContainsKey(kv.Key))
                    values = dictidcpm[kv.Key];

                if (values.Count == kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = kv.Value[i] - values[i];
                        float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                        this.chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }
        }

        public void Accuracywdc()
        {
            chartdefaults();

            Title title = new Title("PCU Wdc reporting accurancy. Phase andgle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            chart2.Titles.Add(title);
            title.DockedToChartArea = chart2.ChartAreas[0].Name;

            Dictionary<float, List<float>> dictwdcpm =
                new Dictionary<float, List<float>>(plotwdcpowermeterbl.GetSlices);
            List<float> values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotwdcpcubl.GetSlices)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "accuracybl";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.Black;

                if (dictwdcpm.ContainsKey(kv.Key))
                    values = dictwdcpm[kv.Key];

                if (values.Count <= kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = kv.Value[i] - values[i];
                        float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                        this.chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }

            Dictionary<float, List<float>> dictwdcpm2 = new Dictionary<float, List<float>>(plotwdcpowermeter.GetSlices);
            values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            foreach (var kv in plotwdcpcu.GetSlices)
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

                if (dictwdcpm2.ContainsKey(kv.Key))
                    values = dictwdcpm2[kv.Key];

                if (values.Count <= kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        float accuracy = kv.Value[i] - values[i];
                        float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                        chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                    }
                }
            }
        }
        /// <summary>
        /// Plots the Var vs real power triangle
        /// </summary>
        public async void acvarvapower()
        {
            //List<YourType> newList = new List<YourType>(oldList);
            chartdefaults();

            Title title = new Title("Powermeter VA against VAR: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 12, FontStyle.Bold), Color.Black);
            chart1.Titles.Add(title);
            title.DockedToChartArea = chart1.ChartAreas[0].Name;

            
            //ChartArea ChartArea0 = new ChartArea("name");
            //chart1.ChartAreas.Add(ChartArea0);
            //Series Series0 = new Series();
            //chart1.Series.Add(Series0);
            //// link series to area here
            //Series0.ChartArea = "name";

                //Below are the values for chart one
            Dictionary<float, List<float>> dictacwpm =
                new Dictionary<float, List<float>>(plotwacpowermeter.GetSlices);
            List<float> values = new List<float>();
            Dictionary<float, List<float>> dictwaccnf =
                new Dictionary<float, List<float>>(plotwacconfigured.GetSlices);
            List<float> waccnf = new List<float>();
            Dictionary<float, List<float>> dictwavarccnf =
                new Dictionary<float, List<float>>(plotwacvarconfigured.GetSlices);
            List<float> wacvarcnf = new List<float>();
            Dictionary<float, List<float>> acvardict =
                new Dictionary<float, List<float>>(plotacvarpowermeter.GetSlices);

            //Chart2 this is for chart 2 the baseline vlues
            Dictionary<float, List<float>> acvardictbl =
                new Dictionary<float, List<float>>(plotacvarpowermeterbl.GetSlices);
            List<float> values2 = new List<float>();
            Dictionary<float, List<float>> dictacwpmbl =
                new Dictionary<float, List<float>>(plotwacpowermeterbl.GetSlices);
            List<float> values3 = new List<float>();
            //congfigured values for chart 2 baseline unit
            Dictionary<float, List<float>> dictwaccnfbl =
                new Dictionary<float, List<float>>(plotwacconfiguredbl.GetSlices);
            List<float> waccnfbl = new List<float>();
            Dictionary<float, List<float>> dictwavarccnfbl =
                new Dictionary<float, List<float>>(plotwacvarconfiguredbl.GetSlices);
            List<float> wacvarcnfbl = new List<float>();
 
            foreach (var kv in acvardict)
            {
                //create the series name from the values 
                string seriesac = (Convert.ToString(kv.Key));
                string seriesvar = "cnf";
                string serieschart2 = "cmp";
                string serieschart2cnf = "cn";
                if (seriesac.Length > 4)
                    seriesac = seriesac.Substring(0, 4);
                //create another series name 
                seriesvar += seriesac;
                seriesac += "Wvar";
                serieschart2 += seriesac;
                serieschart2cnf += seriesac;
                //chart1 stuff
                chart1.Series.Add(seriesac);
                chart1.Series[seriesac].ChartType = SeriesChartType.Line;
                chart1.Series[seriesac].BorderWidth = 2;
                chart1.Series.Add(seriesvar);
                chart1.Series[seriesvar].ChartType = SeriesChartType.Point;
                chart1.Series[seriesvar].MarkerStyle = MarkerStyle.Cross;
                chart1.Series[seriesvar].MarkerSize = 10;
                if (dictacwpm.ContainsKey(kv.Key))
                    values = dictacwpm[kv.Key];
                if (dictwaccnf.ContainsKey(kv.Key))
                    waccnf = dictwaccnf[kv.Key];
                if (dictwavarccnf.ContainsKey(kv.Key))
                    wacvarcnf = dictwavarccnf[kv.Key];

                //chart2: this is for the chart 2 comparison betwwen baseline and real unit
                chart2.Series.Add(serieschart2);
                chart2.Series[serieschart2].ChartType = SeriesChartType.Line;
                chart2.Series[serieschart2].BorderWidth = 2;

                chart2.Series.Add(serieschart2cnf);
                chart2.Series[serieschart2cnf].ChartType = SeriesChartType.Point;
                chart2.Series[serieschart2cnf].MarkerStyle = MarkerStyle.Cross;
                chart2.Series[serieschart2cnf].MarkerSize = 10;

                if (dictacwpmbl.ContainsKey(kv.Key))
                    values2 = dictacwpmbl[kv.Key];
                if (acvardictbl.ContainsKey(kv.Key))
                    values3 = acvardictbl[kv.Key];
                if (dictwaccnfbl.ContainsKey(kv.Key))
                    waccnfbl = dictwaccnfbl[kv.Key];
                if (dictwavarccnfbl.ContainsKey(kv.Key))
                    wacvarcnfbl = dictwavarccnfbl[kv.Key];
                

                if (values.Count <= kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        //Plot the congfigured w/va powermeter values
                        chart1.Series[seriesac].Points.AddXY(0, 0);
                        chart1.Series[seriesac].Points.AddXY(values[i], 0);
                        chart1.Series[seriesac].Points.AddXY(values[i], kv.Value[i]);
                        //Plot the congfigured w/va configured values
                        chart1.Series[seriesvar].Points.AddXY(0, 0);
                        chart1.Series[seriesvar].Points.AddXY(waccnf[i], 0);
                        chart1.Series[seriesvar].Points.AddXY(waccnf[i], wacvarcnf[i]);

                        if ((i < values2.Count) && (i < values3.Count))
                        {
                            chart2.Series[serieschart2].Points.AddXY(0, 0);
                            chart2.Series[serieschart2].Points.AddXY(values2[i], 0);
                            chart2.Series[serieschart2].Points.AddXY(values2[i], values3[i]);
                            //Plot the congfigured w/va configured values
                            chart2.Series[serieschart2cnf].Points.AddXY(0, 0);
                            chart2.Series[serieschart2cnf].Points.AddXY(waccnfbl[i], 0);
                            chart2.Series[serieschart2cnf].Points.AddXY(waccnfbl[i], wacvarcnfbl[i]);

                        }
                        await Task.Delay(Convert.ToInt16(textBox2.Text));
                    }
                    if (reverse)//not actually reverse, just delete previous plot
                    {
                        chart1.Series[seriesac].Points.Clear();
                        chart1.Series[seriesvar].Points.Clear();
                        chart2.Series[serieschart2].Points.Clear();
                        chart2.Series[serieschart2cnf].Points.Clear();
                    }
                }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //Clear the charts
            chartdefaults();

            ChartArea accurrent = new ChartArea("ac");
            Dcacplot plotdcac = new Dcacplot();
            plotdcac.plotdcaccombo(chart1, accurrent);

            ////create new chart area, give it a name ex. "ac"
            //ChartArea accurrent = new ChartArea("ac");
            //chart1.ChartAreas.Add(accurrent);
            ////create a new series
            //Series dcseries = new Series();
            //chart1.Series.Add(dcseries);
            ////link the series to the area
            //dcseries.ChartArea = "ac";
            //dcseries.ChartType = SeriesChartType.Point;
            //dcseries.MarkerStyle = MarkerStyle.Cross;
            //dcseries.MarkerSize = 13;
            //dcseries.Points.AddXY(10, 0);

            //chart1.ChartAreas.Remove(accurrent);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            float deg = -45.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Text = button6.Text;
            textBox1.Text = button6.Text;
            changeplotdegrees(deg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            float deg = -30.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button7.Text;
            changeplotdegrees(deg);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            float deg = -15.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button8.Text;
            changeplotdegrees(deg);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            float deg = 0.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button9.Text;
            changeplotdegrees(deg);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            float deg = 15.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button10.Text;
            changeplotdegrees(deg);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            float deg = 30.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button11.Text;
            changeplotdegrees(deg);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            float deg = 45.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
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
            Accuracydcv();
        }
        //private async void writeto plot 
        private void button16_Click(object sender, EventArgs e)
        {
            Accuracyicv();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Accuracywdc();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            acvarvapower();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if(!reverse)
            {
                reverse = true;
                button19.BackColor = Color.DarkGreen;
            }
            else
            {
                reverse = false;
                button19.BackColor = Color.LightYellow;
            }
                
        }
    }
}
