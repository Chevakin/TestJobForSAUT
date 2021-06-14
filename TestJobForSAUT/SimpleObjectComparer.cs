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

        private PropertyLayer Layer { get; set; } = PropertyLayer.First;

        public IEnumerable<Difference> Compare<T>(T First, T Second)
        {
            Type = typeof(T);

            props = GetTypeProps(Type);
            CheckProps(props);
        }

        private void CheckProps(IEnumerable<PropertyInfo> properties)
        {
            if (properties.All(p => CheckProp(p)) == false)
            {
                throw new Exception($"Тип {Type.FullName} некорректен, т.к. содержит минимум одно свойство некорректного типа");
            }
        }

        private bool CheckProp(PropertyInfo p)
        {
            if (correctTypes.Contains(p.PropertyType))
                return true;

            if (Layer == PropertyLayer.First)
            {
                Layer = PropertyLayer.Second;

                CheckProps(GetTypeProps(p.PropertyType));

                Layer = PropertyLayer.First;

                return true;
            }

            Layer = PropertyLayer.First;

            return false;
        }

        private HashSet<PropertyInfo> GetTypeProps(Type propertyType)
        {
            return Type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(p => p.CanRead)
                            .ToHashSet();
        }

        private enum PropertyLayer
        {
            First,
            Second,
        }
    }
}
