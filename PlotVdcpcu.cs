using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class PlotVdcpcu
    {
        private List<Baselist> columnobjects;
        private float ave;
        private float max;
        private float min;
        private Dictionary<float, List<float>> slices;

        public PlotVdcpcu(List<Baselist> list)
        {
            columnobjects = new List<Baselist>();
            columnobjects = list;
            CreatSlices();
        }

        private void CreatSlices()
        {
            foreach (Baselist bl in columnobjects)
            {
                if (bl is Vdcpcu)
                {

                    slices = bl.GetSlices();
                    //Create sliced dictionary for Idc
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            //Vdc congfigured is need in all the plot classes
                            if (bltwo is Vdcconfigured)
                                (bl as Vdcpcu).Populareslices((bltwo as Vdcconfigured).Slicelist);
                        }
                    }
                    catch (InvalidCastException)
                    {

                    }
                }
            }
        }

        public void CreatSlices(float deg)
        {
            foreach (Baselist bl in columnobjects)
            {
                if (bl is Vdcpcu)
                {
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            if (bltwo is Vdcconfigured)
                                (bl as Vdcpcu).Populareslices((bltwo as Vdcconfigured).Slicelist, deg);
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
