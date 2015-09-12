using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Plotwdcpowermeter
    {
        private List<Baselist> columnobjects;
        private float ave;
        private float max;
        private Dictionary<float, List<float>> slices;

        public Plotwdcpowermeter(List<Baselist> list)
        {
            columnobjects = new List<Baselist>();
            columnobjects = list;
            CreatSlices();
        }

        private void CreatSlices()
        {
            foreach (Baselist bl in columnobjects)
            {
                if (bl is Wdcpowermeter)
                {
                    slices = bl.GetSlices();
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            if (bltwo is Vdcconfigured)
                                bl.Populareslices((bltwo as Vdcconfigured).Slicelist);
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
                if (bl is Wdcpowermeter)
                {
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            if (bltwo is Vdcconfigured)
                                bl.Populareslices((bltwo as Vdcconfigured).Slicelist, deg);
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

        //public float GetMin
        //{
        //    get { return min;}
        //}

        public float GetAverage
        {
            get { return ave;}
        }
    }
}
