using System;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Data;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.LanguageServices;
using TextTemplateTransformationFramework.T4.Common.Contracts;
using Utilities;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.SectionProcessors
{
    public sealed class TokenMapperAdapter<TState> : IModeledTemplateSectionProcessor<TState>, ITemplateCustomDirectiveName
        where TState : class
    {
        private readonly TokenMapperAttribute _tokenMapperAttribute;
        private readonly Type _directiveSerializerType;
        private readonly object _mapperInstance;
        private readonly MethodInfo _mapMethod;
        private readonly Func<SectionContext<TState>, object, bool> _isValidDelegate;
        private readonly bool _passThrough;
        private readonly bool _tokensAreForRootTemplateSection;

        public string TemplateCustomDirectiveName { get; }

        public Type ModelType
            => _tokenMapperAttribute.Type.GetModelType(typeof(TState));

        private readonly IFileNameProvider _fileNameProvider;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;

        public TokenMapperAdapter(Type mapperType,
                                  IFileNameProvider fileNameProvider,
                                  IFileContentsProvider fileContentsProvider,
                                  ITemplateCodeCompiler<TState> templateCodeCompiler)
        {
            mapperType = mapperType?.GetModelType(typeof(TState)) ?? throw new ArgumentNullException(nameof(mapperType));
            _fileNameProvider = fileNameProvider ?? throw new ArgumentNullException(nameof(fileNameProvider));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));
            _tokenMapperAttribute = mapperType.GetCustomAttribute<TokenMapperAttribute>(true);
            if (_tokenMapperAttribute == null)
            {
                var groupedAttr = mapperType.GetCustomAttribute<GroupedTokenMapperAttribute>(true);
                if (groupedAttr == null)
                {
                    throw new ArgumentException(string.Format("Mapper type [{0}] does not have a TokenMapperAttribute or GroupedTokenMapperAttribute", mapperType.FullName), nameof(mapperType));
                }
                _tokenMapperAttribute = new TokenMapperAttribute(groupedAttr.Type);
            }

            var prefixAttribute = mapperType.GetCustomAttribute<DirectivePrefixAttribute>(true);
            if (prefixAttribute == null)
            {
                throw new ArgumentException(string.Format("Mapper type [{0}] does not have a DirectivePrefixAttribute", mapperType.FullName), nameof(mapperType));
            }
            TemplateCustomDirectiveName = prefixAttribute.Name;

            _mapMethod = mapperType.GetMethod("Map");
            if (_mapMethod == null)
            {
                throw new ArgumentException(string.Format("Mapper type [{0}] does not have a Map method", mapperType.FullName), nameof(mapperType));
            }

            var isValidMethod = mapperType.GetMethod("IsValidForProcessing");
            _isValidDelegate = isValidMethod == null
                ? new Func<SectionContext<TState>, object, bool>((ctx, m) => true)
                : new Func<SectionContext<TState>, object, bool>((ctx, m) => (bool)isValidMethod.Invoke(_mapperInstance, [ctx, m]));

            _directiveSerializerType = typeof(DirectiveSerializer<,>).MakeGenericType(typeof(TState), _tokenMapperAttribute.Type.GetModelType(typeof(TState)));
            _mapperInstance = Activator.CreateInstance(mapperType);
            _mapperInstance.TrySetFileNameProvider(_fileNameProvider);
            _mapperInstance.TrySetFileContentsProvider(_fileContentsProvider);
            _mapperInstance.TrySetTemplateCodeCompiler(_templateCodeCompiler);
            _passThrough = mapperType.GetCustomAttribute<PassThroughAttribute>(true) != null;
            _tokensAreForRootTemplateSection = mapperType.GetCustomAttribute<RootTemplateAttribute>(true) != null;
        }

        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => Process(context, null);

        public SectionProcessResult<TState> Process(SectionContext<TState> context, SectionProcessResult<TState> existingResult)
            => ScopedMember.Evaluate
            (
                _directiveSerializerType
                    .GetMethod(nameof(DirectiveSerializer<TState, object>.Deserialize))
                    .Invoke
                    (
                        Activator.CreateInstance(_directiveSerializerType, context, _fileNameProvider, _fileContentsProvider, _templateCodeCompiler),
                        Array.Empty<object>()
                    ),
                model => SectionProcessResult.Create
                (
                    new SectionProcessResultData<TState>()
                        .WithContext(context)
                        .WithModel(model)
                        .WithIsValidDelegate(_isValidDelegate)
                        .WithMapDelegate(() => _mapMethod.Invoke
                        (
                            _mapperInstance,
                            [context, model]
                        ))
                        .WithPassThrough(_passThrough)
                        .WithTokensAreForRootTemplateSection(_tokensAreForRootTemplateSection)
                        .WithExistingResult(existingResult)
                        .WithDirectiveName(TemplateCustomDirectiveName.ToCamelCase() + " directive")
                )
            );

        public override string ToString() => TemplateCustomDirectiveName;
    }
}
