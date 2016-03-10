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
        /// Test paramteres should either come from database or some defaults needs to
        /// be at ahand in the textbox if it cannot connect to the database
        /// </summary>
        private void settestparameters()
        {
            //values should come from database later
            textBox5.Text = "60";//FS Vdc
            textBox6.Text = "12";//FS Idc
            textBox7.Text = "284";//FS Vac
            textBox8.Text = "1.2";//FS Iac
            textBox9.Text = "220";//FS Sac
        }
        /// <summary>
        /// Plots the Var vs real power triangle
        /// </summary>
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
            //AC plots Below are the values for chart one
            List<float> Wacpm = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacpowermeter").GetFloats());
            List<float> Wvarpm = new List<float>(colobjinterflist.First(item => item.GetName() == "ACvarpowermeter").GetFloats());
            List<float> Waccnf = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacconfigured").GetFloats());
            List<float> Wvarcnf = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacvarconfigured").GetFloats());
            //never plot these just use them to determine accuracy
            List<float> WACpcu = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacpcu").GetFloats());
            List<float> WACimagpcu = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacimagpcu").GetFloats());
            //DC plots
            List<float> Dcvcnf = new List<float>(colobjinterflist.First(item => item.GetName() == "Vdcconfigured").GetFloats());
            List<float> Dcvpm = new List<float>(colobjinterflist.First(item => item.GetName() == "Vdcpcu").GetFloats());
            //Get a fail list to do selective plotting
            List<bool> accuracyfaillist = new List<bool>
                (Calculate.Faillist(WACpcu, Wacpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistvar = new List<bool>
                (Calculate.Faillist(WACimagpcu, Wvarpm, 1, float.Parse(textBox9.Text), 1));
            //List<bool> nopowerfaillistvar = new List<bool>(Calculate.Faillist(WACimagpcu, Wvarcnf, 1, float.Parse(textBox7.Text), 1));
            chart3.Series.Add("Wacpowermeter");
            chart3.Series.Add("Wacconfigured");
            //DC plots add series and attach it to ChartArea2
            chart3.Series.Add("DCvpm");
            chart3.Series.Add("Vdcconfigured");
            chart3.Series["DCvpm"].ChartArea = "ChartArea2";
            chart3.Series["Vdcconfigured"].ChartArea = "ChartArea2";
            ////configure series plots
            Series Waccnfseries = chart3.Series["Wacconfigured"];
            chart3.Series["Wacpowermeter"].ChartType = SeriesChartType.Line;
            chart3.Series["Wacpowermeter"].BorderWidth = 1;
            chart3.Series["Wacpowermeter"].IsVisibleInLegend = false;
            Waccnfseries.ChartType = SeriesChartType.Point;
            Waccnfseries.MarkerStyle = MarkerStyle.Cross;
            Waccnfseries.MarkerSize = 6;
            Waccnfseries.IsVisibleInLegend = false;
            chart3.Series["DCvpm"].ChartType = SeriesChartType.Line;
            chart3.Series["DCvpm"].BorderWidth = 1;
            chart3.Series["DCvpm"].IsVisibleInLegend = false;
            chart3.Series["Vdcconfigured"].ChartType = SeriesChartType.Point;
            chart3.Series["Vdcconfigured"].MarkerStyle = MarkerStyle.Cross;
            chart3.Series["Vdcconfigured"].MarkerSize = 6;
            chart3.Series["Vdcconfigured"].IsVisibleInLegend = false;
            Wacpowerbuglist = new Bugs();
            for (int i = 0; i < Wacpm.Count; i++)
            {
                if (accuracyfaillist[i] == false || accuracyfaillistvar[i] == false)
                {
                    Wacpowerbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    //Plot the congfigured w/va powermeter values
                    chart3.Series["Wacpowermeter"].Points.AddXY(0, 0);
                    chart3.Series["Wacpowermeter"].Points.AddXY(Wacpm[i], 0);
                    chart3.Series["Wacpowermeter"].Points.AddXY(Wacpm[i], Wvarpm[i]);
                    chart3.Series["Wacpowermeter"].Points.AddXY(0, 0);
                    ////Plot the congfigured w/va configured values
                    chart3.Series["Wacconfigured"].Points.AddXY(Waccnf[i], 0);
                    chart3.Series["Wacconfigured"].Points.AddXY(Waccnf[i], Wvarcnf[i]);
                    //Plot the congfigured Vdcpm/cnf powermeter values
                    //chart3.Series["DCvpm"].Points.AddXY(0, 0);
                    chart3.Series["DCvpm"].Points.AddXY(Dcvpm[i], Waccnf[i]);
                    chart3.Series["Vdcconfigured"].Points.AddXY(Dcvcnf[i], Waccnf[i]);
                    //await Task.Delay(Convert.ToInt16(textBox2.Text));
                    ////Clear all the data points
                    //if (chart3.Series != null)
                    //{
                    //    foreach (Series s in chart3.Series)
                    //    {
                    //        clearseries(s);
                    //    }
                    //}
                }
            }
        }

        private void clearseries(Series x)
        {
            if((x != null))
            {
                x.Points.Clear();
            }
        }
    }
}
