C:
cd ..\..\T4.Plus.Cmd\bin\Debug\net5.0
dotnet TextTemplateTransformationFramework.T4.Plus.Cmd.dll source -f "C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework\T4.Plus\CodeGenerators\Templates\T4PlusCSharpCodeGenerator.template" -o "C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework\T4.Plus\CodeGenerators\T4PlusCSharpCodeGenerator.cs" -Parameters $T4Plus.BasePath:C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework\
pause
