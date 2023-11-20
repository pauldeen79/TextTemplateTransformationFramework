using System;

namespace TextTemplateTransformationFramework.T4
{
    public sealed class DirectiveProcessResult<TState>
        where TState : class
    {
        public bool Understood { get; }
        public bool Cancel { get; }
        public ProcessSectionResult<TState> ProcessSectionResult { get; }

        private DirectiveProcessResult(bool understood, bool cancel, ProcessSectionResult<TState> processSectionResult)
        {
            Understood = understood;
            Cancel = cancel;
            ProcessSectionResult = processSectionResult;
        }

        public static readonly DirectiveProcessResult<TState> Empty
            = new DirectiveProcessResult<TState>(false, false, ProcessSectionResult<TState>.Empty);

        public DirectiveProcessResult<TState> With(ProcessSectionResult<TState> addTokensResult)
        {
            if (addTokensResult is null)
            {
                throw new ArgumentNullException(nameof(addTokensResult));
            }

            return new DirectiveProcessResult<TState>
                    (
                        true,
                        !addTokensResult.PassThrough,
                        ProcessSectionResult == ProcessSectionResult<TState>.Empty
                            ? addTokensResult
                            : ProcessSectionResult.With(addTokensResult.TemplateTokensSections)
                    );
        }
    }
}
