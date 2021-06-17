using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestJobForSAUT
{
    public class SimpleObjectComparer : IComparer
    {
        private readonly HashSet<Type> simpleTypes = new HashSet<Type>
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

            var props = GetTypeProps(type);

            if (CheckProperties(props) == false)
                throw new Exception($"Тип {type.FullName} некорректен, т.к. содержит минимум одно свойство некорректного типа");

            return GetDifference(first, second, props);
        }

        private IEnumerable<Difference> GetDifference<T>(T first, T second, IEnumerable<PropertyInfo> properties)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            IEnumerable<Difference> differences = new HashSet<Difference>();

            foreach (var prop in properties)
            {
                var firstProp = prop.GetValue(first);
                var secondProp = prop.GetValue(second);

                //В ТЗ не указано, что делать, если как минимум одно из значений свойств null
                if (firstProp == null || secondProp == null)
                    throw new Exception("Значение как минимум одного из свойств null");
               
                var difference = GetDifference(firstProp, secondProp, prop.Name).ToArray();

                if (difference != null && difference.Count() != 0)
                {
                    differences.Concat(difference);
                }
            }

            return differences;
        }

        private IEnumerable<Difference> GetDifference(object firstPropValue, object secondPropValue, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            if (firstPropValue == null)
                throw new ArgumentNullException(nameof(firstPropValue));
            if (secondPropValue == null)
                throw new ArgumentNullException(nameof(secondPropValue));

            var type = firstPropValue.GetType();

            if (simpleTypes.Contains(type))
            {
                //В ТЗ не указано, как сравнивать double
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
            if (simpleTypes.Contains(p.PropertyType))
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
