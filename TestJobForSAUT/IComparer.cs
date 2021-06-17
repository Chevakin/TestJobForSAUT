using System.Collections.Generic;

namespace TestJobForSAUT.Comparer
{
    public interface IComparer
    {
        IEnumerable<Difference> Compare<T>(T First, T Second);
    }
}
