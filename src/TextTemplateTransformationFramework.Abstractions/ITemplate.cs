using System.Text;

namespace TextTemplateTransformationFramework.Abstractions
{
    public interface ITemplate
    {
        void Render(StringBuilder builder);
    }
}
