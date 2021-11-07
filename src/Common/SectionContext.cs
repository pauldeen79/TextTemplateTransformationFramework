using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common
{
    public static class SectionContext
    {
        public static SectionContext<TState> FromCurrentMode<TState>(int currentMode, TState state)
            where TState : class 
            => new SectionContext<TState>
            (
                null,
                Enumerable.Empty<ITemplateToken<TState>>(),
                0,
                null,
                null,
                currentMode,
                state,
                null
            );

        public static SectionContext<TState> FromSection<TState>
        (
            string section,
            int currentMode,
            int lineNumber,
            string fileName,
            IEnumerable<ITemplateToken<TState>> existingTokens,
            ITokenParserCallback<TState> tokenParserCallback,
            TState state,
            ILogger logger
        ) where TState : class
            => new SectionContext<TState>
            (
                section,
                existingTokens,
                lineNumber,
                fileName,
                tokenParserCallback,
                currentMode,
                state,
                logger
            );

        public static SectionContext<TState> FromToken<TState>(ITemplateToken<TState> token, TState state) where TState : class
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return FromSection
            (
                token.SectionContext.Section,
                token.SectionContext.CurrentMode,
                token.SectionContext.LineNumber,
                token.SectionContext.FileName,
                token.SectionContext.ExistingTokens,
                token.SectionContext.TokenParserCallback,
                state,
                token.SectionContext.Logger
            );
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

        internal SectionContext(string section,
                               IEnumerable<ITemplateToken<TState>> existingTokens,
                               int lineNumber,
                               string fileName,
                               ITokenParserCallback<TState> tokenParserCallback,
                               int currentMode,
                               TState state,
                               ILogger logger)
        {
            Section = section;
            ExistingTokens = existingTokens;
            LineNumber = lineNumber;
            FileName = fileName;
            TokenParserCallback = tokenParserCallback;
            CurrentMode = currentMode;
            State = state;
            Logger = logger;
        }

        public IEnumerable<ITemplateSectionProcessor<TState>> CustomSectionProcessors
            => ExistingTokens
                .GetTemplateTokensFromSections<TState, ITemplateSectionProcessorTemplateToken<TState>>()
                .Select(t => t.TemplateSectionProcessor);

        public static readonly SectionContext<TState> Empty
            = new SectionContext<TState>
            (
                null,
                Enumerable.Empty<ITemplateToken<TState>>(),
                0,
                null,
                null,
                Mode.Unknown,
                null,
                null
            );

        public SectionContext<TState> WithFileName(string fileName)
            => new SectionContext<TState>
            (
                Section,
                ExistingTokens,
                LineNumber,
                fileName,
                TokenParserCallback,
                CurrentMode,
                State,
                Logger
            );
    }
}
