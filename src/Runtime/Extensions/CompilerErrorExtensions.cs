namespace TextTemplateTransformationFramework.Runtime.Extensions
{
    public static class CompilerErrorExtensions
    {
        public static string GetErrorType(this CompilerError instance) =>
            instance.IsWarning
                ? "Warning"
                : "Error";
    }
}
