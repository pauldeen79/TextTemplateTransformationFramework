using System;
using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining a section from which tokens are taken in the parsing process.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface ISourceSectionToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string SourceSection { get; }
        Type SectionProcessorType { get; }
        ICollection<ITemplateToken<TState>> TemplateTokens { get; }
        int Mode { get;  }
        bool IsRootTemplateSection { get; }
    }
}
