using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities;

namespace TextTemplateTransformationFramework.Common.SectionProcessors
{
    public sealed class CompositeTemplateSectionProcessor<TState> : ICompositeTemplateSectionProcessor<TState>, ITemplateSectionProcessorContainer<TState>
        where TState : class
    {
        private readonly ITemplateSectionProcessor<TState>[] _containedTemplateSectionProcessors;

        public CompositeTemplateSectionProcessor(IEnumerable<ITemplateSectionProcessor<TState>> containedTemplateSectionProcessors)
        {
            _containedTemplateSectionProcessors = containedTemplateSectionProcessors?.ToArray() ?? throw new ArgumentNullException(nameof(containedTemplateSectionProcessors));
        }

        IEnumerable<ITemplateSectionProcessor<TState>> ITemplateSectionProcessorContainer<TState>.ContainedTemplateSectionProcessors => _containedTemplateSectionProcessors;

        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            var result = SectionProcessResult<TState>.NotUnderstood;
            foreach (var proc in _containedTemplateSectionProcessors.Where(p => p.IsProcessorForSection(context)))
            {
                result = ScopedMember.Evaluate
                (
                    proc.Process(context),
                    innerResult => innerResult.Understood
                        ? SectionProcessResult.Create
                            (
                                innerResult.Tokens,
                                innerResult.SwitchToMode,
                                innerResult.PassThrough,
                                innerResult.TokensAreForRootTemplateSection,
                                result,
                                proc.GetType()
                            )
                        /*: innerResult.PassThrough
                            ? SectionProcessResult.EmptyPassThrough*/
                            : result
                );

                if (result.Understood && !result.PassThrough)
                {
                    break;
                }
            }

            return result;
        }
    }
}
