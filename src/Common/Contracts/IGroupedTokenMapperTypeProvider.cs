using System;
using System.Collections.Generic;
using System.Linq;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IGroupedTokenMapperTypeProvider
    {
        IEnumerable<IGrouping<Type, Type>> GetGroups();
    }
}
