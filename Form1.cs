using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PlotDVT
{
	//This class is too big.
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Initialize the list of wanted columns
            wantedcolumns = new List<Column>();
            //values for the last measured result
            XmlManager<DataCol> columnloader = new XmlManager<DataCol>();
            datacolumns = columnloader.Load("Content/XMLFile1.xml");
            realpowerdict = new Dictionary<string, Column>();
            //columnobjectlist = new List<Baselist>();
            colobjinterflistbl = new List<IBaselist>();//!
            colobjinterflist = new List<IBaselist>();//!
            //diff variable here baseline values and loaders
            XmlManager<DataCol> columnloaderbl = new XmlManager<DataCol>();
            datacolumnsbl = columnloaderbl.Load("Content/XMLFile1.xml");
            realpowerdictbl = new Dictionary<string, Column>();
            wantedcolumnsbl = new List<Column>();
            phasebuttonarray = new Button[] { button6, button7, button8, button9, button10, button11, button12 };
            phasebuttonlist = new List<Button>(phasebuttonarray);
            phasemarkerstyles = new MarkerStyle[] { MarkerStyle.Circle, MarkerStyle.Cross, MarkerStyle.Diamond, 
                MarkerStyle.Square, MarkerStyle.Triangle, MarkerStyle.Star4, MarkerStyle.Star10 };
            phasemarkerlist = new List<MarkerStyle>(phasemarkerstyles);
            //some files to play with should remove/make another plan later
            filenamedata = "C:/values/2015y09m24d_13h35m42s_SN121538001575_S230_60_LN_LoL_HiL_119.csv";//some test files
            populatedatatestunit(filenamedata);
            //****************************************populate the diff data******************************************************
            filenamebl = "C:/values/2015y09m24d_13h35m42s_SN121538001575_S230_60_LN_LoL_HiL_119.csv";//some test files
            populatedatabaseline(filenamebl);
            //************************************************
            //test for some answers
            Richtextedit();
            setupploatarea();
            //plotIdc();
            this.textBox1.Text = "0°";
        }
        /// <summary>
        /// Setup some default values for the plotting area
        /// </summary>
        private void setupploatarea()
        {
            textBox2.Text = "20";//delay synchronous plotting
            textBox5.Text = "60";//Fullscale Vdc: PCUIP_Vdc_OpratRngeMPPT or PCUIP_Vdc_OpratRngeStart
            textBox6.Text = "12";//Fullscale Idc: PCUIP_IdcLimit_OpratRngeRated
            textBox7.Text = "290";//Vn (ac) :PCUIP_Vac_OpratRngeRated
            textBox8.Text = "1.2";//Iac PCUOP_Sac_OpratRngeRated
            textBox9.Text = "220";//Sac
            textBox10.Text = "56.0";//minimum Sac to ignore
            textBox13.Text = "350";
            textBox11.Text = "294.0";//Fulscale Wdc
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
        }
        /// <summary>
        /// populating data for the baseline file values
        /// </summary>
        /// <param name="filename"></param>
        private void populatedatabaseline(string filename)
        {
            //clear the values id needed
            if(wantedcolumnsbl.Count > 0)
                wantedcolumnsbl.Clear();
            if(realpowerdictbl.Count > 0)
                realpowerdictbl.Clear();
            if (colobjinterflistbl.Count > 0)
                colobjinterflistbl.Clear();

            if (!(File.Exists(filename)))
            {
                MessageBox.Show("The baseline file: !exist");
                return;
            }
            //datacolumnsbl comes from xml so do not clear it
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

            foreach (var VAR in realpowerdictbl)
            {
                /// <summary>
                /// Below the instance is created and!!! the parameter is passes to it. I like it. 
                /// VAR.Value.Columnvalues is the value
                /// </summary>
                //columnobjectlistbl.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
                if (VAR.Key == "Vdcconfigured")
                {
                    ///if it is Vdcconfigured, add it as VdcConfigured, some extra functionaly in Vdcconfigure
                    colobjinterflistbl.Add((IBaselist)Activator.CreateInstance
                        (Type.GetType("PlotDVT.VdcconfiguredI"), VAR.Value.Columnvalues, VAR.Key));

                }
                else if (VAR.Key == "Phaseconfigured")
                {
                    ///if it is Phaseconfigured add it as PhaseCongfigured, some extra functionaly in PhaseCongfigured
                    colobjinterflistbl.Add((IBaselist)Activator.CreateInstance
                        (Type.GetType("PlotDVT.PhaseconfiguredI"), VAR.Value.Columnvalues, VAR.Key));
                }
                else
                {
                    //Add the rest just as ValuelistI
                    colobjinterflistbl.Add((IBaselist)Activator.CreateInstance(Type.GetType("PlotDVT.ValuelistI"), VAR.Value.Columnvalues, VAR.Key));
                }
            }

            foreach (IBaselist Ibl in colobjinterflistbl)
            {
                if (Ibl.GetName() == "Phaseconfigured")
                {
                    //Find Vdcconfigure to get slice parameters
                    foreach (IBaselist Ibltwo in colobjinterflistbl)
                    {
                        if (Ibltwo.GetName() == "Vdcconfigured")//if it is vdc configured
                        {
                            (Ibltwo as VdcconfiguredI).Setphaseslice((Ibl as PhaseconfiguredI).Listslices);//set first set
                            (Ibltwo as VdcconfiguredI).Setphaseslice((Ibl as PhaseconfiguredI).Listslices2);//set second set
                        }
                    }
                    break;
                }
            }
            ///<summary>
            ///Below loop creates the slices of the column in the CSV set up by Vdcconfigured.
            ///New with interface implementation
            ///</summary>
            try
            {
                //Find Vdcconfigure to get slice parameters
                foreach (IBaselist Ibltwo in colobjinterflistbl)
                {
                    if (Ibltwo.GetName() == "Vdcconfigured")
                    {
                        // (Ibltwo as VdcconfiguredI)
                        foreach (IBaselist Ibl in colobjinterflistbl)
                        {
                            if (!(Ibl.GetName() == "Vdcconfigured"))//if it is not Vdcconfigured
                            {
                                Ibl.Populareslices((Ibltwo as VdcconfiguredI).Slicelist);
                            }
                        }
                    }
                }
            }
            catch (InvalidCastException)
            {

            }
        }
        //************************************Done populating data for baseline*********************
        //************************************Start populating UUT instances with data*************
        /// <summary>
        /// Populate data for the unit under test
        /// </summary>
        /// <param name="filename"></param>
        private void populatedatatestunit(string filename)
        {
            //clear the old values
            if (wantedcolumns.Count > 0)
                wantedcolumns.Clear();
            if (realpowerdict.Count > 0)
                realpowerdict.Clear();
            if (colobjinterflist.Count > 0)
                colobjinterflist.Clear();

            if (!(File.Exists(filename)))
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
            // Read data from CSV file
            using (CsvFileReader reader = new CsvFileReader(filename))
            {
                CsvRow row = new CsvRow();
                rowcount = 0;
                while (reader.ReadRow(row))
                {
                    //if the row count is 0, then headers
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
            //Uses the CSVrowManager to load all the wanted data as they exist in rows
            CSVrowManager = new CSVrowManager();
            CSVrowManager.Load(wantedcolumns);

            //columnobjectlist = new List<Baselist>();
            foreach (var VAR in realpowerdict)
            {
                /// <summary>
                /// Below the instance is created and!!! the parameter is passes to it. I like it. 
                /// VAR.Value.Columnvalues is the value
                /// </summary>
                //columnobjectlist.Add((Baselist)Activator.CreateInstance(Type.GetType("PlotDVT." + VAR.Key), VAR.Value.Columnvalues));
                if(VAR.Key == "Vdcconfigured")
                {
                    colobjinterflist.Add((IBaselist)Activator.CreateInstance
                        (Type.GetType("PlotDVT.VdcconfiguredI"), VAR.Value.Columnvalues, VAR.Key));

                }
                else if (VAR.Key == "Phaseconfigured")
                {
                    colobjinterflist.Add((IBaselist)Activator.CreateInstance
                        (Type.GetType("PlotDVT.PhaseconfiguredI"), VAR.Value.Columnvalues, VAR.Key));
                }
                else
                {
                    colobjinterflist.Add((IBaselist)Activator.CreateInstance(Type.GetType("PlotDVT.ValuelistI"), VAR.Value.Columnvalues, VAR.Key));
                }
            }

            foreach (IBaselist Ibl in colobjinterflist)
            {
                if (Ibl.GetName() == "Phaseconfigured")
                {
                    //Find Vdcconfigure to get slice parameters
                    foreach (IBaselist Ibltwo in colobjinterflist)
                    {
                        if (Ibltwo.GetName() == "Vdcconfigured")
                        {
                            (Ibltwo as VdcconfiguredI).Setphaseslice((Ibl as PhaseconfiguredI).Listslices);
                            (Ibltwo as VdcconfiguredI).Setphaseslice((Ibl as PhaseconfiguredI).Listslices2);
                        }
                    }
                    break;    
                }
            }
            ///<summary>
            ///Below loop creates the slices of the column set up by Vdcconfigured.
            ///New with interface implementation
            ///</summary>
            //slices = Ibl.GetSlices();
            try
            {
                //Find Vdcconfigure to get slice parameters
                foreach (IBaselist Ibltwo in colobjinterflist)
                {
                    if (Ibltwo.GetName() == "Vdcconfigured")
                    {
                       // (Ibltwo as VdcconfiguredI)
                        foreach(IBaselist Ibl in colobjinterflist)
                        {
                            if (!(Ibl.GetName() == "Vdcconfigured"))
                            {
                                Ibl.Populareslices((Ibltwo as VdcconfiguredI).Slicelist);
                            }
                        }
                    }
                }
            }
            catch (InvalidCastException)
            {

            }

        }
        //**************************************Done UUT data populatin****************************
        private void chartdefaults()
        {
            chart1.ChartAreas[0].BackColor = Color.White;
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.ChartAreas[0].AxisX.Minimum = Double.NaN;
            chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
            chart1.ChartAreas[0].AxisX.Maximum = Double.NaN;
            chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
            chart1.ChartAreas[0].AxisX.Title = "";
            chart1.ChartAreas[0].AxisY.Title = "";
            //chart1.ChartAreas.Clear();

            chart2.ChartAreas[0].AxisX.Title = "";
            chart2.ChartAreas[0].AxisY.Title = "";
            chart2.ChartAreas[0].BackColor = Color.White;
            chart2.Series.Clear();
            chart2.Titles.Clear();
            chart2.ChartAreas[0].AxisX.Minimum = Double.NaN;
            chart2.ChartAreas[0].AxisY.Minimum = Double.NaN;
            chart2.ChartAreas[0].AxisX.Maximum = Double.NaN;
            chart2.ChartAreas[0].AxisY.Maximum = Double.NaN;
            //chart2.ChartAreas.Clear();
        }
//**********************************Done populating data for unit undertest*********************
        private void plotIdc()//Changing this plot to deal with interface data
        {
            ///<comments>
            ///Reset the chart default values twith this call
            ///</comments>
            chartdefaults();

            Title title = new Title("Reported Idc UUT vs. baseline. Ph angle: " + textBox1.Text,
             Docking.Top, new Font("Verdana", 8, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;

            Dictionary<float,List<float>> valuestoplot = new Dictionary<float,List<float>>();
            foreach(IBaselist Ibl in colobjinterflist)
            {
                if (Ibl.GetName() == "Idcconfigured")
                {
                    valuestoplot = Ibl.GetSlices();
                    break;
                }
            }
            foreach (var kv in valuestoplot)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "CNF";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart1.Series[chartseries].Color = Color.CadetBlue;
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Star6;
                this.chart1.Series[chartseries].MarkerSize = 13;
                //this.chart1.Series[chartseries].BorderWidth = 8;
                foreach (var v in kv.Value)
                {
                    this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }

            IBaselist ibl = colobjinterflist.First(item => item.GetName() == "Idcpowermeter");
            //plot the power meter on chart1
            valuestoplot = ibl.GetSlices();
            foreach (var kv in valuestoplot)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "UUT1";
                //Add a series
                this.chart1.Series.Add(chartseries);
                //set the chart type
                this.chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                this.chart1.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart1.Series[chartseries].MarkerSize = 7;
                this.chart1.Series[chartseries].Color = Color.Black;
                foreach (var v in kv.Value)
                {
                    if (v < 0.1f)
                    {
                        this.chart1.Series[chartseries].Color = Color.Red;
                    }
                   this.chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }

            // plot the DIFF V pcu on chart 2
            //plot the pcu on chart 1
            ibl = colobjinterflistbl.First(item => item.GetName() == "Idcpcu");
            //plot the power meter on chart1
            valuestoplot = ibl.GetSlices();
            foreach (var kv in valuestoplot)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "BL";
                //Add a series
                this.chart2.Series.Add(chartseries);
                this.chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                this.chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                this.chart2.Series[chartseries].MarkerSize = 7;
                this.chart2.Series[chartseries].Color = Color.Black;
                //    this.chart2.ChartAreas[0].AxisX.Minimum = 13;
                foreach (var v in kv.Value)
                {
                    if (v < 0.1f)
                    {
                        this.chart2.Series[chartseries].Color = Color.Red;
                    }
                    this.chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }

            //plot the pcu current on chart 1
            ibl = colobjinterflist.First(item => item.GetName() == "Idcpcu");
            //plot the power meter on chart1
            valuestoplot = ibl.GetSlices();
            foreach (var kv in valuestoplot)
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "UUT";
                //Add a series
                chart1.Series.Add(chartseries);
                //set the chart type
                chart1.Series[chartseries].ChartType = SeriesChartType.Point;
                chart1.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                chart1.Series[chartseries].MarkerSize = 7;
                chart1.Series[chartseries].Color = Color.DarkOrange;
                // plot the pcu on chart 2
                //Add a series
                chart2.Series.Add(chartseries);
                //set the chart type
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.DarkOrange;
                //Scale the x axis

                foreach (var v in kv.Value)
                {
                    if (v < 0.1f)
                    {
                        chart2.Series[chartseries].Color = Color.Red;
                    }
                    chart1.Series[chartseries].Points.AddXY(kv.Key, v);
                    chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }
        }
        /// <summary>
        /// Diff plotting starts
        /// </summary>
        private void ploteff()//Button1_Efficiency
        {
            chartdefaults();

            Title title = new Title("Efficiency powermeter. The sum of the difference. ",
                Docking.Top, new Font("Verdana", 8, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;
            chart2.ChartAreas[0].AxisX.Title = "Vdc configured";
            chart2.ChartAreas[0].AxisY.Title = "Sum of difference in Eff %.";

            List<float> values = new List<float>();
            /// plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit
            int i = 0;//set the marker style position to 1
            int j = 1;
            foreach (Button btn in phasebuttonlist)
            {

                //set the phase angle here, click the phase button
                btn.PerformClick();
                IBaselist ibl2 = colobjinterflistbl.First(item => item.GetName() == "Efficiency");
                IBaselist ibl = colobjinterflist.First(item => item.GetName() == "Efficiency");

                foreach (var kv in ibl.GetSlices())
                {
                    //Name the series
                    string chartseries = (Convert.ToString(kv.Key));
                    if (chartseries.Length > 4)
                        chartseries += chartseries.Substring(0, 4);
                    chartseries += textBox1.Text;//This will be the phase angle
                    //Add a series
                    //Circle, Cross, Diamond, Square, Triangle, Star4, Star10
                    chart2.Series.Add(chartseries);
                    chart2.Series[chartseries].ChartType = SeriesChartType.Point;

                    //chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                    chart2.Series[chartseries].MarkerSize = 10;
                    chart2.Series[chartseries].IsVisibleInLegend = false;
                    chart2.Series[chartseries].Color = Color.DarkOrange;
                    if (j != i)//only make the marker visible if it is the first iteratiion
                        chart2.Series[chartseries].IsVisibleInLegend = true;
                    j = i;
                    chart2.Series[chartseries].LegendText = textBox1.Text;//Legend text
                    chart2.Series[chartseries].MarkerStyle = phasemarkerlist[i];

                    if (ibl2.GetSlices().ContainsKey(kv.Key))
                        values = ibl2.GetSlices()[kv.Key];

                    if (values.Count <= kv.Value.Count)
                    {
                        float accumefficiency = 0.0f;
                        for (int k = 0; k < values.Count; k++)
                        {
                            float accuracy = kv.Value[k] - values[k];//Powermeter efficiencies subtrackted
                            float accuracy2 = (float)(Math.Round((double)accuracy, 2));
                            accumefficiency += accuracy2;//add all the defferences together
                            //chart2.Series[chartseries].Points.AddXY(kv.Key, accuracy2);
                        }
                        //if there is a bigger that 15 accumelated difference then red cross
                        if (accumefficiency > 15 || accumefficiency < -15)
                        {
                            accumefficiency = 0;
                            chart2.Series[chartseries].Color = Color.Red;
                            chart2.Series[chartseries].MarkerSize = 12;
                        }
                        chart2.Series[chartseries].Points.AddXY(kv.Key, accumefficiency);

                    }
                }
                i = i + 1;//move the marker list on 1
            }
            chart2.ChartAreas[0].BackColor = Color.White;
         }

        private void dcpowerdiff()
        {
            chartdefaults();

            IBaselist ibl = colobjinterflistbl.First(item => item.GetName() == "Wdcpowermeter");
            foreach (var kv in ibl.GetSlices())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "BL";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                //this.chart1.Series[chartseries].Palette = ChartColorPalette.Bright;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 9;
                chart2.Series[chartseries].Color = Color.Black;
                foreach (var v in kv.Value)
                {
                    chart2.Series[chartseries].Points.AddXY(kv.Key, v);
                }
            }

            ibl = colobjinterflist.First(item => item.GetName() == "Wdcpowermeter");
            foreach (var kv in ibl.GetSlices())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "UUT";
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

            IBaselist ibl = colobjinterflistbl.First(item => item.GetName() == "Wacpowermeter");
            foreach (var kv in ibl.GetSlices())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "BL";
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

            ibl = colobjinterflist.First(item => item.GetName() == "Wacpowermeter");
            foreach (var kv in ibl.GetSlices())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "UUT";
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
        private void PlotVdccompare()
        {
            chartdefaults();

            Title title = new Title("Vdc compare: Vdc configured, powermeter and PCU", 
                Docking.Top, new Font("Verdana", 8, FontStyle.Regular), Color.Black);
            this.chart1.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = this.chart1.ChartAreas[0].Name;
            Title title2 = new Title("Vdc reported: Test pm vs Baseline pm",
                Docking.Top, new Font("Verdana", 8, FontStyle.Regular), Color.Black);
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
            IBaselist ibl = colobjinterflist.First(item => item.GetName() == "Vdcpcu");
            foreach (var kv in ibl.GetSlices())
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
            foreach (var kv in ibl.GetSlices())
            {
                //Name the series
                this.chart1.Series[series].Points.AddXY(kv.Key, kv.Key);
            }

            // plot the DIFF V powermeter on chart 2
            ibl = colobjinterflistbl.First(item => item.GetName() == "Vdcpowermeter");
            foreach (var kv in ibl.GetSlices())
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
            ibl = colobjinterflist.First(item => item.GetName() == "Vdcpowermeter");
            foreach (var kv in ibl.GetSlices())
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
            ibl = colobjinterflist.First(item => item.GetName() == "Vdcpcu");
            foreach (var kv in ibl.GetSlices())
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
        private void Richtextedit()
        {
           // float maxidc = plotidcpowermeter.GetMax;
            //string value = Convert.ToString(maxidc);
            this.richTextBox1.AppendText("Idc max (powermeter): ");
           // this.richTextBox1.AppendText(value + "A\n");
           // value = Convert.ToString(plotidcpowermeter.GetMin);
            this.richTextBox1.AppendText("Idc min (powermeter): ");
           // this.richTextBox1.AppendText(value + "A\n");
        }

        private void Richtextedit(string text)
        {
            this.richTextBox1.AppendText(text + "\n");
        }

        private Dictionary<string, Column> Realpowerdictionary
        {
            get { return realpowerdict; }
        }
        //Load DC voltage compare tables
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
        //            private List<IBaselist> colobjinterflist;//This is new...Interface implementation
        //private List<IBaselist> colobjinterflistbl;//This is new...Interface implementation
            foreach(IBaselist IBase in colobjinterflist)
            {
                if(IBase is VdcconfiguredI)
                {
                    foreach (IBaselist IBase2 in colobjinterflist)
                        if (!(IBase2.GetName() == "Vdcconfigured") || !(IBase2.GetName() == "Phaseconfigured"))
                            IBase2.Populareslices((IBase as VdcconfiguredI).Slicelist, deg);
                }
            }

            foreach (IBaselist IBase3 in colobjinterflistbl)
            {
                if (IBase3 is VdcconfiguredI)
                {
                    foreach (IBaselist IBase4 in colobjinterflistbl)
                        if (!(IBase4.GetName() == "Vdcconfigured") || !(IBase4.GetName() == "Phaseconfigured"))
                            IBase4.Populareslices((IBase3 as VdcconfiguredI).Slicelist, deg);
                }
            }
        }
        /// <summary>
        /// Done the IBaselist implementation. Plot Dcv accuracy
        /// </summary>
        public void Accuracydcv()
        {
            chartdefaults();

            Title title = new Title("PCU Vdc reporting accurancy. Vdc pcu - Vdc powermeter. Ph angle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;
            List<float> values = new List<float>();
            /// plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit 
            IBaselist baselinepowermeter = colobjinterflistbl.First(item => item.GetName() == "Vdcpowermeter");
            IBaselist baselinepcu = colobjinterflistbl.First(item => item.GetName() == "Vdcpcu");

            IBaselist uutpowermeter = colobjinterflist.First(item => item.GetName() == "Vdcpowermeter");
            IBaselist uutpcu = colobjinterflist.First(item => item.GetName() == "Vdcpcu");

            plotaccuracycomaprison(baselinepcu, baselinepowermeter, uutpcu, uutpowermeter);
        }
        //plot the accuracy of idc
        public void Accuracyicv()
        {
            chartdefaults();

            Title title = new Title("PCU Idc reporting accurancy. Idcpcu - Idc powermeter. Phase andgle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;
            List<float> values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit 
            IBaselist baselinepowermeter = colobjinterflistbl.First(item => item.GetName() == "Idcpowermeter");
            IBaselist baselinepcu = colobjinterflistbl.First(item => item.GetName() == "Idcpcu");
            // plot the DIFF V powermeter on chart 2
            IBaselist uutpowermeter = colobjinterflist.First(item => item.GetName() == "Idcpowermeter");
            IBaselist uutpcu = colobjinterflist.First(item => item.GetName() == "Idcpcu");

            plotaccuracycomaprison(baselinepcu, baselinepowermeter, uutpcu, uutpowermeter);
        }

        public void Accuracywdc()
        {
            chartdefaults();
            Title title = new Title("PCU Wdc reporting accurancy. Phase andgle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;
            //Dictionary<float, List<float>> dictwdcpm =
            //    new Dictionary<float, List<float>>(plotwdcpowermeterbl.GetSlices);
            List<float> values = new List<float>();
            // plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit 
            IBaselist baselinepowermeter = colobjinterflistbl.First(item => item.GetName() == "Wdcpowermeter");
            IBaselist baselinepcu = colobjinterflistbl.First(item => item.GetName() == "Wdcpcu");
            // plot the DIFF V powermeter on chart 2
            IBaselist uutpowermeter = colobjinterflist.First(item => item.GetName() == "Wdcpowermeter");
            IBaselist uutpcu = colobjinterflist.First(item => item.GetName() == "Wdcpcu");

            plotaccuracycomaprison(baselinepcu, baselinepowermeter, uutpcu, uutpowermeter);
        }
        /// <summary>
        /// Plots the Var vs real power triangle
        /// </summary>
        private async void acvarvapower()
        {
            //List<YourType> newList = new List<YourType>(oldList);
            chartdefaults();
            Title title = new Title("Powermeter VA against VAR: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart1.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart1.ChartAreas[0].Name;
            ///Get reference to all the column classes, to acces their data
            IBaselist ibl = colobjinterflistbl.First(item => item.GetName() == "Wacpowermeter");
            IBaselist ibl2 = colobjinterflistbl.First(item => item.GetName() == "Wacconfigured");
            IBaselist ibl3 = colobjinterflistbl.First(item => item.GetName() == "Wacvarconfigured");
            IBaselist ibl4 = colobjinterflistbl.First(item => item.GetName() == "ACvarpowermeter");
            IBaselist ibl5 = colobjinterflist.First(item => item.GetName() == "Wacpowermeter");
            IBaselist ibl6 = colobjinterflist.First(item => item.GetName() == "Wacconfigured");
            IBaselist ibl7 = colobjinterflist.First(item => item.GetName() == "Wacvarconfigured");
            IBaselist ibl8 = colobjinterflist.First(item => item.GetName() == "ACvarpowermeter");

            //Below are the values for chart one
            List<float> values = new List<float>();
            List<float> waccnf = new List<float>();
            List<float> wacvarcnf = new List<float>();
            //Chart2 this is for chart 2 the baseline vlues
            List<float> values2 = new List<float>();
            List<float> values3 = new List<float>();
            //congfigured values for chart 2 baseline unit
            List<float> waccnfbl = new List<float>();
            List<float> wacvarcnfbl = new List<float>();
 
            foreach (var kv in ibl8.GetSlices())
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

                if (ibl5.GetSlices().ContainsKey(kv.Key))
                    values = ibl5.GetSlices()[kv.Key];
                if (ibl6.GetSlices().ContainsKey(kv.Key))
                    waccnf = ibl6.GetSlices()[kv.Key];
                if (ibl7.GetSlices().ContainsKey(kv.Key))
                    wacvarcnf = ibl7.GetSlices()[kv.Key];

                //chart2: this is for the chart 2 comparison betwwen baseline and real unit
                chart2.Series.Add(serieschart2);
                chart2.Series[serieschart2].ChartType = SeriesChartType.Line;
                chart2.Series[serieschart2].BorderWidth = 2;

                chart2.Series.Add(serieschart2cnf);
                chart2.Series[serieschart2cnf].ChartType = SeriesChartType.Point;
                chart2.Series[serieschart2cnf].MarkerStyle = MarkerStyle.Cross;
                chart2.Series[serieschart2cnf].MarkerSize = 10;

                if (ibl.GetSlices().ContainsKey(kv.Key))
                    values2 = ibl.GetSlices()[kv.Key];
                if (ibl4.GetSlices().ContainsKey(kv.Key))
                    values3 = ibl4.GetSlices()[kv.Key];
                if (ibl2.GetSlices().ContainsKey(kv.Key))
                    waccnfbl = ibl2.GetSlices()[kv.Key];
                if (ibl3.GetSlices().ContainsKey(kv.Key))
                    wacvarcnfbl = ibl3.GetSlices()[kv.Key];
                

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
            return;
        }
//********************************************accuracy plots start here*****************************************
        /// <summary>
        /// Done the IBaselist implementation. Plot ACwatt accuracy
        /// </summary>
        public void Accuracyacwatt()
        {
            chartdefaults();

            Title title = new Title("PCU Wac reporting accurancy. Wac pcu - Wac powermeter. Ph angle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;

            /// plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit 
            IBaselist baselinepowermeter = colobjinterflistbl.First(item => item.GetName() == "Wacpowermeter");//done
            IBaselist baselinepcu = colobjinterflistbl.First(item => item.GetName() == "Wacpcu");
            //get the values to plot for the non basline unit 
            IBaselist uutpowermeter = colobjinterflist.First(item => item.GetName() == "Wacpowermeter");
            IBaselist uutpcu = colobjinterflist.First(item => item.GetName() == "Wacpcu");

            plotaccuracycomaprison(baselinepcu, baselinepowermeter, uutpcu, uutpowermeter);
        }
        /// <summary>
        /// Done the IBaselist implementation. Plot ACvar accuracy
        /// </summary>
        public void Accuracyvar()
        {
            chartdefaults();

            Title title = new Title("PCU VAR reporting accurancy. VAR pcu - powermeter. Ph angle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;
            /// plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit 
            IBaselist baselinepowermeter = colobjinterflistbl.First(item => item.GetName() == "ACvarpowermeter");//done
            IBaselist baselinepcu = colobjinterflistbl.First(item => item.GetName() == "Wacimagpcu");

            IBaselist uutpowermeter = colobjinterflist.First(item => item.GetName() == "ACvarpowermeter");
            IBaselist uutpcu = colobjinterflist.First(item => item.GetName() == "Wacimagpcu");

            ///call the plot function to plot the data
            plotaccuracycomaprison(baselinepcu, baselinepowermeter, uutpcu, uutpowermeter);
        }
        /// <summary>
        /// AC voltage accuracy
        /// </summary>
        //Plots the accuracy of the inverter AC voltage measurement vs the powermeter voltage measurement
        public void Accuracyvac()
        {
            chartdefaults();
            Title title = new Title("PCU Vac reporting accurancy. Phase andgle: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart2.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart2.ChartAreas[0].Name;
            // plot the DIFF V powermeter on chart 2
            ///get the columns to print for the baseline unit 
            IBaselist baselinepowermeter = colobjinterflistbl.First(item => item.GetName() == "Vacpowermeter");//Find
            IBaselist baselinepcu = colobjinterflistbl.First(item => item.GetName() == "Vacpcu");//Find
            //UUT values
            IBaselist uutpowermeter = colobjinterflist.First(item => item.GetName() == "Vacpowermeter");
            IBaselist uutpcu = colobjinterflist.First(item => item.GetName() == "Vacpcu");
            ///call the plot function to plot the data
            plotaccuracycomaprison(baselinepcu, baselinepowermeter, uutpcu, uutpowermeter);
        }
        /// <summary>
        /// These are the values for the baseline test unit and the uut (unit under test)
        /// Takes two basleline unit vales and two unit undertest values, subtract the basline values
        /// from each other. Subtract the unit under test values from each other. plot both sets to compare them
        /// </summary>
        /// <param name="baseline1"></param>
        /// <param name="baseline2"></param>
        /// <param name="uut1"></param>
        /// <param name="uut2"></param>
        private void plotaccuracycomaprison(IBaselist baseline1, IBaselist baseline2, IBaselist uut1, IBaselist uut2)
        {
            List<float> values = new List<float>();

            foreach (var kv in baseline1.GetSlices())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "BL";//Baseline
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.Black;

                if (baseline2.GetSlices().ContainsKey(kv.Key))
                    values = baseline2.GetSlices()[kv.Key];

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
            ///second half of plotting
            ///create a new list
            values = new List<float>();
            foreach (var kv in baseline1.GetSlices2())//kv is a dictionary
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "BL2";//Baseline second set
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Circle;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.Black;
                chart2.Series[chartseries].IsVisibleInLegend = false;//Hide this series from legend

                if (baseline2.GetSlices2().ContainsKey(kv.Key))
                    values = baseline2.GetSlices2()[kv.Key];//values list now point to list contained in dictionary

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
            ///*****************************************************END BASE line LIST values*******************************
            //**********************************************************UUT starts***********************
            values = new List<float>();
            foreach (var kv in uut1.GetSlices())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "UUT";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.DarkOrange;

                if (uut2.GetSlices().ContainsKey(kv.Key))
                    values = uut2.GetSlices()[kv.Key];

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
            //values = new List<float>();
            foreach (var kv in uut1.GetSlices2())
            {
                //Name the series
                string chartseries = (Convert.ToString(kv.Key));
                if (chartseries.Length > 4)
                    chartseries = chartseries.Substring(0, 4);
                chartseries += "UUT2";
                //Add a series
                chart2.Series.Add(chartseries);
                chart2.Series[chartseries].ChartType = SeriesChartType.Point;
                chart2.Series[chartseries].MarkerStyle = MarkerStyle.Cross;
                chart2.Series[chartseries].MarkerSize = 7;
                chart2.Series[chartseries].Color = Color.DarkOrange;
                chart2.Series[chartseries].IsVisibleInLegend = false;

                if (uut2.GetSlices().ContainsKey(kv.Key))
                    values = uut2.GetSlices2()[kv.Key];

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
        private List<Column> wantedcolumns;//stores a list of wanted data rows
        private int rowcount;
        private bool reverse = false;
        private DataCol datacolumns;
        private Dictionary<string, Column> realpowerdict;
        //RealPowerAnswer realpoweranswers;
        private List<IBaselist> colobjinterflist;//Interface implementation
        private List<IBaselist> colobjinterflistbl;//Interface implementation
        /// <summary>
        /// Baseline variables below to be used to create the diff
        /// </summary>
        private DataCol datacolumnsbl;
        private Dictionary<string, Column> realpowerdictbl;
        private List<Column> wantedcolumnsbl;
        private CSVrowManager CSVrowManager;
        private string filenamebl;
        private string filenamedata;
        //use this to initialize lists
        private Button[] phasebuttonarray;
        private List<Button> phasebuttonlist;
        //marker styles to match the phase button lists
        private MarkerStyle[] phasemarkerstyles;
        private List<MarkerStyle> phasemarkerlist;

        private string uutdetail = "";
        private string baselinedetail = "";
        private string difftooldir = "";
        //list of bugs detected in csv, goes into report
        private Bugs Wacpowerbuglist;
        private Bugs Vdcvbuglist;
        private Bugs Idcvbuglist;
        private Bugs Vacbuglist;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
    }
}
