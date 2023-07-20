namespace TemplateFramework.Core.TemplateRenderers;

public class SingleContentTemplateRenderer : ITemplateRenderer
{
    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is StringBuilderEnvironment;
    
    public void Render(object template, IGenerationEnvironment generationEnvironment, string defaultFilename)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsAssignableToType<StringBuilderEnvironment>(generationEnvironment);

        var builder = ((StringBuilderEnvironment)generationEnvironment).Builder;

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
