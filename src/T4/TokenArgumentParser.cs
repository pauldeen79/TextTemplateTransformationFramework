using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenArgumentParser<TState> : ITokenArgumentParser<TState>
        where TState : class
    {
        public IEnumerable<string> ParseArgument(SectionContext<TState> context, string argumentName)
            => Enumerable.Empty<string>();
    }
}
