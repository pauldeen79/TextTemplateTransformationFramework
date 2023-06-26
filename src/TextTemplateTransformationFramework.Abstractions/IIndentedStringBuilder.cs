namespace TextTemplateTransformationFramework.Abstractions;

public interface IIndentedStringBuilder
{
    int IndentCount { get; }
    int Length { get; }

    IIndentedStringBuilder Append(string value);
    IIndentedStringBuilder Append(IIndentedStringBuilder builder);
    IIndentedStringBuilder Append(FormattableString value);
    IIndentedStringBuilder Append(char value);
    IIndentedStringBuilder Append(IEnumerable<string> value);
    IIndentedStringBuilder Append(IEnumerable<char> value);
    //IIndentedStringBuilder AppendJoin(IEnumerable<string> values, string separator = ", ");
    IIndentedStringBuilder AppendJoin(string separator, params string[] values);
    IIndentedStringBuilder AppendLine();
    IIndentedStringBuilder AppendLine(string value);
    IIndentedStringBuilder AppendLine(FormattableString value);
    IIndentedStringBuilder AppendLines(string value, bool skipFinalNewline = false);
    IIndentedStringBuilder Clear();
    IIndentedStringBuilder DecrementIndent();
    IIndentedStringBuilder IncrementIndent();
    IDisposable Indent();
    IDisposable SuspendIndent();
}
