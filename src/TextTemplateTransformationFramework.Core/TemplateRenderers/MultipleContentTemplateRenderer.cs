namespace TextTemplateTransformationFramework.Core.TemplateRenderers;

internal class MultipleContentTemplateRenderer : ITemplateRenderer
{
    public bool Supports(object generationEnvironment) => generationEnvironment is IMultipleContentBuilder or IMultipleContentBuilderContainer;

    public void Render(object template, object generationEnvironment, string defaultFilename)
    {
        IMultipleContentBuilder multipleContentBuilder;
        if (generationEnvironment is IMultipleContentBuilderContainer container)
        {
            // Use TemplateFileManager
            multipleContentBuilder = container.MultipleContentBuilder
                ?? throw new InvalidOperationException("MultipleContentBuilder property is null");
        }
        else if (generationEnvironment is IMultipleContentBuilder builder)
        {
            multipleContentBuilder = builder;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(generationEnvironment), "GenerationEnvironment should be of type IMultipleContentBuilder or IMultipleContentBuilderContainer");
        }

        if (template is IMultipleContentBuilderTemplate multipleContentBuilderTemplate)
        {
            // No need to convert string to MultipleContentBuilder, and then add it again..
            // We can simply pass the MultipleContentBuilder instance
            multipleContentBuilderTemplate.Render(multipleContentBuilder);
            return;
        }

        var stringBuilder = new StringBuilder();
        new SingleContentTemplateRenderer().Render(template, stringBuilder, defaultFilename);
        var builderResult = stringBuilder.ToString();

        if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
        {
            var multipleContents = MultipleContentBuilder.FromString(builderResult);
            foreach (var content in multipleContents.Contents.Select(x => x.Build()))
            {
                multipleContentBuilder.AddContent(content.Filename, content.SkipWhenFileExists, content.Builder);
            }
        }
        else
        {
            multipleContentBuilder.AddContent(defaultFilename, false, new StringBuilder(builderResult));
        }
    }
}
