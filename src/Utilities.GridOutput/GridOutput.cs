using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities.GridOutput
{
    public class GridOutput
    {
        public IEnumerable<string> ColumnNames { get; set; }
        public IEnumerable<GridOutputRow> Data { get; set; }

        public static GridOutput FromString(string tabDelimitedValue)
        {
            string[] columnNames = null;
            var returnValue = new Collection<GridOutputRow>();
            using (var reader = new StringReader(tabDelimitedValue))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split('\t');
                    if (columnNames == null)
                    {
                        columnNames = data;
                    }
                    else
                    {
                        returnValue.Add(new GridOutputRow
                        {
                            Cells = data.Select(s => new GridOutputCell
                            {
                                Value = s == "[[NULL]]"
                                    ? null
                                    : s.Replace("[[TAB]]", "\t")
                            })
                        });
                    }
                }
            }

            return new GridOutput { ColumnNames = columnNames, Data = returnValue };
        }

        public static GridOutput Create(IEnumerable source, CultureInfo cultureInfo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var columnNames = new Collection<string>();
            var data = new Collection<GridOutputRow>();
            var firstRow = true;
            IEnumerable<PropertyDescriptor> propertyDescriptorCollection = null;

            foreach (var item in source)
            {
                if (firstRow)
                {
                    propertyDescriptorCollection = TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>().ToArray();
                    foreach (var prop in propertyDescriptorCollection)
                    {
                        columnNames.Add(prop.Name);
                    }
                    firstRow = false;
                }

                data.Add(new GridOutputRow
                {
                    Cells = propertyDescriptorCollection.Select(d => new GridOutputCell
                    {
                        Value = SafeToString(d.GetValue(item), cultureInfo)
                    })
                });
            }

            return new GridOutput
            {
                ColumnNames = columnNames,
                Data = data
            };
        }

        private static string SafeToString(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "[[NULL]]";
            }

            if (value is DateTime dt)
            {
                return dt.ToString(cultureInfo);
            }
            var methodInfo = value.GetType().GetMethods().FirstOrDefault(x => x.Name == "ToString" && x.GetParameters().Length == 1 && x.GetParameters().First().ParameterType == typeof(IFormatProvider));
            if (methodInfo != null)
            {
                var s = (string)methodInfo.Invoke(value, new[] { cultureInfo });
                return (s ?? string.Empty).Replace("\t", "[[TAB]]");
            }
            return value.ToString().Replace("\t", "[[TAB]]");
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine(string.Join("\t", ColumnNames));
            foreach (var item in Data)
            {
                builder.AppendLine(string.Join("\t", item.Cells.Select(c => c.Value)));
            }

            return builder.ToString();
        }
    }
}
