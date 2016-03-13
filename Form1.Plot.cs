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
            await Task.Delay(100);
            //chartdefaults();
            //clear value everytime button calls this function
            chart3.Series.Clear();
            chart3.Titles.Clear();
            //AC plots Set up axis variables
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisX.Maximum = 300;
            chart3.ChartAreas[0].AxisX.Interval = 40; // Whatever you like
            chart3.ChartAreas[0].AxisY.Minimum = -280;
            chart3.ChartAreas[0].AxisY.Maximum = 280;
            chart3.ChartAreas[0].AxisY.Interval = 40; // Whatever you like
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
            List<float> Dcvpm = new List<float>(colobjinterflist.First(item => item.GetName() == "Vdcpowermeter").GetFloats());
            List<float> Dcvpcu = new List<float>(colobjinterflist.First(item => item.GetName() == "Vdcpcu").GetFloats());
            //Get a fail list to do selective plotting
            List<bool> accuracyfaillist = new List<bool>
                (Calculate.Faillist(WACpcu, Wacpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistvar = new List<bool>
                (Calculate.Faillist(WACimagpcu, Wvarpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistdcv = new List<bool>
                (Calculate.Faillist(Dcvpcu, Dcvpm, 1, float.Parse(textBox5.Text), 1));
            //List<bool> nopowerfaillistvar = new List<bool>(Calculate.Faillist(WACimagpcu, Wvarcnf, 1, float.Parse(textBox7.Text), 1));
            chart3.Series.Add("Wacpowermeter");
            chart3.Series.Add("Wacconfigured");
            //DC plots add series and attach it to ChartArea2
            chart3.Series.Add("DCvpm");
            chart3.Series.Add("Vdcconfigured");
            chart3.Series["DCvpm"].ChartArea = "ChartArea2";
            chart3.Series["Vdcconfigured"].ChartArea = "ChartArea2";
            ////configure series plots her
            Series Waccnfseries = chart3.Series["Wacconfigured"];
            Series DCvpm = chart3.Series["DCvpm"];
            Series Vdcconfigured = chart3.Series["Vdcconfigured"];
            Series Wacpowermeter = chart3.Series["Wacpowermeter"];
            setupseriesline(Wacpowermeter);
            setupseriescross(Waccnfseries);
            setupseriescross(Vdcconfigured);
            setupseriescircle(DCvpm);
            //Wacpowermeter.ChartType = SeriesChartType.Line;
            //Wacpowermeter.BorderWidth = 1;
            //Wacpowermeter.IsVisibleInLegend = false;
            //Waccnfseries.ChartType = SeriesChartType.Point;
            //Waccnfseries.MarkerStyle = MarkerStyle.Cross;
            //Waccnfseries.MarkerSize = 6;
            //Waccnfseries.IsVisibleInLegend = false;
            //DCvpm.ChartType = SeriesChartType.Point;
            //DCvpm.MarkerStyle = MarkerStyle.Circle;
            //DCvpm.BorderWidth = 6;
            //DCvpm.IsVisibleInLegend = false;
            //Vdcconfigured.ChartType = SeriesChartType.Point;
            //Vdcconfigured.MarkerStyle = MarkerStyle.Cross;
            //Vdcconfigured.MarkerSize = 6;
            //Vdcconfigured.IsVisibleInLegend = false;
            //**END configure series**
            //Inititalise lists for different bugs 
            Wacpowerbuglist = new Bugs();
            Wdcvbuglist = new Bugs();
            for (int i = 0; i < Wacpm.Count; i++)
            {
                if(accuracyfaillist[i] == false || accuracyfaillistvar[i] == false)
                {
                    //ignore the values that is below this value in textbox10
                    if (Wacpm[i] > float.Parse(textBox10.Text))
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
                    }
                 }

                if(accuracyfaillistdcv[i] == false)
                {
                    Wdcvbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    chart3.Series["DCvpm"].Points.AddY(Dcvpm[i]);
                    chart3.Series["Vdcconfigured"].Points.AddY(Dcvcnf[i]);
                }
            }
            // Create a list of values to extract
            List<string> valuesforreport = new List<string>()
            {
            "Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured", "Phaseconfigured", "Temperature"
            };
            if (Wacpowerbuglist != null)
            {
                foreach (CSVrow row in Wacpowerbuglist.Bugrows)
                    richTextBox1.AppendText(row.Humantext(valuesforreport) + "\r\n");
            }
        }

        private void setupseriescross(Series x)
        {
            x.ChartType = SeriesChartType.Point;
            x.MarkerStyle = MarkerStyle.Cross;
            x.MarkerSize = 7;
            x.IsVisibleInLegend = false;
        }

        private void setupseriescircle(Series x)
        {
            x.ChartType = SeriesChartType.Point;
            x.MarkerStyle = MarkerStyle.Circle;
            x.MarkerSize = 7;
            x.IsVisibleInLegend = false;
        }

        private void setupseriesline(Series x)
        {
            x.ChartType = SeriesChartType.Line;
            x.BorderWidth = 2;
            x.IsVisibleInLegend = false;
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
