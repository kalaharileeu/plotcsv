using Novacode;


namespace PlotDVT
{
    partial class Form1
    {
        public void CreateSampleDocument()
        {
            // Modify to suit your machine:
            string fileName = @"C:\temp\DocXExample4.docx";
            // Create a document in memory:
            var doc = DocX.Create(fileName);
       
//2nd subheading formatting
            Formatting heading2 = new Formatting();
            heading2.Bold = false;
            heading2.Size = 16;
            heading2.Bold = true;
            heading2.UnderlineStyle = UnderlineStyle.singleLine;
            heading2.FontColor = System.Drawing.Color.DarkOrange;
            Paragraph empty = doc.InsertParagraph("", false);
//Main Heading
            Formatting headingformat = new Formatting();
            headingformat.Bold = true;
            headingformat.Size = 48;
            headingformat.FontColor = System.Drawing.Color.DarkOrange;
            Paragraph heading = doc.InsertParagraph("Sequencer results compare.", false, headingformat);
            heading.Color(System.Drawing.Color.DarkOrange);
            doc.InsertParagraph(" ", false);
            doc.InsertParagraph(" ", false);
//Foreword second heading
            //Paragraph headingsub = doc.InsertParagraph("UUT under test information.", false, heading2);
//Foreword formating
            Formatting foreword = new Formatting();
            foreword.Bold = false;
            foreword.Size = 12;
            foreword.FontColor = System.Drawing.Color.Black;
            Paragraph headingsub = doc.InsertParagraph("1.1 UUT under test information.", false, heading2);
            headingsub.Heading(HeadingType.Heading2);
            Paragraph forworduut = doc.InsertParagraph(uutdetail, false, foreword);
            //Foreword baseline heading 
            empty = doc.InsertParagraph("", false);
            //Novacode.HeadingType.Heading1 
            Paragraph headingbaseline = doc.InsertParagraph("1.2 Baseline test information.", false, heading2);
            Paragraph forwordbasline = doc.InsertParagraph(baselinedetail, false, foreword);
//new page
            doc.InsertSectionPageBreak(false);
//new page
            //Efficiency compare Images of
            Novacode.Image img = doc.AddImage(@"C:\temp\Efficiency.jpg");
            // Insert a paragrpah:
            Paragraph p0 = doc.InsertParagraph
                ("1.3 The sum of the difference in efficiency of the two unit under test.", false, heading2);
//Efficiency compare Images of
            Paragraph p = doc.InsertParagraph(" ", false);
            p.AppendLine("The Orange marker are the sum of the defference between the baseline unit and the UUT.");
            p.Append("Every orange marker indicates a differet phase angle. ");
            p.Append("If there is a red cross the it means the total sum was higher than ");
//#region
#region pic1
            Picture pic1 = img.CreatePicture();
            int w = 0;
            p.InsertPicture(pic1, w);
#endregion
//This is the Idc comapre region
            Novacode.Image imgIdc0 = doc.AddImage(@"C:\temp\DC current0°.jpg");
            Novacode.Image imgIdcn45 = doc.AddImage(@"C:\temp\DC current-45°.jpg");
            Novacode.Image imgIdcn30 = doc.AddImage(@"C:\temp\DC current-30°.jpg");
            Novacode.Image imgIdcn15 = doc.AddImage(@"C:\temp\DC current-15°.jpg");
            Novacode.Image imgIdc15 = doc.AddImage(@"C:\temp\DC current15°.jpg");
            Novacode.Image imgIdc30= doc.AddImage(@"C:\temp\DC current30°.jpg");
            Novacode.Image imgIdc45 = doc.AddImage(@"C:\temp\DC current45°.jpg");
            // Insert a paragrpah:
            doc.InsertParagraph("");
            doc.InsertParagraph("1.4 Comparative plots of the DC current at every phase angle.", false, heading2);
            doc.InsertParagraph("");
            Paragraph p3 = doc.InsertParagraph("The baseline values are in black and the orange crosses are the UUT.");
            p3.AppendLine("If the is a red cross in the graph then the reported measurement was below 0.1A,");
            p3.Append("this means that the inverter probably did not start up at that point.");
            p3.AppendLine("");
            p3.AppendLine("");
            //Efficiency compare Images of
            doc.InsertParagraph("", false);
            //Efficiency picture area here
            Paragraph p2 = doc.InsertParagraph(" ", false);
#region picidc
            Picture picIdc0 = imgIdc0.CreatePicture();
            Picture picIdcn45 = imgIdcn45.CreatePicture();
            Picture picIdcn30 = imgIdcn30.CreatePicture();
            Picture picIdcn15 = imgIdcn15.CreatePicture();
            Picture picIdc15 = imgIdc15.CreatePicture();
            Picture picIdc30 = imgIdc30.CreatePicture();
            Picture picIdc45 = imgIdc45.CreatePicture();
            //int w1 = 0;
            p2.InsertPicture(picIdc0, 0);
            p2.InsertPicture(picIdcn45, 0);
            p2.InsertPicture(picIdcn30, 0);
            p2.InsertPicture(picIdcn15, 0);
            p2.InsertPicture(picIdc15, 0);
            p2.InsertPicture(picIdc30, 0);
            p2.InsertPicture(picIdc45, 0);
#endregion
//Vdc accuracy
            Novacode.Image imgvdcrep0 = doc.AddImage(@"C:\temp\Vdc input0°.jpg");
            Novacode.Image imgvdcrep45 = doc.AddImage(@"C:\temp\Vdc input45°.jpg");
            Novacode.Image imgvdcrep30 = doc.AddImage(@"C:\temp\Vdc input30°.jpg");
            Novacode.Image imgvdcrep15 = doc.AddImage(@"C:\temp\Vdc input15°.jpg");
            Novacode.Image imgvdcrepn15 = doc.AddImage(@"C:\temp\Vdc input-15°.jpg");
            Novacode.Image imgvdcrepn30 = doc.AddImage(@"C:\temp\Vdc input-30°.jpg");
            Novacode.Image imgvdcrepn45 = doc.AddImage(@"C:\temp\Vdc input-45°.jpg");
            // Insert a paragrpah:
            doc.InsertParagraph("");
            doc.InsertParagraph("1.5 DC voltage accuracy at every phase angle.", false, heading2);
            doc.InsertParagraph("");
            Paragraph p4 = doc.InsertParagraph("The baseline values are in black and the orange crosses are the UUT.");
            p4.AppendLine("");
            p4.AppendLine("");
            //Efficiency compare Images of
            doc.InsertParagraph("", false);
            Paragraph p5 = doc.InsertParagraph(" ", false);
//# region vdc accuracy region
#region picvdcacc
            p5.InsertPicture(imgvdcrep0.CreatePicture());
            p5.InsertPicture(imgvdcrep45.CreatePicture());
            p5.InsertPicture(imgvdcrep30.CreatePicture());
            p5.InsertPicture(imgvdcrep15.CreatePicture());
            p5.InsertPicture(imgvdcrepn15.CreatePicture());
            p5.InsertPicture(imgvdcrepn30.CreatePicture());
            p5.InsertPicture(imgvdcrepn45.CreatePicture());
#endregion
 //Idc accuracy
            Novacode.Image imgidcrep0 = doc.AddImage(@"C:\temp\Idc input0°.jpg");
            Novacode.Image imgidcrep45 = doc.AddImage(@"C:\temp\Idc input45°.jpg");
            Novacode.Image imgidcrep30 = doc.AddImage(@"C:\temp\Idc input30°.jpg");
            Novacode.Image imgidcrep15 = doc.AddImage(@"C:\temp\Idc input15°.jpg");
            Novacode.Image imgidcrepn15 = doc.AddImage(@"C:\temp\Idc input-15°.jpg");
            Novacode.Image imgidcrepn30 = doc.AddImage(@"C:\temp\Idc input-30°.jpg");
            Novacode.Image imgidcrepn45 = doc.AddImage(@"C:\temp\Idc input-45°.jpg");
            // Insert a paragrpah:
            doc.InsertParagraph("");
            doc.InsertParagraph("DC current accuracy at every phase angle.", false, heading2);
            doc.InsertParagraph("");
            doc.InsertParagraph("The baseline values are in black and the orange crosses are the UUT.");
            doc.InsertParagraph("");
            doc.InsertParagraph("");
            //Efficiency compare Images of
            doc.InsertParagraph("", false);
            Paragraph p7 = doc.InsertParagraph(" ", false);
//# region vdc accuracy region
#region picidcacc
            p7.InsertPicture(imgidcrep0.CreatePicture());
            p7.InsertPicture(imgidcrep45.CreatePicture());
            p7.InsertPicture(imgidcrep30.CreatePicture());
            p7.InsertPicture(imgidcrep15.CreatePicture());
            p7.InsertPicture(imgidcrepn15.CreatePicture());
            p7.InsertPicture(imgidcrepn30.CreatePicture());
            p7.InsertPicture(imgidcrepn45.CreatePicture());
#endregion


            
            // Save to the output directory:
            doc.Save();

            // Open in Word:
            //Process.Start("WINWORD.EXE", fileName);
        }
    }
}
