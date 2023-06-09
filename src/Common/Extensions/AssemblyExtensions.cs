using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTemplateSectionProcessorTypes<TState>(this Assembly instance)
            where TState : class
            => instance
                .GetExportedTypes()
                .Where
                (
                    t => !t.IsInterface
                    && !t.IsAbstract
                    && Array.Exists(t.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(ITemplateSectionProcessor<>)))
                    && Array.Exists(t.GetConstructors(), c => c.GetParameters().Length == 0)
                )
                .Select(t => t.GetModelType(typeof(TState)))
                .Where(t => !typeof(INonDiscoverableTemplateSectionProcessor<TState>).IsAssignableFrom(t));

        public static IEnumerable<Type> GetTokenMapperTypes(this Assembly instance)
            => instance
                .GetExportedTypes<ITokenMapper>()
                .Where(t => !t.IsInterface && !t.IsAbstract && t.GetCustomAttribute<TokenMapperAttribute>(true) != null);

        public static IEnumerable<IGrouping<Type, Type>> GetGroupedTokenMapperTypes(this Assembly instance)
            => instance
                .GetExportedTypes<ITokenMapper>()
                .Where(t => !t.IsInterface && !t.IsAbstract && t.GetCustomAttribute<GroupedTokenMapperAttribute>(true) != null)
                .GroupBy(t => t.GetCustomAttribute<GroupedTokenMapperAttribute>(true).Type);
    }
}
