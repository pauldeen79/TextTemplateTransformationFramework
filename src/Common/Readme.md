# TextTemplateTransformationFramework
T4 syntax-compatible framework, more extensible and no dependency to Visual Studio. Works on both .NET Framework and .NET Core.

This is the flow of the TemplateProcessor (main entry point):
* The host / initiator of the TemplateProcessor is responsible for sending parameter values.
You can call the ExtractParameterValues method to get the available parameters, and then fill the values.
* The host / initiator of the TemplateProcessor calls the Process method to run the entire process, and get the output from the generated template.
* The host / initiator of the TemplateProcessor can also call the PreProcess method to get 'run-time text templates'.
(source code for the template, so it can be re-used, and run seperately)

This is the internal flow of the Process method of the TemplateProcessor:
* Parse text template into tokens (ITextTemplateTokenParser)
* Convert tokens into C# code (ITokenProcessor)
* Compile the generated C# code (ITemplateCodeCompiler)
* Render the generated tempalate (ITemplateRenderer)
* Return the result of this process (generated source code, compiler errors, generated template output, diagnostics dump)

This is the project structure:
* Common: Common infrastructure and interfaces
* Common.Cmd: Common instrastructure for command-line (console) hosts (.NET Framework and .NET Core)
* Runtime: Helpers for working with generated templates from outside the TemplateProcessor, and base class for generated templates (to reduce duplicate code)
* T4: Implementation of parsing text files with T4 syntax (backwards compatbible with Microsoft's T4 implementation)
* T4.Plus: Additional functionality to T4 template language (directives and sections) and the T4 generation process (for example, interceptors)
