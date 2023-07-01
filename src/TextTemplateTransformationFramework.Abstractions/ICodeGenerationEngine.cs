namespace TextTemplateTransformationFramework.Abstractions;

public interface ICodeGenerationEngine<in T>
{
    void Generate(ICodeGenerationProvider<T> provider, ICodeGenerationSettings settings);
}
