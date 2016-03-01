using System;
using System.Collections.Generic;
using System.Linq;

namespace PlotDVT
{
    class PhaseconfiguredI : ValuelistI
    {
        //List<float> distinctlist;
        Dictionary<float, List<int>> listslices;
        //for the second set of values
        Dictionary<float, List<int>> listslices2;

        public PhaseconfiguredI(List<string> stringvaluelist, string colname) : base(stringvaluelist, colname)
        {
            //valuesstring = stringvaluelist;
            listslices = new Dictionary<float, List<int>>();
            listslices2 = new Dictionary<float, List<int>>();
            Distinct();
        }
        //returns the slices of the degrees and their positions
        public Dictionary<float, List<int>> Listslices
        {
            get { return listslices; }
        }

        //returns the slices2 of the degrees and their positions
        public Dictionary<float, List<int>> Listslices2
        {
            get { return listslices2; }
        }

        private void Distinct()
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
                catch (FormatException )
                {
                    f = -2.0f;
                }
                //takes each unique value and gets the range
                listslices.Add(f, GetUniqueSectionRange(value));
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
                    //if value != the searched past reverse 1
                    if (valuesstring[i] != value)
                    {
                        firstllast.Add(i - 1);
                        //Find the second set of values
                        GetUniqueSectionRange2(value, i - 1);
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
        //Get the second range of data in the list
        //Below is a shortened version of get unique, better the 1st version top
        public void GetUniqueSectionRange2(string value, int start)
        {
            List<int> firstllast = new List<int>();
            //gets the List range for this value
            bool found = false;
            for (int i = start + 1; i < valuesstring.Count; i++)
            {
                if (found == false)
                {
                    if (valuesstring[i] == value)
                    {
                        firstllast.Add(i);
                        found = true;
                        firstllast.Add(valuesstring.LastIndexOf(value));
                        listslices2.Add(float.Parse(value), firstllast);
                        return;
                    }
                }
            }
            return;
        }
    }
}
