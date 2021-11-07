using System;
using System.Reflection;
using System.Text;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateProxy : ITemplateProxy
    {
        private readonly ITemplateInitializeMethodParameterProvider _templateInitializeMethodParametersProvider;

        public TemplateProxy(ITemplateInitializeMethodParameterProvider templateInitializeMethodParametersProvider)
        {
            _templateInitializeMethodParametersProvider = templateInitializeMethodParametersProvider ?? throw new ArgumentNullException(nameof(templateInitializeMethodParametersProvider));
        }

        public void Initialize(object template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var initializeMethod = template.GetType().GetMethod("Initialize");
            if (initializeMethod == null)
            {
                return;
            }

            initializeMethod.Invoke(template, GetInitializeParameters(initializeMethod, template));
        }

        public void Render(object template, StringBuilder stringBuilder)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (stringBuilder == null)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            var templateType = template.GetType();
            var renderMethod = templateType.GetMethod("Render");
            if (renderMethod != null)
            {
                renderMethod.Invoke(template, new[] { stringBuilder });
            }
            else
            {
                var transformTextMethod = templateType.GetMethod("TransformText");
                if (transformTextMethod != null)
                {
                    stringBuilder.Append((string)transformTextMethod.Invoke(template, Array.Empty<object>()));
                }
                else
                {
                    var toStringMethod = templateType.GetMethod("ToString");
                    stringBuilder.Append((string)toStringMethod.Invoke(template, Array.Empty<object>()));
                }
            }
        }

        private object[] GetInitializeParameters(MethodInfo initializeMethod, object template)
            => _templateInitializeMethodParametersProvider.Get(initializeMethod, template);
    }
}
