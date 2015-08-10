using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace PlotDVT
{
    class PlotIdcpowermeter
    {
        private List<Baselist> columnobjects;
        private float ave;
        private float max;
        private float min;
        private Dictionary<float, List<float>> slices; 

        public PlotIdcpowermeter(List<Baselist> list)
        {
            columnobjects = new List<Baselist>();
            columnobjects = list;
            CreatSlices();
        }

        private void CreatSlices()
        {
            foreach (Baselist bl in columnobjects)
            {
                //string value = ((V.GetType()).ToString()).Replace("PlotDVT.", "");
                //string value = ((bl.GetType()).ToString());
                if (bl is Idcpowermeter)
                {
                    //downcast as returns reference, is return bool
                    ave = (bl as Idcpowermeter).GetAverage;
                    max = (bl as Idcpowermeter).GetMax;
                    min = (bl as Idcpowermeter).GetMin;
                    slices = (bl as Idcpowermeter).Slices;
                    //Create sliced dictionary for Idc
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            if (bltwo is Vdcconfigured)
                                (bl as Idcpowermeter).Populareslices((bltwo as Vdcconfigured).Listslices);
                        }
                    }
                    catch (InvalidCastException)
                    {

                    }
                }
            }
        }

        public Dictionary<float, List<float>> GetSlices
        {
            get { return slices;}
        }

        public float GetMax
        {
            get { return max; }
        }

        public float GetMin
        {
            get { return min;}
        }

        public float GetAverage
        {
            get { return ave;}
        }

    }
}
