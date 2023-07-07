namespace TextTemplateTransformationFramework.Core.TemplateRenderers;

internal static class MultipleContentTemplateRenderer
{
    internal static void Render(object template, object multipleContentBuilder, string defaultFilename)
    {
        if (multipleContentBuilder is IMultipleContentBuilderContainer container)
        {
            // Use TemplateFileManager
            multipleContentBuilder = container.MultipleContentBuilder
                ?? throw new InvalidOperationException("MultipleContentBuilder property is null");
        }

        // Note that we can safely cast here, as the public method only accepts IMultipleContentBuilder and IMultipleContentBuilderContainer
        var builder = (IMultipleContentBuilder)multipleContentBuilder;

        if (template is IMultipleContentBuilderTemplate multipleContentBuilderTemplate)
        {
            // No need to convert string to MultipleContentBuilder, and then add it again..
            // We can simply pass the MultipleContentBuilder instance
            multipleContentBuilderTemplate.Render(builder);
            return;
        }

        var stringBuilder = new StringBuilder();
        SingleContentTemplateRenderer.Render(template, stringBuilder);
        var builderResult = stringBuilder.ToString();

        if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
        {
            var multipleContents = MultipleContentBuilder.FromString(builderResult);
            foreach (var content in multipleContents.Contents.Select(x => x.Build()))
            {
                builder.AddContent(content.Filename, content.SkipWhenFileExists, content.Builder);
            }
        }
        else
        {
            builder.AddContent(defaultFilename, false, new StringBuilder(builderResult));
        }
    }
}
