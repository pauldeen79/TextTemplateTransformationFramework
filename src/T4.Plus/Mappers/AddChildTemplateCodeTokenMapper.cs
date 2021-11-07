using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.TokenConverterTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(AddChildTemplateCodeDirectiveModel))]
    [DirectivePrefix("addChildTemplateCode")]
    public sealed class AddChildTemplateCodeTokenMapper<TState> : IMultipleTokenMapper<TState, AddChildTemplateCodeDirectiveModel>, IContextValidatableTokenMapper<TState, AddChildTemplateCodeDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, AddChildTemplateCodeDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, AddChildTemplateCodeDirectiveModel model)
        {
            if (model != null)
            {
                if (!model.Override)
                {
                    if (model.Composable)
                    {
                        yield return new AddComposableChildTemplateCodeToken<TState>(context);
                    }
                    else
                    {
                        yield return new AddChildTemplateCodeToken<TState>(context);
                        yield return new ClearPlaceholderTemplatesFieldToken<TState>(context);
                        yield return new ClearChildTemplatesFieldToken<TState>(context);
                    }
                }
                else if (model.ClearFieldsOnOverride && !model.Composable)
                {
                    yield return new ClearPlaceholderTemplatesFieldToken<TState>(context);
                    yield return new ClearChildTemplatesFieldToken<TState>(context);
                }

                if (model.Composable)
                {
                    yield return new CompositionRootClassToken<TState>(context,
                                                                       context.GetClassName(),
                                                                       model.ComposableRegistrationMethodsAccessor);
                    yield return new RegisterChildObjectTokenConverterToken<TState>(context);
                }
                else
                {
                    yield return new RouteChildTemplatesFieldToRootTemplateToken<TState>(context);
                    yield return new RouteChildPlaceholderChildrenDictionaryFieldToRootTemplateToken<TState>(context);
                }
            }
        }
    }
}
