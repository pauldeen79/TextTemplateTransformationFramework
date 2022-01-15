using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common
{
    public static class SectionContext
    {
        public static SectionContext<TState> FromCurrentMode<TState>(int currentMode, TState state)
            where TState : class 
            => new SectionContext<TState>
            (
                new Section(string.Empty, 0, string.Empty),
                Enumerable.Empty<ITemplateToken<TState>>(),
                null,
                currentMode,
                state,
                null,
                null
            );

        public static SectionContext<TState> FromSection<TState>
        (
            Section section,
            int currentMode,
            IEnumerable<ITemplateToken<TState>> existingTokens,
            ITokenParserCallback<TState> tokenParserCallback,
            TState state,
            ILogger logger,
            TemplateParameter[] parameters
        ) where TState : class
            => new SectionContext<TState>
            (
                section,
                existingTokens,
                tokenParserCallback,
                currentMode,
                state,
                logger,
                parameters
            );

        public static SectionContext<TState> FromToken<TState>(ITemplateToken<TState> token, TState state) where TState : class
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return FromSection
            (
                new Section(token.SectionContext.FileName, token.SectionContext.LineNumber, token.SectionContext.Section),
                token.SectionContext.CurrentMode,
                token.SectionContext.ExistingTokens,
                token.SectionContext.TokenParserCallback,
                state,
                token.SectionContext.Logger,
                token.SectionContext.Parameters
            );
        }
    }

    public class Section
    {
        public string Contents { get; }
        public int LineNumber { get; }
        public string FileName { get; }
        public Section(string fileName, int lineNumber, string contents)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            LineNumber = lineNumber;
            Contents = contents ?? throw new ArgumentNullException(nameof(contents));
        }
    }

    public sealed class SectionContext<TState>
        where TState : class
    {
        public string Section { get; }
        public IEnumerable<ITemplateToken<TState>> ExistingTokens { get; }
        public int LineNumber { get; }
        public string FileName { get; }
        public ITokenParserCallback<TState> TokenParserCallback { get; }
        public int CurrentMode { get; }
        public TState State { get; }
        public ILogger Logger { get; }
        public TemplateParameter[] Parameters { get; }

        internal SectionContext(Section section,
                                IEnumerable<ITemplateToken<TState>> existingTokens,
                                ITokenParserCallback<TState> tokenParserCallback,
                                int currentMode,
                                TState state,
                                ILogger logger,
                                TemplateParameter[] parameters)
        {
            Section = section.Contents;
            ExistingTokens = existingTokens;
            LineNumber = section.LineNumber;
            FileName = section.FileName.WhenNullOrEmpty(() => null);
            TokenParserCallback = tokenParserCallback;
            CurrentMode = currentMode;
            State = state;
            Logger = logger;
            Parameters = parameters;
        }

        public IEnumerable<ITemplateSectionProcessor<TState>> CustomSectionProcessors
            => ExistingTokens
                .GetTemplateTokensFromSections<TState, ITemplateSectionProcessorTemplateToken<TState>>()
                .Select(t => t.TemplateSectionProcessor);

        public static readonly SectionContext<TState> Empty
            = new SectionContext<TState>
            (
                new Section(string.Empty, 0, string.Empty),
                Enumerable.Empty<ITemplateToken<TState>>(),
                null,
                Mode.Unknown,
                null,
                null,
                null
            );

        public SectionContext<TState> WithFileName(string fileName)
            => new SectionContext<TState>
            (
                new Section(fileName, LineNumber, Section),
                ExistingTokens,
                TokenParserCallback,
                CurrentMode,
                State,
                Logger,
                Parameters
            );
    }
}
