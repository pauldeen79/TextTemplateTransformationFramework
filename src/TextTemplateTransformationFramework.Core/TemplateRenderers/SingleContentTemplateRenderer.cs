namespace TextTemplateTransformationFramework.Core.TemplateRenderers;

internal class SingleContentTemplateRenderer : ITemplateRenderer
{
    public void Render(object template, object generationEnvironment, string defaultFilename)
    {
        Guard.IsNotNull(generationEnvironment);
        Guard.IsAssignableToType<StringBuilder>(generationEnvironment);

        var builder = (StringBuilder)generationEnvironment;

        if (template is ITemplate typedTemplate)
        {
            typedTemplate.Render(builder);
        }
        else if (template is ITextTransformTemplate textTransformTemplate)
        {
            var output = textTransformTemplate.TransformText();
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }
        }
        else
        {
            var output = template.ToString();
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }
        }
    }

    public bool Supports(object generationEnvironment) => generationEnvironment is StringBuilder;
}
