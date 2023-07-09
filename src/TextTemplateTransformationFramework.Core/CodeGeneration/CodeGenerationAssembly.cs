namespace TemplateFramework.Core.CodeGeneration;

public sealed class CodeGenerationAssembly : IDisposable
{
    private readonly ICodeGenerationEngine _codeGenerationEngine;
    private readonly string _assemblyName;
    private readonly string _basePath;
    private readonly bool _generateMultipleFiles;
    private readonly bool _dryRun;
    private readonly IEnumerable<string> _classNameFilter;
    private readonly AssemblyLoadContext _context;
    private readonly string _currentDirectory;

    public CodeGenerationAssembly(ICodeGenerationEngine codeGenerationEngine,
                                  string assemblyName,
                                  string basePath,
                                  bool generateMultipleFiles,
                                  bool dryRun,
                                  string? currentDirectory = null,
                                  IEnumerable<string>? classNameFilter = null)
    {
        Guard.IsNotNull(codeGenerationEngine);
        Guard.IsNotNullOrWhiteSpace(assemblyName);
        Guard.IsNotNull(basePath);

        _codeGenerationEngine = codeGenerationEngine;
        _assemblyName = assemblyName;
        _basePath = basePath;
        _generateMultipleFiles = generateMultipleFiles;
        _dryRun = dryRun;
        _classNameFilter = classNameFilter ?? Enumerable.Empty<string>();

        if (string.IsNullOrEmpty(currentDirectory))
        {
            _currentDirectory = Directory.GetCurrentDirectory();
        }
        else
        {
            _currentDirectory = currentDirectory;
        }

        _context = new CustomAssemblyLoadContext("TemplateFramework.Core.CodeGeneration", true, () => new[] { _currentDirectory });
    }

    public string Generate()
    {
        var assembly = LoadAssembly(_context ?? AssemblyLoadContext.Default);
        var settings = new CodeGenerationSettings(_basePath, _generateMultipleFiles, _dryRun);

        return GetOutputFromAssembly(assembly, settings);
    }

    public void Dispose() => _context?.Unload();

    private Assembly LoadAssembly(AssemblyLoadContext context)
    {
        try
        {
            return context.LoadFromAssemblyName(new AssemblyName(_assemblyName));
        }
        catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.") || e.Message.EndsWith("The system cannot find the file specified."))
        {
            var assemblyName = _assemblyName;
            if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(assemblyName))
            {
                assemblyName = Path.Combine(_currentDirectory, Path.GetFileName(assemblyName));
            }
            return context.LoadFromAssemblyPath(assemblyName);
        }
    }

    private string GetOutputFromAssembly(Assembly assembly, CodeGenerationSettings settings)
    {
        var multipleContentBuilder = new MultipleContentBuilder { BasePath = settings.BasePath };
        var templateFileManager = new TemplateFileManager(multipleContentBuilder);

        foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly))
        {
            _codeGenerationEngine.Generate(codeGenerationProvider, templateFileManager, settings);
        }

        return multipleContentBuilder.ToString()!;
    }

    private IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly)
        => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.FullName == typeof(ICodeGenerationProvider).FullName))
            .Where(FilterIsValid)
            .Select(t => new CodeGenerationProviderWrapper(Activator.CreateInstance(t)!));

    private bool FilterIsValid(Type type)
        => !_classNameFilter.Any() || _classNameFilter.Any(x => x == type.FullName);
}
