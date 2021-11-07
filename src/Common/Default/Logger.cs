using System;
using System.Text;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    [Serializable]
    public class Logger : ILogger
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void Log(string message) => _stringBuilder.AppendLine(message);

        public string Aggregate() => _stringBuilder.ToString();
    }
}
