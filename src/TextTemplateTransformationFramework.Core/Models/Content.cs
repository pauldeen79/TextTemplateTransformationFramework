﻿namespace TextTemplateTransformationFramework.Core.Models;

internal sealed class Content : IContent
{
    public Content(StringBuilder builder, bool skipWhenFileExists, string filename)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNullOrWhiteSpace(filename);

        Builder = builder;
        SkipWhenFileExists = skipWhenFileExists;
        Filename = filename;
    }

    public string Filename { get; }

    public bool SkipWhenFileExists { get; }

    public StringBuilder Builder { get; }
}
