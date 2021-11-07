namespace TextTemplateTransformationFramework.T4.Plus
{
    public static class Constants
    {
        public const string BasePathParameterName = "$T4Plus.BasePath";

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        internal const System.Reflection.BindingFlags BindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    }
}
