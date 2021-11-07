namespace TextTemplateTransformationFramework.Runtime
{
    public class CompilerError
    {
        public CompilerError(int column, string errorNumber, string errorText, string fileName, bool isWarning, int line)
        {
            Column = column;
            ErrorNumber = errorNumber;
            ErrorText = errorText;
            FileName = fileName;
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
