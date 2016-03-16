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
            //chart3.Titles.Clear();
            //AC plots Set up axis variables
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisX.Maximum = 300;
            chart3.ChartAreas[0].AxisX.Interval = 40; 
            chart3.ChartAreas[0].AxisY.Minimum = -280;
            chart3.ChartAreas[0].AxisY.Maximum = 280;
            chart3.ChartAreas[0].AxisY.Interval = 40; 
            //DC volt plots chartarea2
            chart3.ChartAreas[1].AxisY.Minimum = 0;
            chart3.ChartAreas[1].AxisY.Maximum = 50;
            chart3.ChartAreas[1].AxisY.Interval = 5;
            //DC current plots chartarea3
            chart3.ChartAreas[2].AxisY.Minimum = -3;
            chart3.ChartAreas[2].AxisY.Maximum = 15;
            chart3.ChartAreas[2].AxisY.Interval = 1;
            //AC volts plots chartarea4
            chart3.ChartAreas[3].AxisY.Minimum = -3;
            chart3.ChartAreas[3].AxisY.Maximum = 302;
            chart3.ChartAreas[3].AxisY.Interval = 40;
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
            //AC volts
            List<float> Vacpcu = getfloatlist("Vacpcu");
            List<float> Vacpm = getfloatlist("Vacpowermeter");
            List<bool> accuracyfaillistvac = new List<bool>
                (Calculate.Faillist(Vacpcu, Vacpm, 1, float.Parse(textBox7.Text), 1));
            //Get a bool fail list to do selective plotting
            List<bool> accuracyfaillist = new List<bool>
                (Calculate.Faillist(WACpcu, Wacpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistvar = new List<bool>
                (Calculate.Faillist(WACimagpcu, Wvarpm, 1, float.Parse(textBox9.Text), 1));
            List<bool> accuracyfaillistdcv = new List<bool>
                (Calculate.Faillist(Dcvpcu, Dcvpm, 1, float.Parse(textBox5.Text), 1));
            List<bool> accuracyfaillistdci = new List<bool>
                (Calculate.Faillist(Dcipcu, Dcipm, 1, float.Parse(textBox6.Text), 1));
            //Create all the list of series I want to plot
            List<string> listofseries = new List<string>()
            { "Wacpowermeter", "Wacconfigured", "DCvpm", "Vdcconfigured", "DCipm", "Idcconfigured", "Vacpcu", Vacpm.ToString() };
            addlist_series_tochart(chart3, listofseries);
            //Vdcconnect to Chartarea
            chart3.Series["DCvpm"].ChartArea = "ChartArea2";
            chart3.Series["Vdcconfigured"].ChartArea = "ChartArea2";
            //Idc
            chart3.Series["DCipm"].ChartArea = "ChartArea3";
            chart3.Series["Idcconfigured"].ChartArea = "ChartArea3";
            //Vac add to chartarea
            chart3.Series["Vacpcu"].ChartArea = "ChartArea4";
            chart3.Series[Vacpm.ToString()].ChartArea = "ChartArea4";
            ////configure series plots variables her3
            Series Waccnfseries = chart3.Series["Wacconfigured"];
            Series DCvpm = chart3.Series["DCvpm"];
            Series Vdcconfigured = chart3.Series["Vdcconfigured"];
            Series Wacpowermeter = chart3.Series["Wacpowermeter"];
            //Idc
            Series DCipm = chart3.Series["DCipm"];
            Series Idcconfigured = chart3.Series["Idcconfigured"];
            //Vac
            Series ACVpm = chart3.Series[Vacpm.ToString()];
            Series ACVpcu = chart3.Series["Vacpcu"];
            //Vac setup plot types and variables
            setupseriescircle(ACVpm);
            setupseriescross(ACVpcu);
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
            Wacpowerbuglist = new Bugs(0);
            Vdcvbuglist = new Bugs(1);
            Idcvbuglist = new Bugs(2);
            Vacbuglist = new Bugs(3);
            List<Bugs> Bugslist = new List<Bugs>() { Wacpowerbuglist, Vdcvbuglist, Idcvbuglist, Vacbuglist };


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

                if (accuracyfaillistvac[i] == false)
                {
                    Vacbuglist.Addbug(CSVrowManager.GetaCSVrow(i));
                    ACVpm.Points.AddY(Vacpm[i]);
                    ACVpcu.Points.AddY(Vacpcu[i]);
                }
            }
            // Create a list of values to extract
            List<string> valuesforreport = new List<string>()
            {
                "Wacvarconfigured", "Wacconfigured", "Wdcconfigured", "Vdcconfigured",
                "Phaseconfigured", "Temperature", "Vacpowermeter"
            };

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
        /// add a list of serise names to a chart
        /// </summary>
        /// <param name="x">Chart</param>
        /// <param name="serieslist">list of series names - strings</param>
        private void addlist_series_tochart(Chart x, List<string> serieslist)
        {
            foreach(string s in serieslist)
            {
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
        /// change the background image to a thumbs up all good, scled to chartimage size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="area_no"></param>
        private void set_background_image(Chart x, int area_no)
        {
            x.ChartAreas[area_no].BackImage = "Content/thumbsup.jpg";
            x.ChartAreas[area_no].BackImageWrapMode = ChartImageWrapMode.Scaled;
        }
        /// <summary>
        /// Chagne the background image to neautral color
        /// </summary>
        /// <param name="x"></param>
        /// <param name="area_no"></param>
        private void clear_background_image(Chart x, int area_no)
        {
            x.ChartAreas[area_no].BackImage = "Content/cream.jpg";
        }
    }
}
