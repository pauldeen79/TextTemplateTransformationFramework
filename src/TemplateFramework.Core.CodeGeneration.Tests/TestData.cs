﻿namespace TemplateFramework.Core.CodeGeneration.Tests;

internal static class TestData
{
#if Windows
    internal const string BasePath = @"C:\Somewhere";
#elif Linux
    internal const string BasePath = @"/usr/bin/python3";
#elif OSX
    internal const string BasePath = @"/Users/moi/Downloads";
#else
    internal const string BasePath = "Unknown basepath, only Windows, Linux and OSX are supported";
#endif
    
    internal static string GetAssemblyName() => typeof(TestData).Assembly.FullName!;
}

public sealed class MyGeneratorProvider : ICodeGenerationProvider
{
    public bool GenerateMultipleFiles { get; private set; }

    public bool SkipWhenFileExists { get; private set; }

    public string BasePath { get; private set; } = "";

    public string Path { get; } = "";

    public string DefaultFilename => "MyFile.txt";

    public bool RecurseOnDeleteGeneratedFiles { get; }

    public string LastGeneratedFilesFilename { get; } = "";

    public object? CreateAdditionalParameters() => null;

    public object CreateGenerator() => new MyGenerator();

    public object? CreateModel() => null;

    public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
    {
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        BasePath = basePath;
    }
}

public sealed class MyGenerator
{
    public override string ToString() => "Here is code from MyGenerator";
}
