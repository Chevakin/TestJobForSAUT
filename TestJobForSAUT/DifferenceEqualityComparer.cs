using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJobForSAUT.Comparer
{
    public class DifferenceEqualityComparer : IEqualityComparer<Difference>
    {
        public bool Equals([AllowNull] Difference x, [AllowNull] Difference y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] Difference obj)
        {
            return obj.GetHashCode();
        }
    }
}
