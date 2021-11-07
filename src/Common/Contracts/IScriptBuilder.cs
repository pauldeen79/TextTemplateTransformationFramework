using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IScriptBuilder<TState>
        where TState : class
    {
        string Build(ITemplateSectionProcessor<TState> templateSectionProcessor, params object[] models);
        IEnumerable<ITemplateSectionProcessor<TState>> GetKnownDirectives();
    }
}