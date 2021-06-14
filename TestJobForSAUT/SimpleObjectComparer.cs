using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TestJobForSAUT
{
    public class SimpleObjectComparer : IComparer
    {
        private HashSet<PropertyInfo> props;

        public IEnumerable<Difference> Compare<T>(T First, T Second)
        {
            props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToHashSet();
        }
    }
}
