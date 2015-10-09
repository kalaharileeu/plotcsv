using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PlotDVT
{
    partial class Form1
    {
        //Efficiency button
        private void button1_Click(object sender, EventArgs e)
        {
            ploteff();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlotVdccompare();
        }
        //Load the baseline file
        private void button3_Click(object sender, EventArgs e)
        {
            //Im the baseline dialog
            this.openFileDialog1.Filter = "csv files (*.csv)|*.csv";
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            //string filename = "nothing";
            if (result == DialogResult.OK) // Test result.
            {
                filenamebl = openFileDialog1.FileName;
            }
            //get the file info from the path
            FileInfo info = new FileInfo(filenamebl);
            baselinereadauxinfo(filenamebl);
            //check file info
            if (IsFileLocked(info))
            {
                MessageBox.Show("The file is in use or locked!");
            }
            else
            {
                populatedatabaseline(filenamebl);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            plotIdc();
        }

        //load the unit under test file
        private void button5_Click(object sender, EventArgs e)
        {
            //Im the unit under test dialog
            this.openFileDialog2.Filter = "csv files (*.csv)|*.csv";
            // Show the dialog and get result.
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                filenamedata = openFileDialog2.FileName;
            }
            //get the file info from the path
            FileInfo info = new FileInfo(filenamedata);
            uutreadauxinfo(filenamedata);
            if (IsFileLocked(info))
            {
                MessageBox.Show("The file is in use or locked!");
            }
            else
            {
                populatedatatestunit(filenamedata);
            }
        }
        //Read the auxilliary information from the test folder text file
        private void uutreadauxinfo(string fn)
        {
            uutdetail = "";//use this for document generation, clear it
            string thepath = Path.GetDirectoryName(fn);
            string txtpath = Path.Combine();
            string resultString = Regex.Match(thepath, @"\d{12}").Value;//find the serial number in the path
            textBox3.AppendText("Baseline serial number is: " + resultString + "\n");
            DirectoryInfo resultsdirectory = new DirectoryInfo(thepath);
            FileInfo[] filesindir = resultsdirectory.GetFiles("*" + resultString + "*" + ".txt");
            if (filesindir.Length != 0)
            {
                foreach (var lin in File.ReadLines(filesindir[0].FullName).SkipWhile
                    (line => !line.Contains("Debugger")).TakeWhile(line => !line.Contains("d>")))
                {
                    textBox3.AppendText(lin + "\n");
                    uutdetail += (lin + "\n");
                }
                foreach (var lin in File.ReadLines(filesindir[0].FullName).SkipWhile
                    (line => !line.Contains("Station")).TakeWhile(line => !line.Contains("User")))
                {
                    textBox3.AppendText(lin + "\n");
                    uutdetail += (lin + "\n");
                }
            }
            else
            {
                textBox3.AppendText("The .txt info file is not there");
            }
        }
        //Read the auxilliary information from the test folder text file
        private void baselinereadauxinfo(string fn)
        {
            baselinedetail = "";//use this for document deneration, clear it
            string thepath = Path.GetDirectoryName(fn);
            string txtpath = Path.Combine();
            string resultString = Regex.Match(thepath, @"\d{12}").Value;//find the serial number in the path
            textBox4.AppendText("Baseline serial number is: " + resultString + "\n");
            DirectoryInfo resultsdirectory = new DirectoryInfo(thepath);
            FileInfo[] filesindir = resultsdirectory.GetFiles("*" + resultString + "*" + ".txt");
            if (filesindir.Length != 0)
            {
                foreach (var lin in File.ReadLines(filesindir[0].FullName).SkipWhile
                    (line => !line.Contains("Debugger")).TakeWhile(line => !line.Contains("d>")))
                {
                    textBox4.AppendText(lin + "\n");
                    baselinedetail += (lin + "\n");
                }
                foreach (var lin in File.ReadLines(filesindir[0].FullName).SkipWhile
                    (line => !line.Contains("Station")).TakeWhile(line => !line.Contains("User")))
                {
                    textBox4.AppendText(lin + "\n");
                    baselinedetail += (lin + "\n");
                }
            }
            else
            {
                textBox4.AppendText("The .txt info file is not there");
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            float deg = -45.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Text = button6.Text;
            textBox1.Text = button6.Text;
            changeplotdegrees(deg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            float deg = -30.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button7.Text;
            changeplotdegrees(deg);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            float deg = -15.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button8.Text;
            changeplotdegrees(deg);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            float deg = 0.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button9.Text;
            changeplotdegrees(deg);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            float deg = 15.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button10.Text;
            changeplotdegrees(deg);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            float deg = 30.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button11.Text;
            changeplotdegrees(deg);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            float deg = 45.0f;
            chart1.ChartAreas[0].BackColor = Color.Gainsboro;
            chart2.ChartAreas[0].BackColor = Color.Gainsboro;
            textBox1.Clear();
            textBox1.Text = button12.Text;
            changeplotdegrees(deg);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dcpowerdiff();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            acpowerdiff();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Accuracydcv();
        }
        //private async void writeto plot 
        private void button16_Click(object sender, EventArgs e)
        {
            Accuracyicv();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Accuracywdc();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            acvarvapower();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (!reverse)
            {
                reverse = true;
                button19.BackColor = Color.DarkGreen;
            }
            else
            {
                reverse = false;
                button19.BackColor = Color.LightYellow;
            }
        }
    }
}
