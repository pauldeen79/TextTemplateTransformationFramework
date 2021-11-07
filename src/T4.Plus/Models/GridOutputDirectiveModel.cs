using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Allows to create tabular output in tab-delimited format using the GridOutput class")]
    public class GridOutputDirectiveModel
    {
        [Description("Indicator whether the GridOutput class should be added to the template base class, in order to allow to convert output to a (tab-delimited) data set")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
