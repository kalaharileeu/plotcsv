using System;
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
        /// Plots the Var vs real power triangle
        /// </summary>
        private async void acvarvapowerasync()
        {
            //List<YourType> newList = new List<YourType>(oldList);
            chartdefaults();
            //clear value everytime button calls this function
            chart3.Series.Clear();
            chart3.Titles.Clear();
            string seriesac = "";
            string seriesvar = "";
            //Set up axis variables
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisX.Maximum = 300;
            chart3.ChartAreas[0].AxisX.Interval = 20; // Whatever you like
            chart3.ChartAreas[0].AxisY.Minimum = -300;
            chart3.ChartAreas[0].AxisY.Maximum = 300;
            chart3.ChartAreas[0].AxisY.Interval = 20; // Whatever you like

            Title title = new Title("Powermeter VA against VAR: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart3.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart3.ChartAreas[0].Name;
            ///Get reference to all the column classes, to acces their data
            IBaselist ibl5 = colobjinterflist.First(item => item.GetName() == "Wacpowermeter");
            IBaselist ibl6 = colobjinterflist.First(item => item.GetName() == "Wacconfigured");
            IBaselist ibl7 = colobjinterflist.First(item => item.GetName() == "Wacvarconfigured");
            IBaselist ibl8 = colobjinterflist.First(item => item.GetName() == "ACvarpowermeter");
            //Below are the values for chart one
            List<float> values = new List<float>();//ibl5
            List<float> waccnf = new List<float>();//ibl6
            List<float> wacvarcnf = new List<float>();//ibl7
            //ibl8 values = ACvarpowermeter
            foreach (var kv in ibl8.GetSlices())
            {
                //create the series name from the values 
                seriesac = (Convert.ToString(kv.Key));
                seriesvar = "cnf";
                if (seriesac.Length > 4)
                    seriesac = seriesac.Substring(0, 4);
                //create another series name 
                seriesvar += seriesac;
                seriesac += "Wvar";
                ////Add series to plot
                chart3.Series.Add(seriesac);
                chart3.Series.Add(seriesvar);
                ////configure series
                chart3.Series[seriesac].ChartType = SeriesChartType.Line;
                chart3.Series[seriesac].BorderWidth = 1;
                chart3.Series[seriesac].IsVisibleInLegend = false;
                chart3.Series[seriesvar].ChartType = SeriesChartType.Point;
                chart3.Series[seriesvar].MarkerStyle = MarkerStyle.Cross;
                chart3.Series[seriesvar].MarkerSize = 8;
                chart3.Series[seriesvar].IsVisibleInLegend = false;
                //Find the key values in every series
                if (ibl5.GetSlices().ContainsKey(kv.Key))
                    values = ibl5.GetSlices()[kv.Key];
                if (ibl6.GetSlices().ContainsKey(kv.Key))
                    waccnf = ibl6.GetSlices()[kv.Key];
                if (ibl7.GetSlices().ContainsKey(kv.Key))
                    wacvarcnf = ibl7.GetSlices()[kv.Key];

                if (values.Count <= kv.Value.Count)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        //Plot the congfigured w/va powermeter values
                        chart3.Series[seriesac].Points.AddXY(0, 0);
                        chart3.Series[seriesac].Points.AddXY(values[i], 0);
                        chart3.Series[seriesac].Points.AddXY(values[i], kv.Value[i]);
                        ////Plot the congfigured w/va configured values
                        chart3.Series[seriesvar].Points.AddXY(0, 0);//17Wvar'
                        chart3.Series[seriesvar].Points.AddXY(waccnf[i], 0);
                        chart3.Series[seriesvar].Points.AddXY(waccnf[i], wacvarcnf[i]);

                        await Task.Delay(Convert.ToInt16(textBox2.Text));
                    }
                    if (reverse)//not actually reverse, just delete previous plot
                    {
                        chart3.Series[seriesac].Points.Clear();
                        chart3.Series[seriesvar].Points.Clear();
                    }
                }
            }
        }

        private async void async_col_sequenceplot()
        {
            //List<YourType> newList = new List<YourType>(oldList);
            chartdefaults();
            //clear value everytime button calls this function
            chart3.Series.Clear();
            chart3.Titles.Clear();
            //AC plots Set up axis variables
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisX.Maximum = 300;
            chart3.ChartAreas[0].AxisX.Interval = 20; // Whatever you like
            chart3.ChartAreas[0].AxisY.Minimum = -300;
            chart3.ChartAreas[0].AxisY.Maximum = 300;
            chart3.ChartAreas[0].AxisY.Interval = 20; // Whatever you like
            //DC volt plots chart area 1
            chart3.ChartAreas[1].AxisY.Minimum = 0;
            chart3.ChartAreas[1].AxisY.Maximum = 50;
            chart3.ChartAreas[1].AxisY.Interval = 5; // Whatever you like

            Title title = new Title("Powermeter VA against VAR: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart3.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart3.ChartAreas[0].Name;
            ///Get reference to all the column classes, to acces their data
            IBaselist Wacpowermeter = colobjinterflist.First(item => item.GetName() == "Wacpowermeter");
            IBaselist Wacconfigured = colobjinterflist.First(item => item.GetName() == "Wacconfigured");
            IBaselist Wacvarconfigured = colobjinterflist.First(item => item.GetName() == "Wacvarconfigured");
            IBaselist ACvarpowermeter = colobjinterflist.First(item => item.GetName() == "ACvarpowermeter");
            IBaselist Wacpcu = colobjinterflist.First(item => item.GetName() == "Wacpcu");
            IBaselist Wacimagpcu = colobjinterflist.First(item => item.GetName() == "Wacimagpcu");

            IBaselist Vdcconfigured = colobjinterflist.First(item => item.GetName() == "Vdcconfigured");
            IBaselist Vdcpcu = colobjinterflist.First(item => item.GetName() == "Vdcpcu");
            //AC plots Below are the values for chart one
            List<float> Wacpm = new List<float>(Wacpowermeter.GetFloats());
            List<float> Wvarpm = new List<float>(ACvarpowermeter.GetFloats());
            List<float> Waccnf = new List<float>(Wacconfigured.GetFloats());
            List<float> Wvarcnf = new List<float>(Wacvarconfigured.GetFloats());
            List<float> WACpcu = new List<float>(Wacpcu.GetFloats());
            List<float> WACimagpcu = new List<float>(Wacimagpcu.GetFloats());
            //DC plots
            List<float> Dcvcnf = new List<float>(Vdcconfigured.GetFloats());
            List<float> Dcvpm = new List<float>(Vdcpcu.GetFloats());
            //Add series to plot
            chart3.Series.Add("Wacpowermeter");
            chart3.Series.Add("Wacconfigured");
            //DC plots add series and attach it to ChartArea2
            chart3.Series.Add("DCvpm");
            chart3.Series.Add("Vdcconfigured");
            chart3.Series["DCvpm"].ChartArea = "ChartArea2";
            chart3.Series["Vdcconfigured"].ChartArea = "ChartArea2";
            ////configure series plots
            chart3.Series["Wacpowermeter"].ChartType = SeriesChartType.Line;
            chart3.Series["Wacpowermeter"].BorderWidth = 3;
            chart3.Series["Wacpowermeter"].IsVisibleInLegend = false;
            chart3.Series["Wacconfigured"].ChartType = SeriesChartType.Point;
            chart3.Series["Wacconfigured"].MarkerStyle = MarkerStyle.Cross;
            chart3.Series["Wacconfigured"].MarkerSize = 15;
            chart3.Series["Wacconfigured"].IsVisibleInLegend = false;
            chart3.Series["DCvpm"].ChartType = SeriesChartType.Line;
            chart3.Series["DCvpm"].BorderWidth = 5;
            chart3.Series["DCvpm"].IsVisibleInLegend = false;
            chart3.Series["Vdcconfigured"].ChartType = SeriesChartType.Point;
            chart3.Series["Vdcconfigured"].MarkerStyle = MarkerStyle.Cross;
            chart3.Series["Vdcconfigured"].MarkerSize = 15;
            chart3.Series["Vdcconfigured"].IsVisibleInLegend = false;

            for (int i = 0; i < Wacpm.Count; i++)
            {
                //Plot the congfigured w/va powermeter values
                chart3.Series["Wacpowermeter"].Points.AddXY(0, 0);
                chart3.Series["Wacpowermeter"].Points.AddXY(Wacpm[i] - WACpcu[i], 0);
                chart3.Series["Wacpowermeter"].Points.AddXY(Wacpm[i] - WACpcu[i], Wvarpm[i] - WACimagpcu[i]);
                chart3.Series["Wacpowermeter"].Points.AddXY(0, 0);
                ////Plot the congfigured w/va configured values
                //chart3.Series["Wacconfigured"].Points.AddXY(0, 0);//17Wvar'
                chart3.Series["Wacconfigured"].Points.AddXY(Waccnf[i], 0);
                chart3.Series["Wacconfigured"].Points.AddXY(Waccnf[i], Wvarcnf[i]);

                //Plot the congfigured Vdcpm/cnf powermeter values
                chart3.Series["DCvpm"].Points.AddXY(0, 0);
                chart3.Series["DCvpm"].Points.AddXY(0, Dcvpm[i]);
                chart3.Series["Vdcconfigured"].Points.AddXY(0, Dcvcnf[i]);
                await Task.Delay(Convert.ToInt16(textBox2.Text));
                //Clear all the data points
                chart3.Series["Wacpowermeter"].Points.Clear();
                chart3.Series["Wacconfigured"].Points.Clear();
                chart3.Series["DCvpm"].Points.Clear();
                chart3.Series["Vdcconfigured"].Points.Clear();
            }
            if (reverse)//not actually reverse, just delete previous plot
            {
                chart3.Series["Wacpowermeter"].Points.Clear();
                chart3.Series["Wacconfigured"].Points.Clear();
            }
        }

    }
}
