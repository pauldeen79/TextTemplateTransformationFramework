using System;

namespace TextTemplateTransformationFramework.Common
{
    public sealed class CompilerError
    {
        public CompilerError(int column, string errorNumber, string errorText, string fileName, bool isWarning, int line)
        {
            Column = column;
            ErrorNumber = errorNumber ?? throw new ArgumentNullException(nameof(errorNumber));
            ErrorText = errorText ?? throw new ArgumentNullException(nameof(errorText));
            FileName = fileName ?? "unknown.tt";
            IsWarning = isWarning;
            Line = line;
        }

        public int Column { get; }
        public string ErrorNumber { get; }
        public string ErrorText { get; }
        public string FileName { get; }
        public bool IsWarning { get; }
        public int Line { get; }

        public override string ToString() =>
            string.Format("{0}({1},{2}): {3}{4}: {5}"
                , FileName
                , Line
                , Column
                , IsWarning
                    ? "warning"
                    : "error"
                , string.IsNullOrEmpty(ErrorNumber)
                    ? string.Empty
                    : " " + ErrorNumber
                , ErrorText);
    }
}
