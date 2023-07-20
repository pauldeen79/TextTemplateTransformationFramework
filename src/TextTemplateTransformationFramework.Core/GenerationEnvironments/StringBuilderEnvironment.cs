namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class StringBuilderEnvironment : GenerationEnvironmentBase
{
    public StringBuilderEnvironment(StringBuilder builder) : base(GenerationEnvironmentType.StringBuilder)
    {
        Guard.IsNotNull(builder);

        Builder = builder;
    }

    public StringBuilder Builder { get; }
}
