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
                                                                                    RenderChildTemplateDirectiveModel model)
            where TState : class
            => Utilities.Pattern.Match
            (
                Utilities.Clause.Create<int, IRenderToken<TState>>
                (
                    i => i == ModePosition.Render,
                    _ => new RenderChildTemplateToken<TState>
                    (
                        context,
                        new ValueSpecifier(model.Name, model.NameIsLiteral),
                        new ValueSpecifier(model.Model, model.ModelIsLiteral),
                        model.Enumerable,
                        model.SilentlyContinueOnError,
                        new ValueSpecifier(model.SeparatorTemplateName, model.SeparatorTemplateNameIsLiteral),
                        new(
                            new ValueSpecifier(model.CustomResolverDelegate, model.CustomResolverDelegateIsLiteral),
                            new ValueSpecifier(model.ResolverDelegateModel, model.ResolverDelegateModelIsLiteral),
                            new ValueSpecifier(model.CustomRenderChildTemplateDelegate, model.CustomRenderChildTemplateDelegateIsLiteral),
                            new ValueSpecifier(model.CustomTemplateNameDelegate, model.CustomTemplateNameDelegateIsLiteral)
                        )
                    )
                )
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
                                             context.Logger,
                                             context
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
            => context.ExistingTokens.GetTemplateTokensFromSections().GetRootClassName();

        public static string GetClassName<TState>(this SectionContext<TState> context)
            where TState : class
            => context.ExistingTokens.GetTemplateTokensFromSections().GetClassName();

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
