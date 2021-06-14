using System.Collections.Generic;

namespace TestJobForSAUT
{
    public interface IComparer
    {
        IEnumerable<Difference> Compare<T>(T First, T Second);
    }
}
