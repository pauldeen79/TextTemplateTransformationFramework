using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public sealed class TokenParserCallback : ITokenParserCallback<TokenParserState>
    {
        private readonly ITokenPlaceholderProcessor<TokenParserState> _tokenPlaceholderProcessor;
        private readonly Func<ITextTemplateProcessorContext<TokenParserState>, IEnumerable<ITemplateToken<TokenParserState>>> _parseDelegate;
        private readonly Func<bool> _isChildTemplateDelegate;
        private readonly ITokenArgumentParser<TokenParserState> _tokenArgumentParser;
        private readonly IArgumentParser _argumentParser;
        private readonly IDictionary<string, object> _state;

        public TokenParserCallback
        (
            ITokenPlaceholderProcessor<TokenParserState> tokenPlaceholderProcessor,
            Func<ITextTemplateProcessorContext<TokenParserState>, IEnumerable<ITemplateToken<TokenParserState>>> parseDelegate,
            Func<bool> isChildTemplateDelegate,
            ITokenArgumentParser<TokenParserState> tokenArgumentParser,
            IArgumentParser argumentParser
        )
        {
            _tokenPlaceholderProcessor = tokenPlaceholderProcessor ?? throw new ArgumentNullException(nameof(tokenPlaceholderProcessor));
            _parseDelegate = parseDelegate ?? throw new ArgumentNullException(nameof(parseDelegate));
            _isChildTemplateDelegate = isChildTemplateDelegate;
            _argumentParser = argumentParser ?? throw new ArgumentNullException(nameof(argumentParser));
            _tokenArgumentParser = tokenArgumentParser ?? throw new ArgumentNullException(nameof(tokenArgumentParser));
            _state = new Dictionary<string, object>();
        }

        public IEnumerable<string> GetSectionArguments(SectionContext<TokenParserState> context, string name)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _argumentParser.Parse(context.Section, name)
                .Concat(_tokenArgumentParser?.ParseArgument(context, name) ?? Enumerable.Empty<string>())
                .Select(value => _tokenPlaceholderProcessor.Process(value, context.ExistingTokens, context.State));
        }

        public IEnumerable<ITemplateToken<TokenParserState>> Parse(ITextTemplateProcessorContext<TokenParserState> context)
            => _parseDelegate(context);

        public bool IsChildTemplate
            => _isChildTemplateDelegate();

        public bool SectionIsDirectiveWithName(SectionContext<TokenParserState> context, string name)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Section.IsDirective(name, TokenParser.SectionPrefix, TokenParser.SectionSuffix);
        }

        public bool SectionStartsWithPrefix(SectionContext<TokenParserState> context, string prefix)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Section.StartsWith(prefix);
        }

        public ICollection<string> Keys => _state.Keys;

        public ICollection<object> Values => _state.Values;

        public int Count => _state.Count;

        public bool IsReadOnly => _state.IsReadOnly;

        public object this[string key]
        {
            get => _state[key];
            set => _state[key] = value;
        }

        public void Add(string key, object value) => _state.Add(key, value);

        public bool ContainsKey(string key) => _state.ContainsKey(key);

        public bool Remove(string key) => _state.Remove(key);

        public bool TryGetValue(string key, out object value) => _state.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, object> item) => _state.Add(item);

        public void Clear() => _state.Clear();

        public bool Contains(KeyValuePair<string, object> item) => _state.Contains(item);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _state.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, object> item) => _state.Remove(item);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _state.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_state).GetEnumerator();
    }
}
