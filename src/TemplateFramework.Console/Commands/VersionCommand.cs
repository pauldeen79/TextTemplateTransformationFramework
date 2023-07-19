namespace TemplateFramework.Console.Commands;

[ExcludeFromCodeCoverage]
public class VersionCommand : ICommandLineCommand
{
    public void Initialize(CommandLineApplication app)
    {
        Guard.IsNotNull(app);
        app.VersionOptionFromAssemblyAttributes(GetType().Assembly);
    }
}
