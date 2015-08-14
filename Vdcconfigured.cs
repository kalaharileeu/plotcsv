using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Vdcconfigured : Valuelist
    {

        private List<string> s; 
        private List<Slice> slicelist;
        //private Dictionary<float, List<int>> phaseslicedict;
        private Dictionary<float, List<int>> listslices;
        //private bool donewithlist;
        private int count;

        public Vdcconfigured(List<string> stringvaluelist) : base(stringvaluelist)
        {
            slicelist = new List<Slice>();
            listslices = new Dictionary<float, List<int>>();
            //phaseslicedict = new Dictionary<float, List<int>>();
            s = new List<string>(valuesstring.Distinct().ToList());
            //donewithlist = false;
            count = 0;
            Distinct();
        }

        override public void Populareslices(Dictionary<float, List<int>> slicedvalues)
        {
            //override the inherited function, do nothing
        }

        override public void Populareslices(List<Slice> slice)
        {
            //override the inherited function, do nothing
        }

        override public void Populareslices(List<Slice> slice, float deg)
        {
            //override the inherited function, do nothing
        }

        public Dictionary<float, List<int>> Listslices
        {
            get { return listslices;}
        }

        public List<Slice> Slicelist
        {
            get { return slicelist;}
        }

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
                            slicelist[i].phaseangle = kv.Key;
                        }
                        else
                        {
                            List<int> newparameters = new List<int>() {kv.Value[1] + 1, slicelist[i].vlist[1]};
                            slicelist[i].phaseangle = kv.Key;
                            slicelist[i].vlist[1] = kv.Value[1];
                            //dodgy +15 very dodgy
                            slicelist.Insert(i + 1, new Slice(kv.Value[1] + 15, slicelist[i].vfloat, newparameters));
                        }
                    }

                }
            }
        }

        public void Distinct()
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
                catch (FormatException e)
                {
                    f = -1.0f;
                }
            }
        }


        //find the range of positions to the requested string
        public List<int> GetUniqueSectionRange(string value)
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

        public void GetUniqueSectionRange2(string value)
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
