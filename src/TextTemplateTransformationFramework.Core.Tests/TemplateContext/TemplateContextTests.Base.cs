namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    protected TemplateContext CreateSut()
    {
        var rootTemplateContext = new TemplateContext(template: this, model: 1, viewModel: 2);
        var parentTemplateContext = new TemplateContext(template: this, parentContext: rootTemplateContext, model: "test model", viewModel: "test viewmodel");
        return new TemplateContext(template: this, parentContext: parentTemplateContext);
    }

}
