using System;
using System.Collections.Generic;

namespace PlotDVT
{
    /// <summary>
    /// This might still have to be a singleton or some static helper class
    /// The class should getthe fullscale values and the Pnominal and Vnominal
    /// from either the user inteface or database
    /// </summary>
    public class Calculate
    {
        //retuns a list of values sutracted from each other
        public static List<float> Difflist(List<float> target, List<float> measument)
        {
            if (target.Count == measument.Count)
            {
                List<float> results = new List<float>();
                for (int i = 0; i < target.Count; i++)
                {
                    results.Add(target[i] - measument[i]);
                }
                return results;
            }
            //return target unchanged if not same length as measurement
            return target;
        }
        //Creates the persentage of reading list
        public static List<float> persentofmeasuement(List<float> measurement, float persenetage)
        {
            List<float> persentofmeasurementlist = new List<float>();
            if (measurement.Count > 0 && measurement != null)
            {
                for (int i = 0; i < measurement.Count; i++)
                {
                    persentofmeasurementlist.Add((Math.Abs(measurement[i]) / 100) * persenetage);
                } 
            }
            return persentofmeasurementlist;
        }
        //creates the persentage of Fullscale limit limit
        public static List<float> persentofFS(List<float> target, float FS, float persentageofFS)
        {
            List<float> persentofFSlist = new List<float>();
            if (target.Count > 0 && target != null)
            {
                for (int i = 0; i < target.Count; i++)
                {
                    persentofFSlist.Add((FS / 100) * persentageofFS);
                }
            }
            return persentofFSlist;
        }
        //Takes two limits and add them together for the final limit value
        public static List<float> limitlist(List<float> persentofmeasurementlist, List<float> persentofFS)
        {
            List<float> limitlist = new List<float>();
            if ((persentofFS.Count > 0 && persentofFS != null)
                && (persentofmeasurementlist.Count > 0 && persentofmeasurementlist != null)
                && (persentofmeasurementlist.Count == persentofFS.Count))
            {
                for (int i = 0; i < persentofmeasurementlist.Count; i++)
                {
                    limitlist.Add(persentofmeasurementlist[i] + persentofFS[i]);
                }
            }
            return limitlist;
        }
        /// <summary>
        /// return a bool list of all the values that failed
        /// </summary>
        /// <param name="target">pcu values</param>
        /// <param name="measurement">powermeter</param>
        /// <param name="persentofmeasure">the margin available</param>
        /// <param name="FS">FS value from GUI</param>
        /// <param name="persentageofFS">persentage of FS</param>
        /// <returns></returns>
        public static List<bool> Faillist(List<float> target, 
            List<float> measurement, int persentofmeasure, float FS, float persentageofFS)
        {
            List<bool> passfaillist = new List<bool>();
                if (target.Count > 0 && target != null)
                {
                List<float> difflist = new List<float>(Difflist(target, measurement));
                List<float> thelimitlist = new List<float>(limitlist(persentofmeasuement(measurement, persentofmeasure),
                    persentofFS(target, FS, persentageofFS)));
                    for (int i = 0; i < difflist.Count; i++)
                    {
                    if (Math.Abs(difflist[i]) <= thelimitlist[i])
                        passfaillist.Add(true);
                    else
                        passfaillist.Add(false);
                    }
                }
            return passfaillist;
        }

        /// <summary>
        /// Works out the failure just on persentag between power meter and the inverter does
        /// not use the fullscale values
        /// </summary>
        /// <param name="target"></param>
        /// <param name="measurement"></param>
        /// <param name="persentofmeasure"></param>
        /// <returns></returns>
        //public static List<bool> Pesentage_faillist(List<float> target, List<float> measurement, int persentofmeasure, float FS, float persentageofFS)
        public static List<bool> Pesentage_faillist(List<float> target, List<float> measurement, float persentofmeasure)
        {
            List<bool> passfaillist = new List<bool>();
                if (target.Count > 0 && target != null)
                {
                    //gets the difference pcu and powermeter measurment
                    List<float> difflist = new List<float>(Difflist(target, measurement));
                    List<float> thelimitlist = persentofmeasuement(measurement, persentofmeasure);
                    for (int i = 0; i < difflist.Count; i++)
                    {
                        if (Math.Abs(difflist[i]) <= thelimitlist[i])
                            passfaillist.Add(true);
                        else
                            passfaillist.Add(false);
                    }
                }
                return passfaillist;
        }
        /// <summary>
        /// works out the apparant ac current the the pcu read and feed is ba
        /// </summary>
        /// <param name="pcuimag"></param>
        /// <param name="pcureal"></param>
        /// <returns>list of pcu apparant current</returns>
        public static List<float> Get_pcu_apparantcurrent(List<float> pcuimag, List<float> pcureal)
        {
            List<float> pcu_apparant_i = new List<float>();
            if (pcuimag.Count == pcureal.Count)
            {
                for (int i = 0; i < pcuimag.Count; i++)
                {
                    pcu_apparant_i.Add((float)(Math.Sqrt(
                        Convert.ToDouble(pcuimag[i] * pcuimag[i] + pcureal[i] * pcureal[i]))));
                }
                return pcu_apparant_i;
            }
            //return null; the two list values not equal
            return null;
        }




        // public static List<int> Getparsentagelist()
        //{
        //    List<int> v = new List<int>();
        //    return v;
        //}
        ////convert a value to float
        //public void ConvertToFloat()
        //{
        //    //floatcolvalues = new List<float>();
        //    float f;
        //    foreach (string value in valuesstring)
        //    {
        //        f = 0.0f;
        //        if ((value is string) && (value.Length > 0))
        //        {
        //            try
        //            {
        //                f = float.Parse(value);
        //            }
        //            catch (FormatException e)
        //            {
        //                Console.WriteLine(e.Message);
        //            }
        //        }
        //        valuesfloat.Add(f);
        //    }
        //}
    }
}
