﻿using System;

namespace TextTemplateTransformationFramework.Runtime.CodeGeneration
{
    public interface ICodeGenerationProvider
    {
        bool GenerateMultipleFiles { get; }
        bool SkipWhenFileExists { get; }
        string BasePath { get; }
        string Path { get; }
        string DefaultFileName { get; }
        bool RecurseOnDeleteGeneratedFiles { get; }
        string LastGeneratedFilesFileName { get; }
        Action AdditionalActionDelegate { get; }

        void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath);
        object CreateGenerator();
        object CreateModel();
        object CreateAdditionalParameters();
    }
}
