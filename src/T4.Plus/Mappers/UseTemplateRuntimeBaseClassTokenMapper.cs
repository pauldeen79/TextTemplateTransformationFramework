using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Runtime;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(UseTemplateRuntimeBaseClassDirectiveModel))]
    [DirectivePrefix("useTemplateRuntimeBaseClass")]
    public sealed class UseTemplateRuntimeBaseClassTokenMapper<TState> : IMultipleTokenMapper<TState, UseTemplateRuntimeBaseClassDirectiveModel>, IContextValidatableTokenMapper<TState, UseTemplateRuntimeBaseClassDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, UseTemplateRuntimeBaseClassDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, UseTemplateRuntimeBaseClassDirectiveModel model)
        {
            if (model != null)
            {
                yield return new BaseClassInheritsFromToken<TState>(context, typeof(T4PlusGeneratedTemplateBase).FullName);
                yield return new NamespaceImportToken<TState>(context, "TextTemplateTransformationFramework.Runtime");
                yield return new SkipInitializationCodeToken<TState>(context);
                if (model.AddReference)
                {
                    yield return new ReferenceToken<TState>(context, "TextTemplateTransformationFramework.Runtime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                }
            }
        }
    }
}
