﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    class Vdcpcu : Valuelist
    {
        private Dictionary<float, List<float>> slices;

        public Vdcpcu(List<string> stringvaluelist) : base(stringvaluelist)
        {
            slices = new Dictionary<float, List<float>>();
        }

        public Dictionary<float, List<float>> Slices 
        {
            get{ return slices; }
        }

        public void Populareslices(Dictionary<float, List<int>> slicedvalues)
        {
            foreach (KeyValuePair<float, List<int>> kv in slicedvalues)
            {
                slices.Add(kv.Key, valuesfloat.GetRange(kv.Value[0], kv.Value[1] - kv.Value[0]));
            }
        }

        public void Populareslices(List<Slice> slice)
        {
            foreach (Slice s in slice)
            {
                if (s.phaseangle == 0.0f)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

        public void Populareslices(List<Slice> slice, float deg)
        {
            if (slices.Count > 0)
                slices.Clear();
            foreach (Slice s in slice)
            {
                if (s.phaseangle == deg)
                    slices.Add(s.vfloat, valuesfloat.GetRange(s.vlist[0], s.vlist[1] - s.vlist[0]));
            }
        }

        public new float GetAverage
        {
            get { return average; }
        }

        public new float GetMax
        {
            get { return maxvalue; }
        }

        public new float GetMin
        {
            get { return minvalue; }
        }

        public new List<float> GetFloats
        {
            get { return valuesfloat; }
        }
    }
}
