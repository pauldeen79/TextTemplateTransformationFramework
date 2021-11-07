using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class AdditionalChildTemplateToken<TState> : TemplateToken<TState>, IAdditionalChildTemplateToken<TState>
        where TState : class
    {
        public AdditionalChildTemplateToken(SectionContext<TState> context, string templateName, Func<object> templateDelegate, Type modelType = null)
            : base(context)
        {
            TemplateName = templateName;
            TemplateDelegate = templateDelegate;
            ModelType = modelType;
        }

        public string TemplateName { get; }
        public Func<object> TemplateDelegate { get; }
        public Type ModelType { get; }
    }
}
