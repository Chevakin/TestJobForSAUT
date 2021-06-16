using System;
using System.Text;

namespace TestJobForSAUT
{
    public class Difference
    {
        private readonly StringBuilder path;

        public string Path => path.ToString();

        public string First { get; private set; }

        public string Second { get; private set; }

        public Difference(string path, string first, string second)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            this.path = new StringBuilder(path);
            this.First = first;
            this.Second = second;
        }

        public void AddPrefixToPath(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            path.Insert(0, $"{prefix}.");
        }
    }
}
