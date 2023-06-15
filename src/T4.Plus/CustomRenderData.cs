using System;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class CustomRenderData
    {
        public CustomRenderData(ValueSpecifier customResolverDelegate, ValueSpecifier resolverDelegateModel, ValueSpecifier customRenderChildTemplateDelegate, ValueSpecifier customTemplateNameDelegate)
        {
            CustomResolverDelegate = customResolverDelegate ?? throw new ArgumentNullException(nameof(customResolverDelegate));
            ResolverDelegateModel = resolverDelegateModel ?? throw new ArgumentNullException(nameof(resolverDelegateModel));
            CustomRenderChildTemplateDelegate = customRenderChildTemplateDelegate ?? throw new ArgumentNullException(nameof(customRenderChildTemplateDelegate));
            CustomTemplateNameDelegate = customTemplateNameDelegate ?? throw new ArgumentNullException(nameof(customTemplateNameDelegate));
        }

        public ValueSpecifier CustomResolverDelegate { get; }
        public ValueSpecifier ResolverDelegateModel { get; }
        public ValueSpecifier CustomRenderChildTemplateDelegate { get; }
        public ValueSpecifier CustomTemplateNameDelegate { get; }
    }
}
