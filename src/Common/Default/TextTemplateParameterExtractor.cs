using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TextTemplateParameterExtractor<TState> : ITextTemplateParameterExtractor<TState>
        where TState : class
    {
        private readonly ITextTemplateProcessorPropertyOwnerProvider<TState> _textTemplateProcessorPropertyOwnerProvider;
        private readonly ITextTemplateProcessorPropertyProvider<TState> _textTemplateProcessorPropertyProvider;

        public TextTemplateParameterExtractor(ITextTemplateProcessorPropertyOwnerProvider<TState> textTemplateProcessorPropertyOwnerProvider,
                                              ITextTemplateProcessorPropertyProvider<TState> textTemplateProcessorPropertyProvider)
        {
            _textTemplateProcessorPropertyOwnerProvider = textTemplateProcessorPropertyOwnerProvider ?? throw new ArgumentNullException(nameof(textTemplateProcessorPropertyOwnerProvider));
            _textTemplateProcessorPropertyProvider = textTemplateProcessorPropertyProvider ?? throw new ArgumentNullException(nameof(textTemplateProcessorPropertyProvider));
        }

        public IEnumerable<TemplateParameter> Extract(ITextTemplateProcessorContext<TState> context,
                                                      TemplateCompilerOutput<TState> templateCompilerOutput)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (templateCompilerOutput == null)
            {
                throw new ArgumentNullException(nameof(templateCompilerOutput));
            }

            if (templateCompilerOutput.Template == null)
            {
                return Enumerable.Empty<TemplateParameter>();
            }

            var templateType = templateCompilerOutput.Template.GetType();
            var properties = GetProperties(context, templateCompilerOutput, templateType);

            return properties
                .Select(property => GetPropertyOwner(context, property, templateCompilerOutput).ToParameter(property))
                .Where(p => p.Browsable);
        }

        private object GetPropertyOwner(ITextTemplateProcessorContext<TState> context, PropertyDescriptor property, TemplateCompilerOutput<TState> templateCompilerOutput)
            => _textTemplateProcessorPropertyOwnerProvider.Get(context, property, templateCompilerOutput);

        private IEnumerable<PropertyDescriptor> GetProperties(ITextTemplateProcessorContext<TState> context, TemplateCompilerOutput<TState> templateCompilerOutput, Type templateType)
            => _textTemplateProcessorPropertyProvider.Get(context, templateCompilerOutput, templateType);
    }
}
