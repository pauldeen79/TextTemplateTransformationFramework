namespace TemplateFramework.Console;

[ExcludeFromCodeCoverage]
public class UserInput : IUserInput
{
    public string GetValue(TemplateParameter parameter)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        return parameter.Type.IsEnum
            ? Sharprompt.Prompt.Select(parameter.Name, ConvetToString(Enum.GetValues(parameter.Type)))
            : Sharprompt.Prompt.Input<string>(parameter.Name);
    }

    private static IEnumerable<string> ConvetToString(Array array)
    {
        foreach (var item in array)
        {
            yield return item.ToString()!;
        }
    }
}
