using System;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class GroupedTokenMapperTypeContainer : IGroupedTokenMapperTypeContainer
    {
        public GroupedTokenMapperTypeContainer(IGrouping<Type, Type> types)
        {
            Types = types ?? throw new ArgumentNullException(nameof(types));
        }

        public IGrouping<Type, Type> Types { get; }
    }
}
