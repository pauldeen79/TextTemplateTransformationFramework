using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class SectionContextExtensions
    {
        public static IErrorToken<TState> CreateErrorToken<TState>(this SectionContext<TState> context, string errorMessage)
            where TState : class
            => context.GetModePosition() switch
            {
                ModePosition.Render => new RenderErrorToken<TState>(context, errorMessage),
                ModePosition.ClassFooter => new ClassFooterErrorToken<TState>(context, errorMessage),
                ModePosition.BaseClassFooter => new BaseClassFooterErrorToken<TState>(context, errorMessage),
                ModePosition.NamespaceFooter => new NamespaceFooterErrorToken<TState>(context, errorMessage),
                ModePosition.Initialize => new InitializeErrorToken<TState>(context, errorMessage),
                _ => new RenderErrorToken<TState>(context, "Unsupported mode: " + context.CurrentMode)
            };

        public static IWarningToken<TState> CreateWarningToken<TState>(this SectionContext<TState> context, string errorMessage)
            where TState : class
            => context.GetModePosition() switch
            {
                ModePosition.Render => new RenderWarningToken<TState>(context, errorMessage),
                ModePosition.ClassFooter => new ClassFooterWarningToken<TState>(context, errorMessage),
                ModePosition.BaseClassFooter => new BaseClassFooterWarningToken<TState>(context, errorMessage),
                ModePosition.NamespaceFooter => new NamespaceFooterWarningToken<TState>(context, errorMessage),
                ModePosition.Initialize => new InitializeWarningToken<TState>(context, errorMessage),
                _ => new RenderWarningToken<TState>(context, "Unsupported mode: " + context.CurrentMode)
            };

        public static ITemplateToken<TState> CreateTextToken<TState>(this SectionContext<TState> context, string contents, bool force = true)
            where TState : class
            => !force && !context.CurrentMode.IsTextRange()
                ? null
                : context.GetModePosition() switch
                    {
                        ModePosition.Render => new RenderTextToken<TState>(context, contents),
                        ModePosition.ClassFooter => new ClassFooterTextToken<TState>(context, contents),
                        ModePosition.BaseClassFooter => new BaseClassFooterTextToken<TState>(context, contents),
                        ModePosition.NamespaceFooter => new NamespaceFooterTextToken<TState>(context, contents),
                        ModePosition.Initialize => new InitializeTextToken<TState>(context, contents),
                        _ => GetDefaultRenderErrorToken<TState>(context, force)
                    };

        private static RenderErrorToken<TState> GetDefaultRenderErrorToken<TState>(SectionContext<TState> context, bool force)
            where TState : class
            => force
                ? new RenderErrorToken<TState>(context, "Unsupported mode: " + context.CurrentMode)
                : null;

        public static IExpressionToken<TState> CreateExpressionToken<TState>(this SectionContext<TState> context, string expression)
            where TState : class
            => !context.CurrentMode.IsExpressionRange()
                ? null
                : context.GetModePosition() switch
                    {
                        ModePosition.Render => new RenderExpressionToken<TState>(context, expression),
                        ModePosition.ClassFooter => new ClassFooterExpressionToken<TState>(context, expression),
                        ModePosition.BaseClassFooter => new BaseClassFooterExpressionToken<TState>(context, expression),
                        ModePosition.NamespaceFooter => new NamespaceFooterExpressionToken<TState>(context, expression),
                        ModePosition.Initialize => new InitializeExpressionToken<TState>(context, expression),
                        _ => null
                    };

        public static int GetModePosition<TState>(this SectionContext<TState> context)
            where TState : class
            => context.CurrentMode < 1000
                ? ModePosition.Render
                : int.Parse(context.CurrentMode.ToString().Right(1));

        public static T InContext<TState, T>(this SectionContext<TState> context, string childTemplateFileName, Func<T> functionDelegate)
            where TState : class
        {
            if (functionDelegate is null)
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
