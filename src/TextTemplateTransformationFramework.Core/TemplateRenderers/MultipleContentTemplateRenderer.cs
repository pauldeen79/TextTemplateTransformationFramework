namespace TemplateFramework.Core.TemplateRenderers;

public class MultipleContentTemplateRenderer : ITemplateRenderer
{
    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is MultipleContentBuilderEnvironment or MultipleContentBuilderContainerEnvironment;

    public void Render(object template, IGenerationEnvironment generationEnvironment, string defaultFilename)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(defaultFilename);

        IMultipleContentBuilder multipleContentBuilder;
        if (generationEnvironment is MultipleContentBuilderContainerEnvironment containerEnvironment)
        {
            // Use TemplateFileManager
            multipleContentBuilder = containerEnvironment.Builder.MultipleContentBuilder
                ?? throw new ArgumentException("MultipleContentBuilder property is null", nameof(generationEnvironment));
        }
        else if (generationEnvironment is MultipleContentBuilderEnvironment builderEnvironment)
        {
            multipleContentBuilder = builderEnvironment.Builder;
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
        new SingleContentTemplateRenderer().Render(template, new StringBuilderEnvironment(stringBuilder), defaultFilename);
        var builderResult = stringBuilder.ToString();

        if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">", StringComparison.InvariantCulture))
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
