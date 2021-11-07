using System;
using System.Linq;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IGroupedTokenMapperTypeContainer
    {
        IGrouping<Type, Type> Types { get; }
    }
}
