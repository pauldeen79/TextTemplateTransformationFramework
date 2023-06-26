namespace TextTemplateTransformationFramework.Abstractions;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider provider, ICodeGenerationSettings settings);
}
