C:
cd ..\..\T4.Plus.Cmd\bin\Debug\net5.0
dotnet TextTemplateTransformationFramework.T4.Plus.Cmd.dll source -f "C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework\T4\CodeGenerators\Templates\T4CSharpCodeGenerator.template" -o "C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework\T4\CodeGenerators\T4CSharpCodeGenerator.cs" -Parameters $T4Plus.BasePath:C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework\
pause
