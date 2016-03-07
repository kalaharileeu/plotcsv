using System.Collections.Generic;

namespace PlotDVT
{
    interface ICalculate
    {
        List<float> Difflist(List<float> a, List<float> b);
        List<int> Getparsentagelist();
        List<bool> Faillist(List<int> percentage, int percentagelimit);
    }
}
