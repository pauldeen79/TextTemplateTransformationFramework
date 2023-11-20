using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenModifier : ITokenParserTokenModifier
    {
        private IEnumerable<ITemplateToken<TokenParserState>> _rootTokens;

        public IEnumerable<ITemplateToken<TokenParserState>> Modify(IEnumerable<ITemplateToken<TokenParserState>> tokens)
        {
            foreach (var token in tokens.ToArray())
            {
                if (token is IReferenceToken<TokenParserState> referenceToken)
                {
                    try
                    {
#pragma warning disable S3885 // "Assembly.Load" should be used
                        Assembly.LoadFrom(referenceToken.Name.GetAssemblyName());
#pragma warning restore S3885 // "Assembly.Load" should be used
                    }
                    catch (Exception)
                    {
                        referenceToken = GetReferenceTokenFromAlternatePaths(referenceToken);
                    }

                    yield return referenceToken;
                }
                else if (token is ISourceSectionToken<TokenParserState> sourceSectionToken)
                {
                    var setRootTokens = _rootTokens is null;
                    yield return ScopedBackingMemberOperation.Invoke
                    (
                        ref _rootTokens,
                        tokens,
                        _ => setRootTokens,
                        () => CreateSourceSectionToken(sourceSectionToken)
                    );
                }
                else
                {
                    yield return token;
                }
            }
        }

        private IReferenceToken<TokenParserState> GetReferenceTokenFromAlternatePaths(IReferenceToken<TokenParserState> referenceToken)
        {
            //If the assemby can't be loaded, try alternate paths
            var name = referenceToken.Name.GetAssemblyName();
            var hintPathTokens = _rootTokens
                .GetTemplateTokensFromSections<TokenParserState, IHintPathToken<TokenParserState>>()
                .Where(t => t.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) != false);
            foreach (var hintPathToken in hintPathTokens)
            {
                foreach (var directory in hintPathToken.HintPath.GetDirectories(hintPathToken.Recursive))
                {
                    var fullPath = Path.Combine(directory, name);
                    if (File.Exists(fullPath))
                    {
                        referenceToken = new ReferenceToken<TokenParserState>(SectionContext.FromToken(referenceToken, referenceToken.SectionContext.State), fullPath);
                        break;
                    }
                }
            }

            // Try current directory
            var currentFullPath = Path.Combine(Directory.GetCurrentDirectory(), name);
            if (File.Exists(currentFullPath))
            {
                referenceToken = new ReferenceToken<TokenParserState>(SectionContext.FromToken(referenceToken, referenceToken.SectionContext.State), currentFullPath);
            }

            return referenceToken;
        }

        private SourceSectionToken<TokenParserState> CreateSourceSectionToken(ISourceSectionToken<TokenParserState> sourceSectionToken)
            => new SourceSectionToken<TokenParserState>
            (
                SectionContext.FromToken(sourceSectionToken, sourceSectionToken.SectionContext.State),
                sourceSectionToken.SectionProcessorType,
                sourceSectionToken.IsRootTemplateSection,
                Modify(sourceSectionToken.TemplateTokens)
            );
    }
}
