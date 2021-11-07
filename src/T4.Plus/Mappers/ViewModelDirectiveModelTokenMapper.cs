using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(ViewModelDirectiveModel))]
    [DirectivePrefix("viewmodel")]
    public class ViewModelDirectiveModelTokenMapper<TState> : IMultipleTokenMapper<TState, ViewModelDirectiveModel>
        where TState : class
    {
        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, ViewModelDirectiveModel model)
        {
            if (context != null && model != null)
            {
                yield return new ParameterToken<TState>
                (
                    context,
                    "ViewModel",
                    model.Name.Sanitize(),
                    addPropertySetter: true,
                    omitValueAssignment: true,
                    omitInitialization: true
                );
                if (!string.IsNullOrEmpty(model.Name))
                {
                    if (context.TokenParserCallback.IsChildTemplate)
                    {
                        yield return new ChildTemplateInitializeViewModelToken<TState>
                        (
                            context,
                            model.Name,
                            model.NameIsLiteral,
                            model.Model,
                            model.ModelIsLiteral,
                            model.SilentlyContinueOnError,
                            model.CustomResolverDelegate,
                            model.CustomResolverDelegateIsLiteral,
                            model.ResolverDelegateModel,
                            model.ResolverDelegateModelIsLiteral
                        );
                    }
                    else
                    {
                        yield return new RootTemplateInitializeViewModelToken<TState>
                        (
                            context,
                            model.Name,
                            model.NameIsLiteral,
                            model.Model,
                            model.ModelIsLiteral,
                            model.SilentlyContinueOnError,
                            model.CustomResolverDelegate,
                            model.CustomResolverDelegateIsLiteral,
                            model.ResolverDelegateModel,
                            model.ResolverDelegateModelIsLiteral
                        );
                    }
                }
            }
        }
    }
}
