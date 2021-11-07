using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class GroupedTokenMapperTypeProvider : IGroupedTokenMapperTypeProvider
    {
        private readonly IGroupedTokenMapperTypeContainer[] _groupedTokenMapperTypeContainers;

        public GroupedTokenMapperTypeProvider(IEnumerable<IGroupedTokenMapperTypeContainer> groupedTokenMapperTypeContainers)
            => _groupedTokenMapperTypeContainers = groupedTokenMapperTypeContainers?.ToArray() ?? Array.Empty<IGroupedTokenMapperTypeContainer>();

        public IEnumerable<IGrouping<Type, Type>> GetGroups()
            => _groupedTokenMapperTypeContainers.Reverse().Select(x => x.Types);
    }
}
