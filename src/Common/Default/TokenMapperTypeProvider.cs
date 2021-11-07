using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TokenMapperTypeProvider : ITokenMapperTypeProvider
    {
        private readonly ITokenMapperTypeContainer[] _tokenMapperTypeContainers;

        public TokenMapperTypeProvider(IEnumerable<ITokenMapperTypeContainer> tokenMapperTypeContainers)
            => _tokenMapperTypeContainers = tokenMapperTypeContainers?.ToArray() ?? Array.Empty<ITokenMapperTypeContainer>();

        public IEnumerable<Type> GetTypes()
            => _tokenMapperTypeContainers.Reverse().Select(x => x.Type);
    }
}
