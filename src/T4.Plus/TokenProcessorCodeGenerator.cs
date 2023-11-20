using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.CodeGenerators;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Requests;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenProcessorCodeGenerator<TState> : ITokenProcessorCodeGenerator<TState>
        where TState : class
    {
        public CodeGeneratorResultBuilder Generate(GenerateCodeRequest<TState> request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var codeGenerator = new T4PlusCSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4PlusCSharpCodeGenerator.GeneratorName), request.TemplateTokens.OfType<IGeneratorToken<TState>>().FirstOrDefault()?.Name ?? nameof(T4PlusCSharpCodeGenerator) },
                    { nameof(T4PlusCSharpCodeGenerator.GeneratorVersion), request.TemplateTokens.OfType<IGeneratorToken<TState>>().FirstOrDefault()?.Version ?? "1.0.0.0" },
                    { nameof(T4PlusCSharpCodeGenerator.CultureCode), request.TemplateTokens.GetCultureCode() },
                    { nameof(T4PlusCSharpCodeGenerator.TemplateNamespace), request.TemplateTokens.GetTemplateNamespace() },
                    { nameof(T4PlusCSharpCodeGenerator.TemplateClassName), request.TemplateTokens.GetClassName() },
                    { nameof(T4PlusCSharpCodeGenerator.Model), request.TemplateTokens },
                    { nameof(T4PlusCSharpCodeGenerator.TemplateIsOverride), request.TemplateTokens.GetTemplateIsOverride() },
                    { nameof(T4PlusCSharpCodeGenerator.GenerationEnvironmentAccessor), request.TemplateTokens.GetGenerationEnvironmentAccessor() },
                    { nameof(T4PlusCSharpCodeGenerator.SkipInitializationCode), request.TemplateTokens.GetSkipInitializationCode() },
                    { nameof(T4PlusCSharpCodeGenerator.BaseClassInheritsFrom), request.TemplateTokens.GetBaseClassInheritsFrom() },
                    { nameof(T4PlusCSharpCodeGenerator.EnableAdditionalActionDelegate), request.TemplateTokens.GetEnableAdditionalActionDelegate() },
                    { nameof(T4PlusCSharpCodeGenerator.AddTemplateLineNumbers), request.TemplateTokens.GetAddTemplateLineNumbers() },
                    { nameof(T4PlusCSharpCodeGenerator.EnvironmentVersion), request.TemplateTokens.GetEnvironmentVersion() },
                    { nameof(T4PlusCSharpCodeGenerator.AddExcludeFromCodeCoverageAttribute), request.TemplateTokens.GetAddExcludeFromCodeCoverage() }
                }
            };

            codeGenerator.Initialize(() =>
            {
                foreach (var customTemplateToken in request.TemplateTokens.OfType<IAdditionalChildTemplateToken<TState>>())
                {
                    codeGenerator.RegisterChildTemplate(customTemplateToken.TemplateName, customTemplateToken.TemplateDelegate, customTemplateToken.ModelType);
                }
            });
            var sourceCodeStringBuilder = new StringBuilder();
            codeGenerator.Render(sourceCodeStringBuilder);

            return new CodeGeneratorResultBuilder
            (
                sourceCodeStringBuilder,
                codeGenerator.Errors.Select(d => new CompilerError(d.Column, d.ErrorNumber, d.ErrorText, d.FileName, d.IsWarning, d.Line))
            );
        }
    }
}
