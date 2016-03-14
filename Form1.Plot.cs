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
            //DC volt plots chartarea2
            chart3.ChartAreas[1].AxisY.Minimum = 0;
            chart3.ChartAreas[1].AxisY.Maximum = 50;
            chart3.ChartAreas[1].AxisY.Interval = 5; // Whatever you like
            //DC current plots chartarea3
            chart3.ChartAreas[2].AxisY.Minimum = -3;
            chart3.ChartAreas[2].AxisY.Maximum = 15;
            chart3.ChartAreas[2].AxisY.Interval = 1; // Whatever you like

            Title title = new Title("Powermeter VA against VAR: " + textBox1.Text,
                Docking.Top, new Font("Verdana", 10, FontStyle.Regular), Color.Black);
            chart3.Titles.Add(title);
            title.IsDockedInsideChartArea = false;
            title.DockedToChartArea = chart3.ChartAreas[0].Name;
            //AC plots Below are the values for chart one
            List<float> Wacpm = getfloatlist("Wacpowermeter");
            //List<float> Wacpm = new List<float>(colobjinterflist.First(item => item.GetName() == "Wacpowermeter").GetFloats());
            List<float> Wvarpm = getfloatlist("ACvarpowermeter");
            List<float> Waccnf = getfloatlist("Wacconfigured");
            List<float> Wvarcnf = getfloatlist("Wacvarconfigured");
            //never plot these just use them to determine accuracy
            List<float> WACpcu = getfloatlist("Wacpcu");
            List<float> WACimagpcu = getfloatlist("Wacimagpcu");
            //DC plots
            List<float> Dcvcnf = getfloatlist("Vdcconfigured");
            List<float> Dcvpm = getfloatlist("Vdcpowermeter");
            List<float> Dcvpcu = getfloatlist("Vdcpcu");
            //Idc plots
            List<float> Dcicnf = getfloatlist("Idcconfigured");
            List<float> Dcipm = getfloatlist("Idcpowermeter");
            List<float> Dcipcu = getfloatlist("Idcpcu");
            //Get a bool fail list to do selective plotting
            List<bool> accuracyfaillist = new List<bool>
                (Calculate.Faillist(WACpcu, Wacpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistvar = new List<bool>
                (Calculate.Faillist(WACimagpcu, Wvarpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistdcv = new List<bool>
                (Calculate.Faillist(Dcvpcu, Dcvpm, 1, float.Parse(textBox5.Text), 1));
            List<bool> accuracyfaillistdci = new List<bool>
                (Calculate.Faillist(Dcipcu, Dcipm, 1, float.Parse(textBox6.Text), 1));
            //List<bool> nopowerfaillistvar = new List<bool>(Calculate.Faillist(WACimagpcu, Wvarcnf, 1, float.Parse(textBox7.Text), 1));
            chart3.Series.Add(["Wacpowermeter", "Wacconfigured"]);
            chart3.Series.Add("Wacconfigured");
            //DC plots add series and attach it to ChartArea2
            chart3.Series.Add("DCvpm");
            chart3.Series.Add("Vdcconfigured");
            //Idc plots Add
            chart3.Series.Add("DCipm");
            chart3.Series.Add("Idcconfigured");
            //Vdcconnect to Chartarea
            chart3.Series["DCvpm"].ChartArea = "ChartArea2";
            chart3.Series["Vdcconfigured"].ChartArea = "ChartArea2";
            //Idc
            chart3.Series["DCipm"].ChartArea = "ChartArea3";
            chart3.Series["Idcconfigured"].ChartArea = "ChartArea3";
            ////configure series plots her
            Series Waccnfseries = chart3.Series["Wacconfigured"];
            Series DCvpm = chart3.Series["DCvpm"];
            Series Vdcconfigured = chart3.Series["Vdcconfigured"];
            Series Wacpowermeter = chart3.Series["Wacpowermeter"];
            //Idc
            Series DCipm = chart3.Series["DCipm"];
            Series Idcconfigured = chart3.Series["Idcconfigured"];
            //var and watt
            setupseriesline(Wacpowermeter);
            setupseriescross(Waccnfseries);
            //Vdc
            setupseriescross(Vdcconfigured);
            setupseriescircle(DCvpm);
            //Idc
            setupseriescross(Idcconfigured);
            setupseriescircle(DCipm);
            //**END configure series**
            //Inititalise lists for different bugs 
            Wacpowerbuglist = new Bugs();
            Vdcvbuglist = new Bugs();
            Idcvbuglist = new Bugs();

            for (int i = 0; i < Wacpm.Count; i++)
            {
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

                if(accuracyfaillistdcv[i] == false)
                {
                    Vdcvbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    DCvpm.Points.AddY(Dcvpm[i]);
                    Vdcconfigured.Points.AddY(Dcvcnf[i]);
                }

                if (accuracyfaillistdci[i] == false)
                {
                    Idcvbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    DCipm.Points.AddY(Dcipm[i]);
                    Idcconfigured.Points.AddY(Dcicnf[i]);
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
    }
}
