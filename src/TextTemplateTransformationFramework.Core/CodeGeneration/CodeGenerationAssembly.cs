﻿namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : ICodeGenerationAssembly
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;
    private readonly ITemplateFileManagerFactory _templateFileManagerFactory;

    public CodeGenerationAssembly(ICodeGenerationEngine codeGenerationEngine,
                                  ITemplateFileManagerFactory templateFileManagerFactory)
    {
        Guard.IsNotNull(codeGenerationEngine);
        Guard.IsNotNull(templateFileManagerFactory);

        _codeGenerationEngine = codeGenerationEngine;
        _templateFileManagerFactory = templateFileManagerFactory;
    }

    public string Generate(ICodeGenerationAssemblySettings settings)
    {
        Guard.IsNotNull(settings);
        
        var context = new CustomAssemblyLoadContext("TemplateFramework.Core.CodeGeneration", true, () => new[] { settings.CurrentDirectory });
        try
        {
            var assembly = LoadAssembly(context, settings);

            return GetOutputFromAssembly(assembly, settings);
        }
        finally
        {
            context.Unload();
        }
    }

    private Assembly LoadAssembly(AssemblyLoadContext context, ICodeGenerationAssemblySettings settings)
    {
        try
        {
            return context.LoadFromAssemblyName(new AssemblyName(settings.AssemblyName));
        }
        catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.") || e.Message.EndsWith("The system cannot find the file specified."))
        {
            if (settings.AssemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(settings.AssemblyName))
            {
                return context.LoadFromAssemblyPath(Path.Combine(settings.CurrentDirectory, Path.GetFileName(settings.AssemblyName)));
            }
            return context.LoadFromAssemblyPath(settings.AssemblyName);
        }
    }

    private string GetOutputFromAssembly(Assembly assembly, ICodeGenerationAssemblySettings settings)
    {
        var templateFileManager = _templateFileManagerFactory.Create();
        templateFileManager.MultipleContentBuilder.BasePath = settings.BasePath;

        foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly, settings.ClassNameFilter))
        {
            _codeGenerationEngine.Generate(codeGenerationProvider, templateFileManager, settings);
        }

        return templateFileManager.MultipleContentBuilder.ToString()!;
    }

    private IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly, IEnumerable<string>? classNameFilter)
        => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.FullName == typeof(ICodeGenerationProvider).FullName))
            .Where(t => FilterIsValid(t, classNameFilter))
            .Select(t => new CodeGenerationProviderWrapper(Activator.CreateInstance(t)!));

    private bool FilterIsValid(Type type, IEnumerable<string>? classNameFilter)
        => classNameFilter == null || !classNameFilter.Any() || classNameFilter.Any(x => x == type.FullName);
}
