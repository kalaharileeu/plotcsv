using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class PlotIdcconfigured
    {
        private List<Baselist> columnobjects;
        private float ave;
        private float max;
        private float min;
        private Dictionary<float, List<float>> slices;

        public PlotIdcconfigured(List<Baselist> list)
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
                if (bl is Idcconfigured)
                {
                    //downcast as returns reference, is return bool
                    ave = (bl as Idcconfigured).GetAverage;
                    max = (bl as Idcconfigured).GetMax;
                    min = (bl as Idcconfigured).GetMin;
                    slices = (bl as Idcconfigured).Slices;
                    //Create sliced dictionary for Idc
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            if (bltwo is Vdcconfigured)
                                (bl as Idcconfigured).Populareslices((bltwo as Vdcconfigured).Listslices);
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
