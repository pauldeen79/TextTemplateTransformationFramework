namespace TemplateFramework.Core.TemplateRenderers;

public class SingleContentTemplateRenderer : ITemplateRenderer
{
    public bool Supports(IGenerationEnvironment generationEnvironment) => generationEnvironment is StringBuilderEnvironment;
    
    public void Render(IRenderTemplateRequest request)
    {
        Guard.IsNotNull(request);
        Guard.IsAssignableToType<StringBuilderEnvironment>(request.GenerationEnvironment);

        var builder = ((StringBuilderEnvironment)request.GenerationEnvironment).Builder;

        if (request.Template is ITemplate typedTemplate)
        {
            typedTemplate.Render(builder);
        }
        else if (request.Template is ITextTransformTemplate textTransformTemplate)
        {
            var output = textTransformTemplate.TransformText();
            ApendIfFilled(builder, output);
        }
        else
        {
            var output = request.Template.ToString();
            ApendIfFilled(builder, output);
        }
    }

    private static void ApendIfFilled(StringBuilder builder, string? output)
    {
        if (string.IsNullOrEmpty(output))
        {
            return;
        }
        
        builder.Append(output);
    }
}
