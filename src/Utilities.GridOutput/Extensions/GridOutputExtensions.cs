using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Utilities.GridOutput.Extensions
{
    public static class GridOutputExtensions
    {
        public static IEnumerable<T> To<T>(this GridOutput instance, Func<T> creationDelegate, Action<T, PropertyDescriptor> mapDelegate)
        {
            if (creationDelegate == null)
            {
                throw new ArgumentNullException(nameof(creationDelegate));
            }
            if (mapDelegate == null)
            {
                throw new ArgumentNullException(nameof(mapDelegate));
            }
            var result = new List<T>();

            foreach (var sourceItem in instance.Data)
            {
                var targetItem = creationDelegate();
                foreach (var property in TypeDescriptor.GetProperties(targetItem).Cast<PropertyDescriptor>().ToArray())
                {
                    var index = instance.ColumnNames
                        .Select((name, i) => new { name, i })
                        .First(a => a.name == property.Name);
                    var cell = sourceItem.Cells.ElementAtOrDefault(index.i);
                    if (cell == null)
                    {
                        throw new ArgumentException($"Error: GridOutput instance does not have cell {index}");
                    }

                    mapDelegate(targetItem, property);
                }

                result.Add(targetItem);
            }

            return result;
        }

        public static IEnumerable<T> To<T>(this GridOutput instance, Func<GridOutputRow, T> mapDelegate)
            => instance.Data.Select(mapDelegate);

        public static IEnumerable<T> To<T>(this GridOutput instance, Func<PropertyDescriptor, object> customConverterDelegate = null) where T : class, new()
        {
            var result = new List<T>();

            // First, check if all fields are present
            var propertyDescriptorCollection = TypeDescriptor.GetProperties(new T()).Cast<PropertyDescriptor>().ToArray();
            var missingFields = instance.ColumnNames.Where(name => !propertyDescriptorCollection.Any(p => p.Name == name)).ToArray();
            if (missingFields.Length > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(instance), $"The following properties from the GridOutput instance were not found in type [{typeof(T).FullName}]: {string.Join(", ", missingFields)}");
            }

            // If everything is okay, loop through the grid output, and fill the result
            foreach (var sourceItem in instance.Data)
            {
                var targetItem = new T();
                foreach (var property in propertyDescriptorCollection)
                {
                    var index = instance.ColumnNames
                        .Select((name, i) => new { name, i })
                        .First(a => a.name == property.Name);

                    var cell = sourceItem.Cells.ElementAtOrDefault(index.i);
                    if (cell == null)
                    {
                        throw new ArgumentException($"Error: GridOutput instance does not have cell {index}");
                    }

                    property.SetValue(targetItem, cell.Value.ConvertType(property.PropertyType, property, customConverterDelegate));
                }

                result.Add(targetItem);
            }

            return result;
        }
    }
}
