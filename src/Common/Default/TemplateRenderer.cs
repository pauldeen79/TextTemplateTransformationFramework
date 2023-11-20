using System;
using System.Collections.Generic;
using System.Text;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateRenderer<TState> : ITemplateRenderer<TState>
        where TState : class
    {
        private readonly ITemplateValidator _templateValidator;
        private readonly ITemplateCompilerOutputValidator<TState> _templateCompilerOutputValidator;
        private readonly ITemplateRenderParameterSetter<TState> _parameterSetter;
        private readonly ITemplateProxy _templateProxy;

        public TemplateRenderer(ITemplateValidator templateValidator,
                                ITemplateCompilerOutputValidator<TState> templateCompilerOutputValidator,
                                ITemplateRenderParameterSetter<TState> parameterSetter,
                                ITemplateProxy templateProxy)
        {
            _templateValidator = templateValidator ?? throw new ArgumentNullException(nameof(templateValidator));
            _templateCompilerOutputValidator = templateCompilerOutputValidator ?? throw new ArgumentNullException(nameof(templateCompilerOutputValidator));
            _parameterSetter = parameterSetter ?? throw new ArgumentNullException(nameof(parameterSetter));
            _templateProxy = templateProxy ?? throw new ArgumentNullException(nameof(templateProxy));
        }

        public ProcessResult Render(ITemplateProcessorContext<TState> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            SetParameters(context);
            var validateResult = ValidateTemplate(context.TemplateCompilerOutput.Template);
            if (!validateResult.IsValid())
            {
                return validateResult;
            }

            var stringBuilder = new StringBuilder();
            RenderTemplate(context.TemplateCompilerOutput.Template, stringBuilder);
            if (!ValidateErrors(context, out IEnumerable<CompilerError> compilerErrorCollection))
            {
                //validation error (in render phase)
                return ProcessResult.Create
                (
                    compilerErrorCollection,
                    sourceCode: context.TemplateCompilerOutput.SourceCode,
                    diagnosticDump: context.TextTemplateProcessorContext.Logger.Aggregate(),
                    outputExtension: context.TemplateCompilerOutput.OutputExtension
                );
            }
            else
            {
                //render phase ok
                return ProcessResult.Create
                (
                    compilerErrorCollection,
                    stringBuilder.ToString(),
                    context.TemplateCompilerOutput.SourceCode,
                    diagnosticDump: context.TextTemplateProcessorContext.Logger.Aggregate(),
                    outputExtension: context.TemplateCompilerOutput.OutputExtension
                );
            }
        }

        private ProcessResult ValidateTemplate(object template)
            => _templateValidator.Validate(template);

        private bool ValidateErrors(ITemplateProcessorContext<TState> context, out IEnumerable<CompilerError> compilerErrorCollection)
            => _templateCompilerOutputValidator.Validate(context, out compilerErrorCollection);

        private void RenderTemplate(object template, StringBuilder stringBuilder)
            => _templateProxy.Render(template, stringBuilder);

        private void SetParameters(ITemplateProcessorContext<TState> context)
            => _parameterSetter.Set(context);
    }
}
