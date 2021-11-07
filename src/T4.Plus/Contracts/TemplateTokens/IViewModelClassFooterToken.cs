using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a token that needs to be placed in the view model class footer.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IViewModelClassFooterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
