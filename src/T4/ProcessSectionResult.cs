using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Extensions;

namespace TextTemplateTransformationFramework.T4
{
    public static class ProcessSectionResult
    {
        public static ProcessSectionResult<TState> Unrecognized<TState>(SectionContext<TState> context) where TState : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new ProcessSectionResult<TState>
            (
                new[]
                {
                    new SourceSectionToken<TState>
                    (
                        context,
                        null,
                        false,
                        new[]
                        {
                            new InitializeErrorToken<TState>(context, "Unknown directive: " + context.Section)
                        }
                    )
                },
                Array.Empty<ISourceSectionToken<TState>>(),
                Array.Empty<ITemplateSectionProcessor<TState>>(),
                null,
                false
            );
        }

        public static ProcessSectionResult<TState> Create<TState>
        (
            SectionContext<TState> context,
            Type sectionProcessorType,
            SectionProcessResult<TState> sectionProcessResult
        ) where TState : class
        {
            if(sectionProcessResult == null)
            {
                throw new ArgumentNullException(nameof(sectionProcessResult));
            }

            return new ProcessSectionResult<TState>
            (
                new[]
                {
                    new SourceSectionToken<TState>
                    (
                        context,
                        sectionProcessResult.CustomProcessorType ?? sectionProcessorType,
                        sectionProcessResult.TokensAreForRootTemplateSection,
                        sectionProcessResult.GetTemplateTokens()
                    )
                },
                sectionProcessResult.GetSourceSectionTokens(),
                sectionProcessResult.GetTemplateSectionProcessors(),
                sectionProcessResult.SwitchToMode,
                sectionProcessResult.PassThrough
            );
        }
    }

    public sealed class ProcessSectionResult<TState>
        where TState : class
    {
        public IEnumerable<ISourceSectionToken<TState>> TemplateTokensSections { get; }
        public IEnumerable<ISourceSectionToken<TState>> AdditionalSections { get; }
        public IEnumerable<ITemplateSectionProcessor<TState>> SectionProcessors { get; }
        public int? SwitchToMode { get; }
        public bool PassThrough { get; }

        internal ProcessSectionResult
        (
            IEnumerable<ISourceSectionToken<TState>> templateTokensSections,
            IEnumerable<ISourceSectionToken<TState>> additionalSections,
            IEnumerable<ITemplateSectionProcessor<TState>> sectionProcessors,
            int? switchToMode,
            bool passThrough
        )
        {
            TemplateTokensSections = templateTokensSections.ToArray();
            AdditionalSections = additionalSections.ToArray();
            SectionProcessors = sectionProcessors.ToArray();
            SwitchToMode = switchToMode;
            PassThrough = passThrough;
        }

        public static readonly ProcessSectionResult<TState> Empty =
            new ProcessSectionResult<TState>
            (
                Array.Empty<ISourceSectionToken<TState>>(),
                Array.Empty<ISourceSectionToken<TState>>(),
                Array.Empty<ITemplateSectionProcessor<TState>>(),
                null,
                true
            );

        public ProcessSectionResult<TState> With(IEnumerable<ISourceSectionToken<TState>> additionalSourceSections)
            => new ProcessSectionResult<TState>
            (
                TemplateTokensSections.Concat(additionalSourceSections),
                AdditionalSections,
                SectionProcessors,
                SwitchToMode,
                PassThrough
            );
    }
}
