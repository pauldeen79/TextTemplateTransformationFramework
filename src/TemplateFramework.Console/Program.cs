using var app = new CommandLineApplication
{
    Name = "tf",
    Description = "TemplateFramework",
    UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue
};
app.HelpOption();

var serviceCollection = new ServiceCollection()
    .AddTemplateFramework()
    .AddTemplateCommands();
using var provider = serviceCollection.BuildServiceProvider();
var processor = provider.GetRequiredService<ICommandLineProcessor>();
processor.Initialize(app);
return app.Execute(args);
