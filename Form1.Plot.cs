using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PlotDVT
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// This function gets called by button press
        /// Plots the Var vs real power triangle
        /// This plot is a special plot. It plot the ractive power
        /// triangles and writest to the richtextbox.
        /// </summary>
        private async void async_col_sequenceplot()
        {
            //Clear all the data points
            if (chart3.Series != null)
            {
                foreach (Series s in chart3.Series)
                {
                    clearseries(s);
                }
            }
            //Clear all the data points
            if (chart4.Series != null)
            {
                foreach (Series s in chart4.Series)
                {
                    clearseries(s);
                }
            }

            //enter a dalay for user feedback to show chart is updated
            await Task.Delay(100);
            //chart3.Titles.Clear();
            //AC plots Below are the values for chart one, Wacpm also used for filter
            List<float> Wacpm = getfloatlist("Wacpowermeter");
            //Filter mask, Get a masked value, this will be mask2!!!!!!
            List<float> maskVdccnf = getfloatlist("Vdcconfigured");
            //List<float> Wacpm = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacpowermeter").GetFloats());
            List<float> Wvarpm = getfloatlist("ACvarpowermeter");
            List<float> Waccnf = getfloatlist("Wacconfigured");
            List<float> Wvarcnf = getfloatlist("Wacvarconfigured");
            //never plot these just use them to determine accuracy
            List<float> WACpcu = getfloatlist("Wacpcu");
            List<float> WACimagpcu = getfloatlist("Wacimagpcu");
            //Get a bool fail list to do selective plotting
            List<bool> accuracyfaillist = new List<bool>
                (Calculate.Faillist(WACpcu, Wacpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistvar = new List<bool>
                (Calculate.Faillist(WACimagpcu, Wvarpm, 1, float.Parse(textBox9.Text), 1));
            //this is independent of plot
            List<string> valuesforreport = new List<string>(){"Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured",
                "Phaseconfigured", "Temperature", "Vacpowermeter" };
            //clear value everytime button calls this function
            List<string> listofseries = new List<string>() { "Wacpowermeter", "Wacconfigured" };
            // Create a list of values to extract, alias names from xml
            //**************************Setup chart3***************************************************
            //chart3.Series.Clear();
            //AC plots Set up axis variables
            setup_chart3axes(chart3);
            addlist_series_tochart(chart3, listofseries);
            Series Waccnfseries = chart3.Series["Wacconfigured"];
            Series Wacpowermeter = chart3.Series["Wacpowermeter"];
            //**************************Setup chart4*************************************************** 
            setup_chart4axes(chart4, Wvarpm.Count);
            addlist_series_tochart(chart4, new List<string>() { "Wacpowermeter", "Accumulate" });
            setupseriesline(chart4.Series["Wacpowermeter"]);//setup the color and the line type plot
            setupseriescross(chart4.Series["Accumulate"]);//setup the color and the line type plot
            chart4.Series["Wacpowermeter"].Points.AddXY(0, 0);//Add the first dot to the line
            //******************************End chart 4 setup***********************************************
            //var and watt
            setupseriesline(Wacpowermeter);
            setupseriescross(Waccnfseries);
            //**END configure series**
            //Inititalise classes for different bugs, the int is the chartarea number
            //Bugs class contains a list of text bugrows
            Wacpowerbuglist = new Bugs(0);
            Vdcvbuglist = new Bugs(1);
            Idcvbuglist = new Bugs(2);
            Vacbuglist = new Bugs(3);
            List<Bugs> Bugslist = new List<Bugs>() { Wacpowerbuglist};
            //loop trough a series to cover every value
            for (int i = 0; i < Wacpm.Count; i++)
            {
                //This will have to stay a special plot
                if(accuracyfaillist[i] == false || accuracyfaillistvar[i] == false)
                {
                    //ignore the values that is below this value in textbox10 and above textBox13
                    if (Wacpm[i] > float.Parse(textBox10.Text) && Wacpm[i] < float.Parse(textBox13.Text))
                    {
                        //Filter according to Vcnf
                        if (maskVdccnf[i] > float.Parse(textBox5.Text) && maskVdccnf[i] < float.Parse(textBox6.Text))
                        {
                            Wacpowerbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                            //Plot the congfigured w/va powermeter values
                            Wacpowermeter.Points.AddXY(0, 0);
                            Wacpowermeter.Points.AddXY(Wacpm[i], 0);
                            Wacpowermeter.Points.AddXY(Wacpm[i], Wvarpm[i]);
                            Wacpowermeter.Points.AddXY(0, 0);
                            ////Plot the congfigured w/va configured values
                            Waccnfseries.Points.AddXY(Waccnf[i], 0);
                            Waccnfseries.Points.AddXY(Waccnf[i], Wvarcnf[i]);
                            //**************************CHART4*************************
                            //Add bug plot to the line plot
                            chart4.Series["Wacpowermeter"].Points.AddXY(i, 0);
                            chart4.Series["Wacpowermeter"].Points.AddXY(i, 3);
                            chart4.Series["Wacpowermeter"].Points.AddXY(i, 0);
                            //**********************Accumulate bugs to Chart 4*********
                            //if the series is empty then add something
                            if (chart4.Series["Accumulate"].Points.Count == 0)
                            {
                                chart4.Series["Accumulate"].Points.AddXY(0, 0);
                            }
                            DataPoint datapoint = chart4.Series["Accumulate"].Points.FindByValue(i, "X", 0);//o is start index
                            if (datapoint == null)
                            {
                                //chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                chart4.Series["Accumulate"].Points.AddXY(i, 1);
                                //chart4.Series["Accumulate"].Points.AddXY(i, 0);
                            }
                            else
                            {
                                datapoint.SetValueY(datapoint.YValues.Last() + 1);
                            }
                        }
                    }
                 }
            }
            //clear the richtext box
            richTextBox1.Clear();
            //hunt bugs kicks of a series of bug plots with Wacpm as the filter list
            huntbugs(Wacpm, maskVdccnf);
            //Writes to richtext box, this needs to be function
            printbugs(Bugslist);
        }
        /// <summary>
        /// print the bugs to the richtext box
        /// the richtextbox also has to be cleared everytim after a
        /// series of bug plots
        /// </summary>
        private void printbugs(List<Bugs> Bugslist)
        {
            richTextBox1.AppendText("*****************************" + "ACWreporting" + "**********************************" + "\r\n");
            List<string> valuesforreport = new List<string>(){"Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured",
                "Phaseconfigured", "Temperature", "Vacpowermeter" };
            foreach (Bugs B in Bugslist)
            {
                if (B.Bugrows.Count != 0)
                {
                    foreach (CSVrow row in B.Bugrows)
                        richTextBox1.AppendText(row.Humantext(valuesforreport) + "\r\n");
                    clear_background_image(chart3, B.Getchartno());
                }
                else
                    set_background_image(chart3, B.Getchartno());
            }
        }

        /// <summary>
        /// This is a alternative plotting path for bugs and ultimately the path that should be used
        /// takes the filter mask, this is the list of float values to filter by so if it is
        /// bigger or small than this value it will not be plotted
        /// the filter values is checked again values textbox values and if it is smaller or larger
        /// then the other value is not plotted. to filter again say output power values
        /// </summary>
        private async void huntbugs(List<float> the_filter_mask, List<float> the_filter_mask2)
        {
            //The filter mask in these are the List of parameter that will be searched in addition 
            //to the plotted values, and it will not plot the values if it does not meet the 
            //mask requirements
            //Vac
            plot_general("Vacpcu", "Vacpowermeter", "Vacpcu", "ChartArea4", (float)numericUpDown5.Value, the_filter_mask, the_filter_mask2);
            //Idc
            plot_general("Idcpcu", "Idcpowermeter", "Idcconfigured", "ChartArea3", (float)numericUpDown3.Value, the_filter_mask, the_filter_mask2);
            //Wdc can use the generic plot
            plot_general("Wdcpcu", "Wdcpowermeter", "Wdcconfigured", "ChartArea6", (float)numericUpDown4.Value, the_filter_mask, the_filter_mask2);
            //DC plots
            plot_general("Vdcpcu", "Vdcpowermeter", "Vdcconfigured", "ChartArea2", (float)numericUpDown2.Value, the_filter_mask, the_filter_mask2);
            //special plot for apparant current pcu ac current rerting
            plot_apparant_current("Iacpcu", "Iacpowermeter", "Iacimagpcu", "ChartArea5", (float)numericUpDown6.Value, the_filter_mask, the_filter_mask2);
        }
        /// <summary>
        /// Atthis stage it plots and writes to richtext. Should change it.
        /// </summary>
        /// <param name="pcu">value from pcu</param>
        /// <param name="powermeter">value from power meter</param>
        /// <param name="cnf">configured value by the test</param>
        /// <param name="chartarea">The chart area name where data should go</param>
        /// <param name="FS">the fulscale value used for accuracy</param>
        /// <param name="filter_mask">the mask to filter plotted values</param>
        private void plot_general(string pcu, string powermeter, string cnf, string chartarea, float persent_fail_margin, List<float> filter_mask,
            List<float> maskVdccnf)
        {
            List<string> valuesforreport = new List<string>(){"Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured",
                "Phaseconfigured", "Temperature", "Vacpowermeter" };
            //clear series and add the series
            //chart3.Series.Clear();
            //Get the column float values: (cnf is configured values)
            List<float> CNF = getfloatlist(cnf);
            List<float> PM = getfloatlist(powermeter);
            List<float> PCU = getfloatlist(pcu);
            //Get the bool fail list for specific margins (The ones are percentage 1)
            //convert double from the numericUpDown to float
            float failpersentage = persent_fail_margin;
            List<bool> accuracyfaillist = new List<bool>(Calculate.Pesentage_faillist(PCU, PM, failpersentage));
            //**************************Setup chart4********************************************************
           // setup_chart4axes(chart4);
            addlist_series_tochart(chart4, new List<string>() { pcu });
            setupseriesline(chart4.Series[pcu]);//setup the color and the line type plot
            chart4.Series[pcu].Points.AddXY(0, 0);//Add the first dot to the line
            //******************************End chart 4 setup***********************************************
            //*****************************Setup chart 3****************************************************
            addlist_series_tochart(chart3, new List<string> { pcu, powermeter, cnf });
            //List<bool> accuracyfaillist = new List<bool>(Calculate.Faillist(PCU, PM, 1, FS, 1));
            chart3.Series[powermeter].ChartArea = chartarea;
            chart3.Series[cnf].ChartArea = chartarea;
            setupseriescross(chart3.Series[cnf]);
            setupseriescircle(chart3.Series[powermeter]);
            //****************************End setup chart 3*************************************************
            //Add a litte heading to the richtextbob
            richTextBox1.AppendText( "*****************************" + pcu + "**********************************" + "\r\n");
            for (int i = 0; i < PM.Count; i++)
            {
                //ignore the values that is below this value in textbox10 and above textBox13
                if (filter_mask[i] > float.Parse(textBox10.Text) && filter_mask[i] < float.Parse(textBox13.Text))
                {
                    //Filter according to Vcnf
                    if (maskVdccnf[i] > float.Parse(textBox5.Text) && maskVdccnf[i] < float.Parse(textBox6.Text))
                    { 
                        if (accuracyfaillist[i] == false)
                        {
                            if (CSVrowManager.GetaCSVrow(i) != null)
                            {
                                //Append text to the richtext box
                                richTextBox1.AppendText(CSVrowManager.GetaCSVrow(i).Humantext(valuesforreport) + "\r\n");
                                //Add point to the chart
                                chart3.Series[powermeter].Points.AddY(PM[i]);
                                chart3.Series[cnf].Points.AddY(CNF[i]);
                                //**************************CHART4*************************
                                //Add bug plot to the line plot
                                chart4.Series[pcu].Points.AddXY(i, 0);
                                chart4.Series[pcu].Points.AddXY(i, 3);
                                chart4.Series[pcu].Points.AddXY(i, 0);
                                //**********************Accumulate bugs to Chart 4****************
                                //if the series is empty then add something
                                if (chart4.Series["Accumulate"].Points.Count == 0)
                                {
                                    chart4.Series["Accumulate"].Points.AddXY(0, 0);
                                }
                                //If the series is empty then add something
                                DataPoint datapoint = chart4.Series["Accumulate"].Points.FindByValue(i, "X", 0);//o is start index
                                if (datapoint == null)
                                {
                                    //chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                    chart4.Series["Accumulate"].Points.AddXY(i, 1);
                                    //chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                }
                                else
                                {
                                    datapoint.SetValueY(datapoint.YValues.Last() + 1);
                                    //chart4.Series["Accumulate"].Points.AddXY(i, datapoint.YValues.Last() + 1);
                                    ////chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                    //double last = chart4.Series["Accumulate"].Points[i].YValues.Last();
                                    //last += 1;
                                    //chart4.Series["Accumulate"].Points.AddXY(i, last);
                                    ////chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                }
                            }
                        }
                    }
                }
            }
            setbackground(chart3.Series[powermeter], chartarea);
        }
        /// <summary>
        /// calculate the aparant current, Iacs current measured by the pcu components 
        /// </summary>
        /// <param name="pcureal"></param>
        /// <param name="powermeter"></param>
        /// <param name="reactive"></param>
        /// <param name="chartarea"></param>
        /// <param name="FS"></param>
        private void plot_apparant_current(string pcureal, string powermeter, string reactive, string chartarea, float persent_fail_margin,
            List<float> filter_mask, List<float> maskVdccnf)
        {
            //clear series and add the series
            //chart3.Series.Clear();

            //get the powermeter float values
            List<float> PM = getfloatlist(powermeter);
            //Calculate tthe aparant ac current list from the pcu
            List<float> appartcurrent = new List<float>
                (Calculate.Get_pcu_apparantcurrent(getfloatlist(reactive), getfloatlist(pcureal)));
            //**************************Setup chart4********************************************************
          //  setup_chart4axes(chart4);
            addlist_series_tochart(chart4, new List<string>() { pcureal, "Accumulate" });
            setupseriesline(chart4.Series[pcureal]);//setup the color and the line type plot
            chart4.Series[pcureal].Points.AddXY(0, 0);//Add the first dot to the line
            //******************************End chart 4 setup***********************************************
            //**************************Setup chart3********************************************************
            //setup series names to be added to chart3
            List<string> series = new List<string> { powermeter, "pcuIacs" };
            addlist_series_tochart(chart3, new List<string> { powermeter, "pcuIacs" });
            //Get the bool fail list for specific margins (The ones are percentage 1) 
            // List<bool> accuracyfaillist = new List<bool>(Calculate.Faillist(appartcurrent, PM, 1, FS, 1));
            List<bool> accuracyfaillist = new List<bool>(Calculate.Pesentage_faillist(appartcurrent, PM, persent_fail_margin));
            chart3.Series[powermeter].ChartArea = chartarea;
            chart3.Series["pcuIacs"].ChartArea = chartarea;
            setupseriescross(chart3.Series["pcuIacs"]);
            setupseriescircle(chart3.Series[powermeter]);
            for (int i = 0; i < PM.Count; i++)
            {
                //ignore the values that is below this value in textbox10 and above textBox13
                if (filter_mask[i] > float.Parse(textBox10.Text) && filter_mask[i] < float.Parse(textBox13.Text))
                {
                    //Filter according to Vcnf
                    if (maskVdccnf[i] > float.Parse(textBox5.Text) && maskVdccnf[i] < float.Parse(textBox6.Text))
                    {
                        if (accuracyfaillist[i] == false)
                        {
                            //Vdcvbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                            chart3.Series[powermeter].Points.AddY(PM[i]);
                            chart3.Series["pcuIacs"].Points.AddY(appartcurrent[i]);
                            //**************************CHART4 load point*************************
                            //Add bug plot to the line plot
                            chart4.Series[pcureal].Points.AddXY(i, 0);
                            chart4.Series[pcureal].Points.AddXY(i, 3);
                            chart4.Series[pcureal].Points.AddXY(i, 0);
                            //**********************Accumulate bugs to Chart 4****************
                            //if the series is empty then add something
                            if (chart4.Series["Accumulate"].Points.Count == 0)
                            {
                                chart4.Series["Accumulate"].Points.AddXY(0, 0);
                            }
                            //If the series is empty then add something
                            DataPoint datapoint = chart4.Series["Accumulate"].Points.FindByValue(i, "X", 0);//o is start index
                            if (datapoint == null)
                            {
                                //chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                chart4.Series["Accumulate"].Points.AddXY(i, 1);
                                //chart4.Series["Accumulate"].Points.AddXY(i, 0);
                            }
                            else
                            {
                                datapoint.SetValueY(datapoint.YValues.Last() + 1);
                                ////chart4.Series["Accumulate"].Points.AddXY(i, 0);
                                //double last = chart4.Series["Accumulate"].Points[i].YValues.Last();
                                //last += 1;
                                //chart4.Series["Accumulate"].Points.AddXY(i, last);
                                ////chart4.Series["Accumulate"].Points.AddXY(i, 0);
                            }
                        }
                    }
                }
            }
            setbackground(chart3.Series[powermeter], chartarea);
        }
        /// <summary>
        /// add a list of serise names to a chart
        /// </summary>
        /// <param name="x">Chart</param>
        /// <param name="serieslist">list of series names - strings</param>
        private void addlist_series_tochart(Chart x, List<string> serieslist)
        {
            foreach(string s in serieslist)
            {
                //check if the series name already exists
                if(x.Series.IndexOf(s) == -1)
                    x.Series.Add(s);
            }
        }
        /// <summary>
        /// Get the float values from colobjinterflist. return the full column values with out slices
        /// </summary>
        /// <returns></returns>
        private List<float> getfloatlist(string aliascolumnname)
        {
            return colobjinterflist.First(item => item.GetName() == aliascolumnname).GetFloats();
        }
        //setup series plot variables
        private void setupseriescross(Series x)
        {
            x.ChartType = SeriesChartType.Point;
            x.MarkerStyle = MarkerStyle.Cross;
            x.MarkerColor = Color.Red;
            x.MarkerSize = 8;
            x.IsVisibleInLegend = false;
        }
        //setup series plot variables
        private void setupseriescircle(Series x)
        {
            x.ChartType = SeriesChartType.Point;
            x.MarkerStyle = MarkerStyle.Circle;
            x.MarkerColor = Color.Blue;
            x.MarkerSize = 8;
            x.IsVisibleInLegend = false;
        }
        //setup series plot variables
        private void setupseriesline(Series x)
        {
            x.ChartType = SeriesChartType.Line;
            x.Color = Color.Blue;
            x.MarkerColor = Color.Blue;
            x.BorderWidth = 3;
            x.IsVisibleInLegend = false;
        }

        private void clearseries(Series x)
        {
            if((x != null))
            {
                x.Points.Clear();
            }
        }
        /// <summary>
        /// check the chart series for points and set background accordingly
        /// </summary>
        private void setbackground(Series x_s, string chartarea_name)
        {
            int index = chart3.ChartAreas.IndexOf(chartarea_name);
            if (x_s.Points.Count > 0)
                clear_background_image(chart3, index);
            else
                set_background_image(chart3, index);
        }
        /// <summary>
        /// change the background image to a thumbs up all good, scled to chartimage size
        /// </summary>
        private void set_background_image(Chart x, int area_no)
        {
            x.ChartAreas[area_no].BackImage = "Content/thumbsup.jpg";
            x.ChartAreas[area_no].BackImageWrapMode = ChartImageWrapMode.Scaled;
        }
        /// <summary>
        /// Chagne the background image to neautral color
        /// </summary>
        private void clear_background_image(Chart x, int area_no)
        {
            x.ChartAreas[area_no].BackImage = "Content/cream.jpg";
        }

        private void setup_chart3axes(Chart x)
        {
            //AC plots Set up axis variables
            x.ChartAreas[0].AxisX.Minimum = 0;
            x.ChartAreas[0].AxisX.Maximum = 300;
            x.ChartAreas[0].AxisX.Interval = 50;
            x.ChartAreas[0].AxisY.Minimum = -280;
            x.ChartAreas[0].AxisY.Maximum = 280;
            x.ChartAreas[0].AxisY.Interval = 40;
            //DC volt plots chartarea2
            x.ChartAreas[1].AxisY.Minimum = 0;
            x.ChartAreas[1].AxisY.Maximum = 50;
            x.ChartAreas[1].AxisY.Interval = 5;
            //DC current plots chartarea3
            x.ChartAreas[2].AxisY.Minimum = -3;
            x.ChartAreas[2].AxisY.Maximum = 15;
            x.ChartAreas[2].AxisY.Interval = 1;
            //AC volts plots chartarea4
            x.ChartAreas[3].AxisY.Minimum = -3;
            x.ChartAreas[3].AxisY.Maximum = 302;
            x.ChartAreas[3].AxisY.Interval = 40;
        }

        private void setup_chart4axes(Chart x, int xaxeslegth)
        {
            x.ChartAreas[0].AxisX.Minimum = 0;
            //set the x axes lenth to custom length depending on data
           // x.ChartAreas[0].AxisX.Enabled = AxisEnabled.True; 
            x.ChartAreas[0].AxisX.Maximum = xaxeslegth + 5;
            x.ChartAreas[0].AxisX.Interval = 1;
            x.ChartAreas[0].AxisY.Minimum = 0;
            x.ChartAreas[0].AxisY.Maximum = 8;
            x.ChartAreas[0].AxisY.Interval = 1;
            x.ChartAreas[0].InnerPlotPosition.Height = 100;
            x.ChartAreas[0].InnerPlotPosition.Width = 100;
        }
    }
}
