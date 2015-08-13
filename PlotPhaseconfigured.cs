using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class PlotPhaseconfigured
    {
        private List<Baselist> columnobjects;
        private Dictionary<float, List<int>> slices;
        //private Dictionary<int, Slice> plotclaslices;  

        public PlotPhaseconfigured(List<Baselist> list)
        {
            columnobjects = new List<Baselist>();
            columnobjects = list;
            CreatSlices();
        }

        private void CreatSlices()
        {
            foreach (Baselist bl in columnobjects)
            {
                if (bl is Phaseconfigured)
                {
                    //downcast as returns reference, is return bool
                    slices = (bl as Phaseconfigured).Listslices;
                    //Create sliced dictionary for Idc
                    try
                    {
                        //Find Vdcconfigure to get slice parameters
                        foreach (Baselist bltwo in columnobjects)
                        {
                            if (bltwo is Vdcconfigured)
                                (bltwo as Vdcconfigured).Setphaseslice(slices);
                        }
                    }
                    catch (InvalidCastException)
                    {

                    }
                }
            }
        }

        public Dictionary<float, List<int>> GetSlices
        {
            get { return slices;}
        }
    }
}
