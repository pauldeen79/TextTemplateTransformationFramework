using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public abstract class RegisterChildTemplateDirectiveModelBase<TState> : ISectionContextContainer<TState>
        where TState : class
    {
        [Description("Optional typename of the base class to inherit from")]
        public string BaseClass { get; set; }

        [Browsable(false)]
        public SectionContext<TState> SectionContext { get; set; }
    }
}
