using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILogger Create() => new Logger();
    }
}
