using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
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

            return GetTokens(model);
        }

        private IEnumerable<ITemplateToken<TState>> GetTokens(RegisterCustomSectionProcessorDirectiveModel<TState> instance)
        {
            if (CustomSectionProcessorDoesNotExist(instance))
            {
                yield return new InitializeErrorToken<TState>(instance.SectionContext, string.Format("Custom section processor assembly [{0}] does not exist", instance.AssemblyName));
                yield break;
            }
            
            if (instance.SectionContext != null)
            {
                //assembly
                var assembly = string.IsNullOrEmpty(instance.AssemblyName)
                    ? null
#pragma warning disable S3885 // "Assembly.Load" should be used
                    : System.Reflection.Assembly.LoadFrom(instance.AssemblyName.GetAssemblyName());
#pragma warning restore S3885 // "Assembly.Load" should be used

                foreach (var type in GetTypes(instance, assembly))
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

        private static bool CustomSectionProcessorDoesNotExist(RegisterCustomSectionProcessorDirectiveModel<TState> instance)
            => !string.IsNullOrEmpty(instance.AssemblyName)
                && instance.AssemblyName.EndsWith(".dll")
                && !File.Exists(instance.AssemblyName);

        private static IEnumerable<Type> GetTypes(RegisterCustomSectionProcessorDirectiveModel<TState> instance, System.Reflection.Assembly assembly)
            => assembly == null
                ? GetCustomProcessorTypes(instance.TypeName, instance.SectionContext.ExistingTokens)
                : assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(x =>
                    x.IsGenericType
                    && x.GetGenericTypeDefinition() == typeof(ITemplateSectionProcessor<>)));

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
