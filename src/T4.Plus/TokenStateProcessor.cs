using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using Utilities;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenStateProcessor : ITokenParserTokenStateProcessor<TokenParserState>
    {
        private readonly ITokenParserTokenStateProcessor<TokenParserState> _baseProcessor;
        private IEnumerable<ITemplateToken<TokenParserState>> _sourceTokens;

        public TokenStateProcessor(ITokenParserTokenStateProcessor<TokenParserState> baseProcessor)
        {
            _baseProcessor = baseProcessor ?? throw new ArgumentNullException(nameof(baseProcessor));
        }

        public ProcessSectionResult<TokenParserState> Process(TokenParserState state, ITokenParserCallback<TokenParserState> tokenParserCallback, ILogger logger, TemplateParameter[] parameters)
            => ScopedAssemblyResolveOperation.Invoke
            (
                CurrentDomain_AssemblyResolve,
                () => ScopedBackingMemberOperation.InvokeAlways(ref _sourceTokens, state.Tokens, () => _baseProcessor.Process(state, tokenParserCallback, logger, parameters))
            );

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            => ScopedMember.Evaluate
            (
                args.Name.GetAssemblyName(),
                name => _sourceTokens
                    .NotNull()
                    .GetTemplateTokensFromSections<TokenParserState, IHintPathToken<TokenParserState>>()
                    .Where(t => t.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) != false)
                    .SelectMany(hintPathToken => hintPathToken.HintPath.GetDirectories(hintPathToken.Recursive).Select(p => Path.Combine(p, name)))
                    .Where(File.Exists)
                    .Select(Assembly.LoadFrom)
                    .FirstOrDefault()
            );
    }
}
