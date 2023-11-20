using System;

namespace TextTemplateTransformationFramework.Common.Data
{
    public class SectionProcessResultData<TState>
        where TState : class
    {
        public SectionContext<TState> Context { get; set; }
        public object Model { get; set; }
        public Func<SectionContext<TState>, object, bool> IsValidDelegate { get; set; }
        public Func<object> MapDelegate { get; set; }
        public int? SwitchToMode { get; set; }
        public bool PassThrough { get; set; }
        public bool TokensAreForRootTemplateSection { get; set; }
        public SectionProcessResult<TState> ExistingResult { get; set; }
        public string DirectiveName { get; set; }

        public SectionProcessResultData<TState> WithContext(SectionContext<TState> context)
        {
            Context = context;
            return this;
        }

        public SectionProcessResultData<TState> WithModel(object model)
        {
            Model = model;
            return this;
        }

        public SectionProcessResultData<TState> WithIsValidDelegate(Func<SectionContext<TState>, object, bool> isValidDelegate)
        {
            IsValidDelegate = isValidDelegate;
            return this;
        }

        public SectionProcessResultData<TState> WithMapDelegate(Func<object> mapDelegate)
        {
            MapDelegate = mapDelegate;
            return this;
        }

        public SectionProcessResultData<TState> WithSwitchToMode(int? switchToMode)
        {
            SwitchToMode = switchToMode;
            return this;
        }

        public SectionProcessResultData<TState> WithPassThrough(bool passThrough)
        {
            PassThrough = passThrough;
            return this;
        }

        public SectionProcessResultData<TState> WithTokensAreForRootTemplateSection(bool tokensAreForRootTemplateSection)
        {
            TokensAreForRootTemplateSection = tokensAreForRootTemplateSection;
            return this;
        }

        public SectionProcessResultData<TState> WithExistingResult(SectionProcessResult<TState> existingResult)
        {
            ExistingResult = existingResult;
            return this;
        }

        public SectionProcessResultData<TState> WithDirectiveName(string directiveName)
        {
            DirectiveName = directiveName;
            return this;
        }

        public void Validate()
        {
            if (Context is null)
            {
                throw new InvalidOperationException($"{nameof(Context)} is required");
            }

            if (Model is null)
            {
                throw new InvalidOperationException($"{nameof(Model)} is required");
            }

            if (IsValidDelegate is null)
            {
                throw new InvalidOperationException($"{nameof(IsValidDelegate)} is required");
            }

            if (MapDelegate is null)
            {
                throw new InvalidOperationException($"{nameof(MapDelegate)} is required");
            }
        }
    }
}
