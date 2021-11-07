using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootConstructorTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.TokenConverterTokens
{
    public class RegisterChildObjectTokenConverterToken<TState> : TemplateToken<TState>, ITokenConverterToken<TState>
        where TState : class
    {
        public RegisterChildObjectTokenConverterToken(SectionContext<TState> context)
            : base(context)
        {
        }

        public IEnumerable<ITemplateToken<TState>> Convert(IEnumerable<ITemplateToken<TState>> result)
            => result.Select(ConvertToken);

        private ITemplateToken<TState> ConvertToken(ITemplateToken<TState> token)
        {
            if (token is IRegisterChildTemplateToken<TState> ct)
            {
                return new RegisterComposableChildTemplateToken<TState>(ct);
            }

            if (token is IRegisterViewModelToken<TState> vt)
            {
                return new RegisterComposableViewModelToken<TState>(vt);
            }

            return token;
        }
    }
}
