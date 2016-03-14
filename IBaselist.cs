using System.Collections.Generic;

namespace PlotDVT
{
    /// <summary>
    /// ValuelistI implement this
    /// </summary>
    interface IBaselist
    {
        void Populareslices(Dictionary<float, List<int>> slicedvalues);
        void Populareslices(List<Slice> slice);
        void Populareslices(List<Slice> slice, float deg);
        Dictionary<float, List<float>> GetSlices();
        Dictionary<float, List<float>> GetSlices2();
        void ConvertToFloat();
        List<float> GetFloats();
        string GetName();
    }
}