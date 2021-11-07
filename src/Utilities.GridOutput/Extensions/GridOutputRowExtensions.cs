using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.GridOutput.Extensions
{
    public static class GridOutputRowExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this GridOutputRow instance, GridOutput parentInstance)
        {
            if (parentInstance == null)
            {
                throw new ArgumentNullException(nameof(parentInstance));
            }
            return parentInstance.ColumnNames
                                 .Select((name, i) => new KeyValuePair<string, string>(name, instance.Cells.ElementAt(i).Value));
        }

        public static IDictionary<string, string> ToDictionary(this GridOutputRow instance, GridOutput parentInstance)
            => instance.ToKeyValuePairs(parentInstance).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
