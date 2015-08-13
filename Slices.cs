using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotDVT
{
    public class Slice
    {
        public int key;
        public float vfloat;
        public float phaseangle;
        public List<int> vlist;

        public Slice(int k, float vf, List<int> vl)
        {
            vlist = new List<int>();
            vlist = vl;
            vfloat = vf;
            key = k;
            phaseangle = -1.0f;
        }

        public Slice(int k, float vf, List<int> vl, float fl)
        {
            vlist = new List<int>();
            vlist = vl;
            vfloat = vf;
            key = k;
            phaseangle = fl;
        }

        public void Setphaseangle(float pa)
        {
            phaseangle = pa;
        }
    }
}
