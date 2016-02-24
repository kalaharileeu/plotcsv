using System;
using System.Collections.Generic;
using System.Linq;

namespace PlotDVT
{
    class VdcconfiguredI : ValuelistI
    {
        private List<string> s; 
        private List<Slice> slicelist;
        private Dictionary<float, List<int>> listslices;
        private int count;

        public VdcconfiguredI(List<string> stringvaluelist, string colname) : base(stringvaluelist, colname)
        {
            slicelist = new List<Slice>();
            listslices = new Dictionary<float, List<int>>();
            s = new List<string>(valuesstring.Distinct().ToList());
            count = 0;
            //Distnct the polupate listslices
            Distinct();
        }

        public Dictionary<float, List<int>> Listslices
        {
            get { return listslices;}
        }

        public List<Slice> Slicelist
        {
            get { return slicelist;}
        }
        /// <summary>
        /// The phaseconfigured has to call this and set it up
        /// This is called from Form1.cs
        /// </summary>
        /// <param name="create phaseslices"></param>
        public void Setphaseslice(Dictionary<float, List<int>> phaseslicestuff)
        {
            foreach (var kv in phaseslicestuff)
            {
                for(int i = 0; i < slicelist.Count; i++)
                {
                    if (slicelist[i].vlist[0] >= kv.Value[0] && slicelist[i].vlist[0] <= kv.Value[1])
                    {
                        if (slicelist[i].vlist[1] <= kv.Value[1])
                        {
                            //populate the phase angle of <Slices> instance
                            slicelist[i].phaseangle = kv.Key;
                        }
                        else
                        {
                            List<int> newparameters = new List<int>() {kv.Value[1] + 1, slicelist[i].vlist[1]};
                            slicelist[i].phaseangle = kv.Key;
                            slicelist[i].vlist[1] = kv.Value[1];
                            //TODO: dodgy +15 very dodgy fix it
                            slicelist.Insert(i + 1, new Slice(kv.Value[1] + 15, slicelist[i].vfloat, newparameters));
                        }
                    }
                }
            }
        }
        //gets the distincr values in the column
        private void Distinct()
        {
            // Get distinct elements and convert into a list again.
            foreach (string value in s)
            {
                float f = 0.0f;
                try
                {
                    f = float.Parse(value);
                    //takes each unique vaule and gets the range
                    listslices.Add(f, GetUniqueSectionRange(value));
                    GetUniqueSectionRange2(value);
                }
                catch (FormatException)
                {
                    f = -1.0f;
                }
            }
        }

        //find the range of positions to the requested string
        private List<int> GetUniqueSectionRange(string value)
        {
            List<int> firstllast = new List<int>();
            //gets the List range for this value
            bool found = false;
            for (int i = count; i < valuesstring.Count; i++)
            {
                if (found == true)
                {
                    if (valuesstring[i] != value)
                    {
                        firstllast.Add(i - 1);
                        count = i;
                        return firstllast;
                    }
                    //if it is the last element then: end range 
                    if (i == valuesstring.Count - 1)
                    {
                        firstllast.Add(i);
                        count = i;
                        //donewithlist = true;
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
            slicelist.Add(new Slice(-1, float.Parse(value), firstllast));
            return firstllast;
        }

        private void GetUniqueSectionRange2(string value)
        {
            List<int> firstllast = new List<int>();
            //gets the List range for this value
            bool foundbool = false;
            int foundcount = 0;
            for (int i = 0; i < valuesstring.Count; i++)
            {
                if (foundbool == true)
                {
                    if (valuesstring[i] != value)
                    {
                        firstllast.Add(i - 1);
                        slicelist.Add(new Slice(i - 1, float.Parse(value), firstllast));
                        foundbool = false;
                    }
                    //if it is the last element then: end range 
                    if (i == valuesstring.Count - 1)
                    {
                        firstllast.Add(i);
                        slicelist.Add(new Slice(i - 1, float.Parse(value), firstllast));
                    }
                }
                if (foundbool == false)
                {
                    if (valuesstring[i] == value)
                    {
                        firstllast = new List<int>();
                        firstllast.Add(i);
                        foundbool = true;
                        foundcount += 1;
                    }
                }
            }
        }   
    }
}
