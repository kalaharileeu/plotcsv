using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    public class Column
    {
        public string columnname;
        public string alias;
        public string from;
        public int columnnumber;
        public List<string> colvalues;
        List<float> distinctList;
        List<float> floatcolvalues;
        //bool convertedtofloat;

        public Column()
        {
            //convertedtofloat = false;
            floatcolvalues = new List<float>();
        }

        public List<float> DistinctList
        {
            get { return distinctList;}
        }
        /// <summary>
        /// Below just use for DC coltage config table
        /// </summary>
        public void Distinct()
        {
            // Get distinct elements and convert into a list again.
            //List<int> distinct = list.Distinct().ToList();
            distinctList = new List<float>();

            List<string> s = new List<string>(colvalues.Distinct().ToList());
            //s = colvalues.Distinct().ToList();
            foreach(string value in s)
            {
                float f = 0.0f;
                try
                {
                    f = float.Parse(value);
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                distinctList.Add(f);
            }
        }
        /// <summary>
        /// returns the max value between the 2 given floats
        /// </summary>
        /// <param name="rangelist">List of int</param>
        /// <returns>float</returns>
        public float MaxForRange(List<int> rangelist)
        {
            float maxvalue = rangelist[0];

            for(int i = rangelist[0]; i == rangelist[1]; i++ )
            {
                if (rangelist[i] < maxvalue)
                    maxvalue = rangelist[i];
            }
            return maxvalue;
        }

        public void ConvertToFloat()
        {
            if (floatcolvalues.Count == 0)
            {
                //floatcolvalues = new List<float>();
                float f;
                foreach (string value in colvalues)
                {
                    f = 0.0f;
                    try
                    {
                        f = float.Parse(value);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    floatcolvalues.Add(f);
                }
            }
        }

        public float GetMaxValue()
        {
            if (floatcolvalues.Count == 0)
            {
                ConvertToFloat();
            }
            return floatcolvalues.Max();
        }

        //find the range of positions to the requested string
        public List<int> GetUniqueSectionRange(string value)
        {
            List<int> firstllast = new List<int>();
            //gets the List range for this value
            bool found = false;
            for(int i = 0; i < colvalues.Count; i++)
            {
                if (found == true)  
                {
                    if (colvalues[i] != value)
                    {
                        firstllast.Add(i - 1);
                        return firstllast;
                    }
                    //if it is the last element then: end range 
                    if (i == colvalues.Count - 1)
                    {
                        firstllast.Add(i - 1);
                        return firstllast;
                    }
                }
                if (found == false)
                {
                    if (colvalues[i] == value)
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
