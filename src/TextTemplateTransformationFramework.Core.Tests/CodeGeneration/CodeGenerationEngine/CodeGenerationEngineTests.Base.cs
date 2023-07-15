﻿namespace TemplateFramework.Core.Tests.CodeGeneration;

public abstract partial class CodeGenerationEngineTests
{
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateFileManager> TemplateFileManagerMock { get; } = new();
    protected Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
    protected Mock<ICodeGenerationSettings> CodeGenerationSettingsMock { get; } = new();


    protected CodeGenerationEngine CreateSut() => new(TemplateEngineMock.Object);
}
