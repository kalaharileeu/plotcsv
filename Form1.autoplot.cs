using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Imaging;

namespace PlotDVT
{
    partial class Form1
    {
        private async void plottoprint()
        {
            List<Button> thebuttons = new List<Button>();
            //thebuttons.Add(button1); //efficiency chart 2 go !
            thebuttons.Add(button4);//DC current
            thebuttons.Add(button15);//Vdc reporting accuracy go!
            thebuttons.Add(button16);//Idc reporting accuracy go!
            thebuttons.Add(button17);//Wdc reporting accuracy go!
            int delay = 80;

            button1.PerformClick();
            await Task.Delay(delay);
            chart2.SaveImage("C:\\temp\\" + button1.Text + ".jpg", ChartImageFormat.Jpeg);
            await Task.Delay(delay);
     //0 deg here!!!!!!!!!!!!
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                //chart1.SaveImage("C:\\temp\\mychart.jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
                chart2.SaveImage("C:\\temp\\" + b.Text + button9.Text  +".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
  ////-45 deg here!!!!!!!!
            button6.PerformClick();//change to -45 degrees C
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                chart2.SaveImage("C:\\temp\\" + b.Text + button6.Text + ".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
    ////-30 deg here!!!!!!!!
            button7.PerformClick();//change to -30 degrees C
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                chart2.SaveImage("C:\\temp\\" + b.Text + button7.Text + ".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
     ////-15 deg here!!!!!!!!
            button8.PerformClick();//change to -15 degrees C
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                chart2.SaveImage("C:\\temp\\" + b.Text + button8.Text + ".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
     ////15 deg here!!!!!!!!
            button10.PerformClick();//change to 15 degrees C
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                chart2.SaveImage("C:\\temp\\" + b.Text + button10.Text + ".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
      ////30 deg here!!!!!!!!
            button11.PerformClick();//change to 30 degrees C
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                chart2.SaveImage("C:\\temp\\" + b.Text + button11.Text + ".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
        ////45 deg here!!!!!!!!
            button12.PerformClick();//change to 45 degrees C
            foreach (Button b in thebuttons)
            {
                b.PerformClick();
                chart2.SaveImage("C:\\temp\\" + b.Text + button12.Text + ".jpg", ChartImageFormat.Jpeg);
                await Task.Delay(delay);
            }
//create the report
            CreateSampleDocument();

        }

        private void button20_Click(object sender, EventArgs e)
        {
            plottoprint();
            //chart1.SaveImage("C:\\temp\\mychart.jpg", ChartImageFormat.Jpeg);
            ////Clear the charts




        }
    }
}
