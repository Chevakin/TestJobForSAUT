using System;

namespace TestJobForSAUT
{
    public class Difference
    {
        public string Path { get; private set; }

        public string First { get; private set; }

        public string Second { get; private set; }

        public Difference(string path, string first, string second)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            First = first;
            Second = second;
        }

        public void AddPrefixToPath(string prefix)
        {
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            Path = $"{prefix}.{Path}";
        }
    }
}
