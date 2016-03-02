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
    }
}
