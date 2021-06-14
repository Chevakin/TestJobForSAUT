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

        private readonly HashSet<Type> correctTypes = new HashSet<Type>
        {
            typeof(string),
            typeof(int),
            typeof(double)
        };

        private Type Type { get; set; }

        public IEnumerable<Difference> Compare<T>(T First, T Second)
        {
            Type = typeof(T);

            props = GetTypeProps(Type);

            if (CheckProps(props) == false)
                throw new Exception($"Тип {Type.FullName} некорректен, т.к. содержит минимум одно свойство некорректного типа");


        }

        private bool CheckProps(IEnumerable<PropertyInfo> properties)
        {
            return properties.All(p => CheckProp(p));
        }

        private bool CheckProp(PropertyInfo p)
        {
            if (correctTypes.Contains(p.PropertyType))
            {
                return true;
            }

            var properties = GetTypeProps(p.PropertyType);

            if (properties == null || properties.Count == 0)
            {
                return false;
            }

            return CheckProps(properties);
        }

        private HashSet<PropertyInfo> GetTypeProps(Type propertyType)
        {
            return Type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(p => p.CanRead)
                            .ToHashSet();
        }
    }
}
