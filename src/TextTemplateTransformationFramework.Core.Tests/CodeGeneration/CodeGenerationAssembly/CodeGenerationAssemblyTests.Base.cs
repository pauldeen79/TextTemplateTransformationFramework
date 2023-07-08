namespace TemplateFramework.Core.Tests.CodeGeneration;

public partial class CodeGenerationAssemblyTests
{
    protected Mock<ICodeGenerationEngine> CodeGenerationEngineMock { get; } = new();
    protected string GetAssemblyName() => GetType().Assembly.FullName!;
    protected CodeGenerationAssembly CreateSut() => new(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, false);
}
