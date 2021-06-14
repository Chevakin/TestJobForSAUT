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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            Path = path;
            First = first;
            Second = second;
        }

        public void AddPrefixToPath(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            Path = $"{prefix}.{Path}";
        }
    }
}
