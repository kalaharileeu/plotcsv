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

         public static List<int> Getparsentagelist()
        {
            List<int> v = new List<int>();
            return v;
        }
    }
}
