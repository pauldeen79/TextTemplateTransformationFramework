using System.Text;

namespace TextTemplateTransformationFramework.Abstractions
{
    public interface ITemplateRenderer
    {
        void Render(object template,
                    StringBuilder generationEnvironment,
                    string defaultFileName = "",
                    object? model = null,
                    object? additionalParameters = null);

        void Render(object template,
                    IMultipleContentBuilder generationEnvironment,
                    string defaultFileName = "",
                    object? model = null,
                    object? additionalParameters = null);

        void Render(object template,
                    IMultipleContentBuilderContainer generationEnvironment,
                    string defaultFileName = "",
                    object? model = null,
                    object? additionalParameters = null);
    }
}
