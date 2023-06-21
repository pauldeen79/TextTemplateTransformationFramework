using System;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TextTemplateProcessorContext<TState> : ContextBase, ITextTemplateProcessorContext<TState>
        where TState : class
    {
        public TextTemplateProcessorContext(TextTemplate textTemplate,
                                            TemplateParameter[] parameters,
                                            ILogger logger,
                                            SectionContext<TState> parentContext)
        {
            TextTemplate = textTemplate ?? throw new ArgumentNullException(nameof(textTemplate));
            Parameters = parameters?.ToArray() ?? Array.Empty<TemplateParameter>();
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ParentContext = parentContext;
        }

        public TextTemplateProcessorContext(AssemblyTemplate assemblyTemplate,
                                            TemplateParameter[] parameters,
                                            ILogger logger,
                                            SectionContext<TState> parentContext)
        {
            AssemblyTemplate = assemblyTemplate ?? throw new ArgumentNullException(nameof(assemblyTemplate));
            Parameters = parameters?.ToArray() ?? Array.Empty<TemplateParameter>();
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ParentContext = parentContext;
        }

        public TextTemplate TextTemplate { get; }
        public AssemblyTemplate AssemblyTemplate { get; }
        public TemplateParameter[] Parameters { get; }
        public ILogger Logger { get; }
        public SectionContext<TState> ParentContext { get; }
    }
}
