namespace TextTemplateTransformationFramework.Abstractions
{
    public interface ITemplateRenderer
    {
        void Render(object template,
                    object generationEnvironment,
                    string defaultFileName = "",
                    object? model = null,
                    object? additionalParameters = null);
    }
}
