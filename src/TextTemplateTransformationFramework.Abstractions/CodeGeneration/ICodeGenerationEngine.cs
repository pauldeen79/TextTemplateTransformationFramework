namespace TemplateFramework.Abstractions.CodeGeneration;

public interface ICodeGenerationEngine
{
    void Generate(ICodeGenerationProvider provider, ICodeGenerationSettings settings);
}

public interface ICodeGenerationEngine<in T>
{
    void Generate(ICodeGenerationProvider<T> provider, ICodeGenerationSettings settings);
}
