namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateContextTests
{
    protected TemplateContext CreateSut()
    {
        var rootTemplateContext = new TemplateContext(template: this, parentContext: null, model: 1);
        var parentTemplateContext = new TemplateContext(template: this, parentContext: rootTemplateContext, model: "test");
        return new TemplateContext(template: this, parentContext: parentTemplateContext);
    }

}
