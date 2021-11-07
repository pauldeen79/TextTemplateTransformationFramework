using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class SourceSectionToken<TState> : TemplateToken<TState>, ISourceSectionToken<TState>
        where TState : class
    {
        public SourceSectionToken(SectionContext<TState> context, Type sectionProcessorType, bool isRootTemplateSection, IEnumerable<ITemplateToken<TState>> templateTokens)
            : base(context)
        {
            SourceSection = context.Section;
            SectionProcessorType = sectionProcessorType;
            Mode = context.CurrentMode;
            IsRootTemplateSection = isRootTemplateSection;
            TemplateTokens = new List<ITemplateToken<TState>>(templateTokens ?? Enumerable.Empty<ITemplateToken<TState>>());
        }

        public string SourceSection { get; }
        public Type SectionProcessorType { get; }
        public ICollection<ITemplateToken<TState>> TemplateTokens { get; }
        public int Mode { get; }
        public bool IsRootTemplateSection { get; }
    }
}
