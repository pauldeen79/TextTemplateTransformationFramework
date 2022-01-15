using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenProcessor<TState> : ITokenProcessor<TState>
        where TState : class
    {
        private readonly ITokenProcessorCodeDomLanguageConverter _codeDomLanguageConverter;
        private readonly ITokenProcessorTokenConverter<TState> _tokenConverter;
        private readonly ITokenProcessorCodeGenerator<TState> _codeGenerator;
        private readonly ITokenProcessorReferenceAssemblyNamesProvider<TState> _referenceAssemblyNamesProvider;
        private readonly ITokenProcessorPackageReferenceNamesProvider<TState> _packageReferenceNamesProvider;
        private readonly ITokenProcessorDefaultNamespaceImportTokenProvider<TState> _defaultNamespaceImportTokenProvider;

        public TokenProcessor(ITokenProcessorCodeDomLanguageConverter codeDomLanguageConverter,
                              ITokenProcessorTokenConverter<TState> tokenConverter,
                              ITokenProcessorCodeGenerator<TState> codeGenerator,
                              ITokenProcessorReferenceAssemblyNamesProvider<TState> referenceAssemblyNamesProvider,
                              ITokenProcessorPackageReferenceNamesProvider<TState> packageReferenceNamesProvider,
                              ITokenProcessorDefaultNamespaceImportTokenProvider<TState> defaultNamespaceImportTokenProvider)
        {
            _codeDomLanguageConverter = codeDomLanguageConverter ?? throw new ArgumentNullException(nameof(codeDomLanguageConverter));
            _tokenConverter = tokenConverter ?? throw new ArgumentNullException(nameof(tokenConverter));
            _codeGenerator = codeGenerator ?? throw new ArgumentNullException(nameof(codeGenerator));
            _referenceAssemblyNamesProvider = referenceAssemblyNamesProvider ?? throw new ArgumentNullException(nameof(referenceAssemblyNamesProvider));
            _packageReferenceNamesProvider = packageReferenceNamesProvider ?? throw new ArgumentNullException(nameof(packageReferenceNamesProvider));
            _defaultNamespaceImportTokenProvider = defaultNamespaceImportTokenProvider ?? throw new ArgumentNullException(nameof(defaultNamespaceImportTokenProvider));
        }

        public TemplateCodeOutput<TState> Process(ITextTemplateProcessorContext<TState> context, IEnumerable<ITemplateToken<TState>> tokens)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //First, flatten hierarchy from template sections into tokens
            var templateTokens = ConvertTokens
            (
                tokens
                .GetTemplateTokensFromSections()
                .Concat(GetDefaultUsingsTokens())
            ).ToList();

            var templateNamespace = templateTokens.GetTemplateNamespace();
            var templateClassName = templateTokens.GetTemplateClassName();
            var language = templateTokens.GetLanguage();
            var codeDomLanguage = GetCodeDomLanguage(language)
                .WhenNull
                (
                    CodeDomLanguage.CSharp,
                    () => templateTokens.Add(new InitializeErrorToken<TState>(SectionContext<TState>.Empty, "Unsupported language: " + language))
                );
            var outputExtension = templateTokens.GetOutputExtension();

            var codeGeneratorResultBuilder = RunCodeGenerator(templateTokens).WithLanguage(codeDomLanguage);

            var tempPath = templateTokens.GetTempPath();

            // Important: Store temp path in context for later use
            context["TempPath"] = tempPath;

            return new TemplateCodeOutput<TState>
            (
                tokens,
                codeGeneratorResultBuilder.Build(),
                outputExtension,
                GetReferencedAssemblies(templateTokens),
                GetPackageReferences(templateTokens),
                $"{templateNamespace}.{templateClassName}",
                tempPath
            );
        }

        private string GetCodeDomLanguage(Language language)
            => _codeDomLanguageConverter.Convert(language);

        private IEnumerable<ITemplateToken<TState>> ConvertTokens(IEnumerable<ITemplateToken<TState>> tokens)
            => _tokenConverter.Convert(tokens);

        private CodeGeneratorResultBuilder RunCodeGenerator(IEnumerable<ITemplateToken<TState>> templateTokens)
            => _codeGenerator.Generate(new Requests.GenerateCodeRequest<TState>(templateTokens));

        private IEnumerable<string> GetReferencedAssemblies(IEnumerable<ITemplateToken<TState>> templateTokens)
            => _referenceAssemblyNamesProvider.Get(templateTokens);

        private IEnumerable<string> GetPackageReferences(IEnumerable<ITemplateToken<TState>> templateTokens)
            => _packageReferenceNamesProvider.Get(templateTokens);

        private IEnumerable<ITemplateToken<TState>> GetDefaultUsingsTokens()
            => _defaultNamespaceImportTokenProvider.Get();
    }
}
