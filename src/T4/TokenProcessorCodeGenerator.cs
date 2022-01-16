using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.CodeGenerators;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Requests;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenProcessorCodeGenerator<TState> : ITokenProcessorCodeGenerator<TState>
        where TState : class
    {
        public CodeGeneratorResultBuilder Generate(GenerateCodeRequest<TState> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var codeGenerator = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.GeneratorName), request.TemplateTokens.OfType<IGeneratorToken<TState>>().FirstOrDefault()?.Name ?? nameof(T4CSharpCodeGenerator) },
                    { nameof(T4CSharpCodeGenerator.GeneratorVersion), request.TemplateTokens.OfType<IGeneratorToken<TState>>().FirstOrDefault()?.Version ?? "1.0.0.0" },
                    { nameof(T4CSharpCodeGenerator.CultureCode), request.TemplateTokens.GetCultureCode() },
                    { nameof(T4CSharpCodeGenerator.TemplateNamespace), request.TemplateTokens.GetTemplateNamespace() },
                    { nameof(T4CSharpCodeGenerator.TemplateClassName), request.TemplateTokens.GetTemplateClassName() },
                    { nameof(T4CSharpCodeGenerator.Model), request.TemplateTokens },
                    { nameof(T4CSharpCodeGenerator.TemplateIsOverride), request.TemplateTokens.GetTemplateIsOverride() },
                    { nameof(T4CSharpCodeGenerator.GenerationEnvironmentAccessor), request.TemplateTokens.GetGenerationEnvironmentAccessor() },
                    { nameof(T4CSharpCodeGenerator.EnvironmentVersion), request.TemplateTokens.GetEnvironmentVersion() },
                }
            };

            codeGenerator.Initialize();
            var sourceCodeStringBuilder = new StringBuilder();
            codeGenerator.Render(sourceCodeStringBuilder);

            return new CodeGeneratorResultBuilder
            (
                sourceCodeStringBuilder,
                codeGenerator.Errors.Select(e => new CompilerError(e.Column, e.ErrorNumber, e.ErrorText, e.FileName, e.IsWarning, e.Line))
            );
        }
    }
}
