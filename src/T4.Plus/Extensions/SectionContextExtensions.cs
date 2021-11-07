using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class SectionContextExtensions
    {
        public static ITemplateToken<TState> CreateRenderChildTemplateToken<TState>(this SectionContext<TState> context,
                                                                                    string childTemplateName,
                                                                                    bool childTemplateNameIsLiteral,
                                                                                    string model,
                                                                                    bool modelIsLiteral,
                                                                                    bool? enumerable,
                                                                                    bool silentlyContinueOnError,
                                                                                    string separatorTemplateName,
                                                                                    bool separatorTemplateNameIsLiteral,
                                                                                    string headerTemplateName,
                                                                                    bool headerTemplateNameIsLiteral,
                                                                                    string headerCondition,
                                                                                    string footerTemplateName,
                                                                                    bool footerTemplateNameIsLiteral,
                                                                                    string footerCondition,
                                                                                    string customResolverDelegate,
                                                                                    bool customResolverDelegateIsLiteral,
                                                                                    string resolverDelegateModel,
                                                                                    bool resolverDelegateModelIsLiteral,
                                                                                    string customRenderChildTemplateDelegateExpression,
                                                                                    bool customRenderChildTemplateDelegateExpressionIsLiteral,
                                                                                    string customTemplateNameDelegateExpression,
                                                                                    bool customTemplateNameDelegateExpressionIsLiteral)
            where TState : class
            => Utilities.Pattern.Match
            (
                Utilities.Clause.Create<int, IRenderToken<TState>>(i => i == ModePosition.Render, _ => new RenderChildTemplateToken<TState>(context,
                                                                                                                                            childTemplateName,
                                                                                                                                            childTemplateNameIsLiteral,
                                                                                                                                            model,
                                                                                                                                            modelIsLiteral,
                                                                                                                                            enumerable,
                                                                                                                                            silentlyContinueOnError,
                                                                                                                                            separatorTemplateName,
                                                                                                                                            separatorTemplateNameIsLiteral,
                                                                                                                                            headerTemplateName,
                                                                                                                                            headerTemplateNameIsLiteral,
                                                                                                                                            headerCondition,
                                                                                                                                            footerTemplateName,
                                                                                                                                            footerTemplateNameIsLiteral,
                                                                                                                                            footerCondition,
                                                                                                                                            customResolverDelegate,
                                                                                                                                            customResolverDelegateIsLiteral,
                                                                                                                                            resolverDelegateModel,
                                                                                                                                            resolverDelegateModelIsLiteral,
                                                                                                                                            customRenderChildTemplateDelegateExpression,
                                                                                                                                            customRenderChildTemplateDelegateExpressionIsLiteral,
                                                                                                                                            customTemplateNameDelegateExpression,
                                                                                                                                            customTemplateNameDelegateExpressionIsLiteral))
            )
            .Default(() => new RenderErrorToken<TState>(context, "Unsupported mode: " + context.CurrentMode))
            .Evaluate(context.GetModePosition());

        public static ChildTemplateTokenInfo<TState> GetChildTemplateTokens<TState>(this SectionContext<TState> context,
                                                                                    IFileContentsProvider fileContentsProvider,
                                                                                    string childTemplateFileName)
            where TState : class
        {
            var childTemplateTokens = new List<ITemplateToken<TState>>();
            var rootTemplateTokens = new List<ITemplateToken<TState>>();

            if (!string.IsNullOrEmpty(childTemplateFileName))
            {
                context = context.WithFileName(childTemplateFileName);
                context.TokenParserCallback.SetCustomSectionProcessors(context.CustomSectionProcessors);
                var sectionTokens = context
                    .InContext
                    (
                        childTemplateFileName,
                        () => context.TokenParserCallback
                                     .Parse
                                     (
                                         new TextTemplateProcessorContext<TState>
                                         (
                                             new TextTemplate
                                             (
                                                 fileContentsProvider.GetFileContents(childTemplateFileName),
                                                 childTemplateFileName
                                             ),
                                             Array.Empty<TemplateParameter>(),
                                             context.Logger
                                         ).SetCustomSectionProcessors(context.CustomSectionProcessors)
                                     )
                                     .Cast<ISourceSectionToken<TState>>()
                    );
                foreach (var sectionToken in sectionTokens)
                {
                    var tokens = sectionToken.TemplateTokens;
                    if (sectionToken.IsRootTemplateSection)
                    {
                        rootTemplateTokens.AddRange(tokens);
                    }
                    else
                    {
                        childTemplateTokens.AddRange(tokens);
                    }
                }
            }

            return new ChildTemplateTokenInfo<TState>(childTemplateTokens, rootTemplateTokens);
        }

        public static string GetRootClassName<TState>(this SectionContext<TState> context)
            where TState : class
            => context.ExistingTokens.GetRootClassName();

        public static string GetClassName<TState>(this SectionContext<TState> context)
            where TState : class
            => context.ExistingTokens.GetClassName();

        public static ITextTemplateProcessorContext<TState> GetTextTemplateProcessorContext<TState>(this SectionContext<TState> context)
            where TState : class
        {
            if (context is SectionContext<TokenParserState> tokenParserStateSectionContext)
            {
                return (ITextTemplateProcessorContext<TState>)tokenParserStateSectionContext.State.Context;
            }

            throw new InvalidOperationException($"Could not obtain TextTemplateProcessorContext of type [{typeof(TState).FullName}]");
        }
    }
}
