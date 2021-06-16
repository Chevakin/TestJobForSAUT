using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestJobForSAUT
{
    public class SimpleObjectComparer : IComparer
    {
        private readonly HashSet<Type> correctTypes = new HashSet<Type>
        {
            typeof(string),
            typeof(int),
            typeof(double)
        };

        public IEnumerable<Difference> Compare<T>(T first, T second)
        {
            var type = typeof(T);

            if (first == null)
                throw new ArgumentNullException(nameof(first));
            else if (second == null)
                throw new ArgumentNullException(nameof(second));
            else if (first.GetType() != type || second.GetType() != type)
                throw new ArgumentException($"{nameof(first)} и {nameof(second)} должны принадлежать типу {type.FullName}");

            var props = GetTypeProps(type);

            if (CheckProperties(props) == false)
                throw new Exception($"Тип {type.FullName} некорректен, т.к. содержит минимум одно свойство некорректного типа");

            return GetDifference(first, second, props);
        }

        private IEnumerable<Difference> GetDifference<T>(T first, T second, IEnumerable<PropertyInfo> properties)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            else if (second == null)
                throw new ArgumentNullException(nameof(second));

            IEnumerable<Difference> differences = new HashSet<Difference>();

            foreach (var prop in properties)
            {
                var firstProp = prop.GetValue(first);
                var secondProp = prop.GetValue(second);

                var difference = GetDifference(firstProp, secondProp, prop.Name).ToArray();

                if (difference != null)
                {
                    differences.Concat(difference);
                }
            }

            return new Difference[0];
        }

        private IEnumerable<Difference> GetDifference(object firstPropValue, object secondPropValue, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            var type = firstPropValue.GetType();

            if (correctTypes.Contains(type))
            {
                //Тут могут возникнуть проблемы со сравнением double
                if (firstPropValue.Equals(secondPropValue) == false)
                {
                    return new Difference[] { new Difference(propertyName, firstPropValue.ToString(), secondPropValue.ToString()) };
                }
            }

            var differences = GetDifference(firstPropValue, secondPropValue, GetTypeProps(type)).ToArray();

            foreach(var difference in differences)
            {
                difference.AddPrefixToPath(propertyName);
            }

            return differences;
        }

        private bool CheckProperties(IEnumerable<PropertyInfo> properties)
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

            if (properties == null || properties.Count() == 0)
            {
                return false;
            }

            return CheckProperties(properties);
        }

        private IEnumerable<PropertyInfo> GetTypeProps(Type propertyType)
        {
            return propertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Where(p => p.CanRead)
                            .ToArray();
        }
    }
}
