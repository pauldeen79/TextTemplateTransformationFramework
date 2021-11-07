using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public abstract class ChildViewModelClassToken<TState> : TemplateToken<TState>, IChildViewModelClassToken<TState>
        where TState : class
    {
        protected ChildViewModelClassToken(SectionContext<TState> context,
                                           string className,
                                           string baseClass,
                                           string modelType,
                                           bool copyPropertiesFromTemplate,
                                           IEnumerable<ITemplateToken<TState>> childTemplateTokens = null)
            : base(context)
        {
            ClassName = className;
            BaseClass = baseClass;
            ModelType = modelType;
            CopyPropertiesFromTemplate = copyPropertiesFromTemplate;
            ChildTemplateTokens = childTemplateTokens.ToArray();
        }

        public string ClassName { get; }

        public string BaseClass { get; }

        public string ModelType { get; }

        public bool CopyPropertiesFromTemplate { get; }

        public IEnumerable<ITemplateToken<TState>> ChildTemplateTokens { get; }
    }
}
