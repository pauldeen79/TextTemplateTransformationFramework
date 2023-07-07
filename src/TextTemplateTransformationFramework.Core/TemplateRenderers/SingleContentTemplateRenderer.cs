namespace TextTemplateTransformationFramework.Core.TemplateRenderers;

internal static class SingleContentTemplateRenderer
{
    internal static void Render(object template, StringBuilder builder)
    {
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
}
