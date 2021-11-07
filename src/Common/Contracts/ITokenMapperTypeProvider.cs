using System;
using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITokenMapperTypeProvider
    {
        IEnumerable<Type> GetTypes();
    }
}
