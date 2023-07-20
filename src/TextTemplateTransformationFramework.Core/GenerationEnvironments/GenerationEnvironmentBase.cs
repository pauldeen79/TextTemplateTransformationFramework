﻿namespace TemplateFramework.Core.GenerationEnvironments;

internal abstract class GenerationEnvironmentBase : IGenerationEnvironment
{
    protected GenerationEnvironmentBase(GenerationEnvironmentType type)
    {
        Type = type;
    }

    public GenerationEnvironmentType Type { get; }
}
