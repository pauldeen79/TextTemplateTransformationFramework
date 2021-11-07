using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public abstract class TemplateToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        protected TemplateToken(SectionContext<TState> context, string overrideFileName = null)
        {
            SectionContext = context ?? throw new ArgumentNullException(nameof(context));
            LineNumber = context.LineNumber;
            FileName = overrideFileName ?? context.FileName;
        }

        public int LineNumber { get; }

        public string FileName { get; }

        public SectionContext<TState> SectionContext { get; }
    }
}
