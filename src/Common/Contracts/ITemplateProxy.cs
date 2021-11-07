using System.Text;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateProxy
    {
        void Initialize(object template);
        void Render(object template, StringBuilder stringBuilder);
    }
}
