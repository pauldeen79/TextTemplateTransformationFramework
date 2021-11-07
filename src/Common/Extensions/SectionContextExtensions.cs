using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using Utilities;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class SectionContextExtensions
    {
        public static IErrorToken<TState> CreateErrorToken<TState>(this SectionContext<TState> context, string errorMessage)
            where TState : class
            => Pattern.Match
            (
                Clause.Create<int, IErrorToken<TState>>(i => i == ModePosition.Render, _ => new RenderErrorToken<TState>(context, errorMessage)),
                Clause.Create<int, IErrorToken<TState>>(i => i == ModePosition.ClassFooter, _ => new ClassFooterErrorToken<TState>(context, errorMessage)),
                Clause.Create<int, IErrorToken<TState>>(i => i == ModePosition.BaseClassFooter, _ => new BaseClassFooterErrorToken<TState>(context, errorMessage)),
                Clause.Create<int, IErrorToken<TState>>(i => i == ModePosition.NamespaceFooter, _ => new NamespaceFooterErrorToken<TState>(context, errorMessage)),
                Clause.Create<int, IErrorToken<TState>>(i => i == ModePosition.Initialize, _ => new InitializeErrorToken<TState>(context, errorMessage))
            )
            .Default(() => new RenderErrorToken<TState>(context, "Unsupported mode: " + context.CurrentMode))
            .Evaluate(context.GetModePosition());

        public static IWarningToken<TState> CreateWarningToken<TState>(this SectionContext<TState> context, string errorMessage)
            where TState : class
            => Pattern.Match
            (
                Clause.Create<int, IWarningToken<TState>>(i => i == ModePosition.Render, _ => new RenderWarningToken<TState>(context, errorMessage)),
                Clause.Create<int, IWarningToken<TState>>(i => i == ModePosition.ClassFooter, _ => new ClassFooterWarningToken<TState>(context, errorMessage)),
                Clause.Create<int, IWarningToken<TState>>(i => i == ModePosition.BaseClassFooter, _ => new BaseClassFooterWarningToken<TState>(context, errorMessage)),
                Clause.Create<int, IWarningToken<TState>>(i => i == ModePosition.NamespaceFooter, _ => new NamespaceFooterWarningToken<TState>(context, errorMessage)),
                Clause.Create<int, IWarningToken<TState>>(i => i == ModePosition.Initialize, _ => new InitializeWarningToken<TState>(context, errorMessage))
            )
            .Default(() => new RenderWarningToken<TState>(context, "Unsupported mode: " + context.CurrentMode))
            .Evaluate(context.GetModePosition());

        public static ITemplateToken<TState> CreateTextToken<TState>(this SectionContext<TState> context, string contents, bool force = true)
            where TState : class
            => !force && !context.CurrentMode.IsTextRange()
                ? null
                : Pattern.Match
                    (
                        Clause.Create<int, ITemplateToken<TState>>(i => i == ModePosition.Render, _ => new RenderTextToken<TState>(context, contents)),
                        Clause.Create<int, ITemplateToken<TState>>(i => i == ModePosition.ClassFooter, _ => new ClassFooterTextToken<TState>(context, contents)),
                        Clause.Create<int, ITemplateToken<TState>>(i => i == ModePosition.BaseClassFooter, _ => new BaseClassFooterTextToken<TState>(context, contents)),
                        Clause.Create<int, ITemplateToken<TState>>(i => i == ModePosition.NamespaceFooter, _ => new NamespaceFooterTextToken<TState>(context, contents)),
                        Clause.Create<int, ITemplateToken<TState>>(i => i == ModePosition.Initialize, _ => new InitializeTextToken<TState>(context, contents))
                    )
                    .Default(() => GetDefaultRenderErrorToken<TState>(context, force))
                    .Evaluate(context.GetModePosition());

        private static RenderErrorToken<TState> GetDefaultRenderErrorToken<TState>(SectionContext<TState> context, bool force)
            where TState : class
            => force
                ? new RenderErrorToken<TState>(context, "Unsupported mode: " + context.CurrentMode)
                : null;

        public static IExpressionToken<TState> CreateExpressionToken<TState>(this SectionContext<TState> context, string expression)
            where TState : class
            => !context.CurrentMode.IsExpressionRange()
                ? null
                : Pattern.Match
                    (
                        Clause.Create<int, IExpressionToken<TState>>(i => i == ModePosition.Render, _ => new RenderExpressionToken<TState>(context, expression)),
                        Clause.Create<int, IExpressionToken<TState>>(i => i == ModePosition.ClassFooter, _ => new ClassFooterExpressionToken<TState>(context, expression)),
                        Clause.Create<int, IExpressionToken<TState>>(i => i == ModePosition.BaseClassFooter, _ => new BaseClassFooterExpressionToken<TState>(context, expression)),
                        Clause.Create<int, IExpressionToken<TState>>(i => i == ModePosition.NamespaceFooter, _ => new NamespaceFooterExpressionToken<TState>(context, expression)),
                        Clause.Create<int, IExpressionToken<TState>>(i => i == ModePosition.Initialize, _ => new InitializeExpressionToken<TState>(context, expression))
                    )
                    .Default(() => null)
                    .Evaluate(context.GetModePosition());

        public static int GetModePosition<TState>(this SectionContext<TState> context)
            where TState : class
            => context.CurrentMode < 1000
                ? ModePosition.Render
                : int.Parse(context.CurrentMode.ToString().Right(1));

        public static T InContext<TState, T>(this SectionContext<TState> context, string childTemplateFileName, Func<T> functionDelegate)
            where TState : class
        {
            if (functionDelegate == null)
            {
                throw new ArgumentNullException(nameof(functionDelegate));
            }

            context = context.WithFileName(childTemplateFileName);
            context.TokenParserCallback.SetCustomSectionProcessors(context.CustomSectionProcessors);
            try
            {
                return functionDelegate();
            }
            finally
            {
                context.TokenParserCallback.SetCustomSectionProcessors(null);
            }
        }
    }
}
