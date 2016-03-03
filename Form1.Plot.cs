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
    }
}
