using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace PlotDVT
{
    class Dcacplot
    {
        public Dcacplot()
        {
            
        }

        public void plotdcaccombo(Chart chart1, ChartArea accurrent)
        {
            //create new chart area, give it a name ex. "ac"
            accurrent = new ChartArea("ac");
            chart1.ChartAreas.Add(accurrent);
            //create a new series
            Series dcseries = new Series();
            chart1.Series.Add(dcseries);
            //link the series to the area
            dcseries.ChartArea = "ac";
            dcseries.ChartType = SeriesChartType.Point;
            dcseries.MarkerStyle = MarkerStyle.Cross;
            dcseries.MarkerSize = 13;
            dcseries.Points.AddXY(10, 0);
        }
    }
}
