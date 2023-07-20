namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class MultipleContentBuilderContainerEnvironment : GenerationEnvironmentBase
{
    public MultipleContentBuilderContainerEnvironment(IMultipleContentBuilderContainer builder) : base(GenerationEnvironmentType.MultipleContentBuilderContainer)
    {
        Guard.IsNotNull(builder);

        Builder = builder;
    }

    public IMultipleContentBuilderContainer Builder { get; }
}
