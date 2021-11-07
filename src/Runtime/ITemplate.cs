using System.Text;

namespace TextTemplateTransformationFramework.Runtime
{
    public interface ITemplate
    {
        void Initialize();
        void Render(StringBuilder builder);
    }
}
