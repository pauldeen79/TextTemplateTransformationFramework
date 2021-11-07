using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RegisterCustomSectionProcessorDirectiveModel))]
    [DirectivePrefix("registerCustomSectionProcessor")]
    public sealed class RegisterCustomSectionProcessorTokenMapper<TState> : IMultipleTokenMapper<TState, RegisterCustomSectionProcessorDirectiveModel<TState>>, ITemplateCodeCompilerContainer<TState>, IFileContentsProviderContainer
        where TState : class
    {
        public ITemplateCodeCompiler<TState> TemplateCodeCompiler { get; set; }
        public IFileContentsProvider FileContentsProvider { get; set; }

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, RegisterCustomSectionProcessorDirectiveModel<TState> model)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return GetTokens(context, model);
        }

        private IEnumerable<ITemplateToken<TState>> GetTokens(SectionContext<TState> context, RegisterCustomSectionProcessorDirectiveModel<TState> instance)
        {
            if (string.IsNullOrEmpty(instance.File)
                && !string.IsNullOrEmpty(instance.AssemblyName)
                && instance.AssemblyName.EndsWith(".dll")
                && !File.Exists(instance.AssemblyName))
            {
                yield return new InitializeErrorToken<TState>(instance.SectionContext, string.Format("Custom section processor assembly [{0}] does not exist", instance.AssemblyName));
            }
            else if (!string.IsNullOrEmpty(instance.File))
            {
                //source code
                var source = FileContentsProvider.GetFileContents(instance.File);
                var referencedAssemblies = instance.SectionContext.ExistingTokens
                    .GetTemplateTokensFromSections<TState, IReferenceToken<TState>>()
                    .Select(r => r.Name)
                    .Distinct()
                    .ToArray();

                var packageReferences = instance.SectionContext.ExistingTokens
                    .GetTemplateTokensFromSections<TState, IPackageReferenceToken<TState>>()
                    .Select(r => r.Name)
                    .Distinct()
                    .ToArray();

                var tempPath = instance.SectionContext.ExistingTokens
                    .GetTemplateTokensFromSections<TState, ITempPathToken<TState>>()
                    .Distinct(t => t.Value)
                    .LastOrDefault(); //design decision: when multiple values are found, use the last one

                var compileOutput = TemplateCodeCompiler.Compile
                    (
                        context.GetTextTemplateProcessorContext(),
                        new TemplateCodeOutput<TState>
                        (
                            Array.Empty<ITemplateToken<TState>>(),
                            source,
                            "cs",
                            "C#",
                            referencedAssemblies,
                            packageReferences,
                            instance.TypeName,
                            tempPath,
                            Array.Empty<CompilerError>()
                        )
                    );

                if (!(compileOutput.Template is ITemplateSectionProcessor<TState> processor))
                {
                    yield return new InitializeErrorToken<TState>(instance.SectionContext, "Compilation of custom section processor failed");
                }
                else
                {
                    yield return new TemplateSectionProcessorTemplateToken<TState>(instance.SectionContext, processor);
                    if (processor is IInitializableTemplateSectionProcessor<TState> initializableTemplateSectionProcessor)
                    {
                        var result = initializableTemplateSectionProcessor.Initialize(instance.SectionContext);
                        if (result.Understood)
                        {
                            foreach (var t in result.Tokens)
                            {
                                yield return t;
                            }
                        }
                    }
                }
            }
            else if (instance.SectionContext != null)
            {
                //assembly
                var assembly = string.IsNullOrEmpty(instance.AssemblyName)
                    ? null
#pragma warning disable S3885 // "Assembly.Load" should be used
                    : System.Reflection.Assembly.LoadFrom(instance.AssemblyName.GetAssemblyName());
#pragma warning restore S3885 // "Assembly.Load" should be used

                foreach (var type in assembly == null
                    ? GetCustomProcessorTypes(instance.TypeName, instance.SectionContext.ExistingTokens)
                    : assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(x =>
                        x.IsGenericType
                        && x.GetGenericTypeDefinition() == typeof(ITemplateSectionProcessor<>))))
                {
                    if (type == null)
                    {
                        yield return new InitializeErrorToken<TState>(instance.SectionContext, string.Format("Could not get type [{0}] as a custom section processor. Did you forget a reference?", instance.TypeName));
                        continue;
                    }

                    if (!string.IsNullOrEmpty(instance.TypeName)
                        && !type.FullName.WithoutGenerics().Equals(instance.TypeName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var concreteType = type.GetModelType(typeof(TState));
                    var processor = (ITemplateSectionProcessor<TState>)Activator.CreateInstance(concreteType, Array.Empty<object>());
                    yield return new TemplateSectionProcessorTemplateToken<TState>(instance.SectionContext, processor);

                    if (processor is IInitializableTemplateSectionProcessor<TState> initializableTemplateSectionProcessor)
                    {
                        var result = initializableTemplateSectionProcessor.Initialize(instance.SectionContext);
                        if (result.Understood)
                        {
                            foreach (var t in result.Tokens)
                            {
                                yield return t;
                            }
                        }
                    }
                }
            }
        }

        private static Type[] GetCustomProcessorTypes(string typeName, IEnumerable<ITemplateToken<TState>> existingTokens)
        {
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic
                        && !string.IsNullOrEmpty(asm.Location)
                        && !asm.FullName.IsUnitTestAssembly()))
            {
                var t = ass.GetType(typeName);
                if (t != null)
                {
                    return new[] { t };
                }
            }

            foreach (var assemblyToken in existingTokens.GetTemplateTokensFromSections<TState, IAssemblyToken<TState>>())
            {
                var t = assemblyToken.Assembly.GetType(typeName);
                if (t != null)
                {
                    return new[] { t };
                }
            }

            return new Type[] { null };
        }
    }
}
