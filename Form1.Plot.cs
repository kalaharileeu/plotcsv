﻿using System.Collections.Generic;
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
        /// Plots the Var vs real power triangle
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
            //enter a dalay for user feedback to show chart is updated
            await Task.Delay(100);
            //clear value everytime button calls this function
            chart3.Series.Clear();
            //chart3.Titles.Clear();
            //AC plots Set up axis variables
            setup_chart3axes(chart3);
            //AC plots Below are the values for chart one
            List<float> Wacpm = getfloatlist("Wacpowermeter");
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
            List<string> listofseries = new List<string>()
            { "Wacpowermeter", "Wacconfigured" };
            // Create a list of values to extract, alias names from xml
            //this is independent of plot
            List<string> valuesforreport = new List<string>()
            {"Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured",
                "Phaseconfigured", "Temperature", "Vacpowermeter" };
            addlist_series_tochart(chart3, listofseries);
            Series Waccnfseries = chart3.Series["Wacconfigured"];
            Series Wacpowermeter = chart3.Series["Wacpowermeter"];
            //var and watt
            setupseriesline(Wacpowermeter);
            setupseriescross(Waccnfseries);
            //**END configure series**
            //Inititalise lists for different bugs
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
                    //ignore the values that is below this value in textbox10
                    if (Wacpm[i] > float.Parse(textBox10.Text))
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
                    }
                 }
            }
            huntbugs();
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
        /// </summary>
        private async void huntbugs()
        {
            //Vac
            plot_general("Vacpcu", "Vacpowermeter", "Vacpcu", "ChartArea4", float.Parse(textBox7.Text));
            //Idc
            plot_general("Idcpcu", "Idcpowermeter", "Idcconfigured", "ChartArea3", float.Parse(textBox6.Text));
            //Wdc can use the generic plot
            plot_general("Wdcpcu", "Wdcpowermeter", "Wdcconfigured", "ChartArea6", float.Parse(textBox11.Text));
            //DC plots
            plot_general("Vdcpcu", "Vdcpowermeter", "Vdcconfigured", "ChartArea2", float.Parse(textBox5.Text));
            //special plot for apparant current pcu ac current rerting
            plot_apparant_current("Iacpcu", "Iacpowermeter", "Iacimagpcu", "ChartArea5", float.Parse(textBox8.Text));
        }

        private void plot_general(string pcu, string powermeter, string cnf, string chartarea, float FS)
        {
            //clear series and add the series
            //chart3.Series.Clear();
            List<string> series = new List<string> { pcu, powermeter, cnf };
            addlist_series_tochart(chart3, series);
            //Get the column float values: (cnf is configured values)
            List<float> CNF = getfloatlist(cnf);
            List<float> PM = getfloatlist(powermeter);
            List<float> PCU = getfloatlist(pcu);
            //Get the bool fail list for specific margins (The ones are percentage 1) 
            List<bool> accuracyfaillist = new List<bool>(Calculate.Faillist(PCU, PM, 1, FS, 1));
            chart3.Series[powermeter].ChartArea = chartarea;
            chart3.Series[cnf].ChartArea = chartarea;
            setupseriescross(chart3.Series[cnf]);
            setupseriescircle(chart3.Series[powermeter]);
            for (int i = 0; i < PM.Count; i++)
            {
                if (accuracyfaillist[i] == false)
                {
                    //Vdcvbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    chart3.Series[powermeter].Points.AddY(PM[i]);
                    chart3.Series[cnf].Points.AddY(CNF[i]);
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
        private void plot_apparant_current(string pcureal, string powermeter, string reactive, string chartarea, float FS)
        {
            //clear series and add the series
            //chart3.Series.Clear();
            //setup series names to be added to chart3
            List<string> series = new List<string> { powermeter, "pcuIacs" };
            addlist_series_tochart(chart3, series);
            //get the powermeter float values
            List<float> PM = getfloatlist(powermeter);
            //Calculate tthe aparant ac current list from the pcu
            List<float> appartcurrent = new List<float>
                (Calculate.Get_pcu_apparantcurrent(getfloatlist(reactive), getfloatlist(pcureal)));
            //Get the bool fail list for specific margins (The ones are percentage 1) 
            List<bool> accuracyfaillist = new List<bool>(Calculate.Faillist(appartcurrent, PM, 1, FS, 1));
            chart3.Series[powermeter].ChartArea = chartarea;
            chart3.Series["pcuIacs"].ChartArea = chartarea;
            setupseriescross(chart3.Series["pcuIacs"]);
            setupseriescircle(chart3.Series[powermeter]);
            for (int i = 0; i < PM.Count; i++)
            {
                if (accuracyfaillist[i] == false)
                {
                    //Vdcvbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    chart3.Series[powermeter].Points.AddY(PM[i]);
                    chart3.Series["pcuIacs"].Points.AddY(appartcurrent[i]);
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
    }
}
