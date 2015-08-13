using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Phaseconfigured  : Valuelist
    {
        //List<float> distinctlist;
        Dictionary<float, List<int>> listslices;

        public Phaseconfigured(List<string> stringvaluelist) : base(stringvaluelist)
        {
            //valuesstring = stringvaluelist;
            listslices = new Dictionary<float, List<int>>();
            Distinct();
        }

        public Dictionary<float, List<int>> Listslices
        {
            get { return listslices;}
        }

        public void Distinct()
        {
            // Get distinct elements and convert into a list again.
            //distinctlist = new List<float>();
            List<string> s = new List<string>(valuesstring.Distinct().ToList());
            //s = colvalues.Distinct().ToList();
            foreach (string value in s)
            {
                float f = 0.0f;
                try
                {
                    f = float.Parse(value);
                }
                catch (FormatException e)
                {
                    f = -2.0f;
                }
                //takes each unique vaule and gets the range
                listslices.Add(f,GetUniqueSectionRange(value));
            }
        }

        //find the range of positions to the requested string
        public List<int> GetUniqueSectionRange(string value)
        {
            List<int> firstllast = new List<int>();
            //gets the List range for this value
            bool found = false;
            for (int i = 0; i < valuesstring.Count; i++)
            {
                if (found == true)
                {
                    if (valuesstring[i] != value)
                    {
                        firstllast.Add(i - 1);
                        return firstllast;
                    }
                    //if it is the last element then: end range 
                    if (i == valuesstring.Count - 1)
                    {
                        firstllast.Add(i);
                        return firstllast;
                    }
                }
                if (found == false)
                {
                    if (valuesstring[i] == value)
                    {
                        firstllast.Add(i);
                        found = true;
                    }
                }
            }
            return firstllast;
        }   
    }
}
