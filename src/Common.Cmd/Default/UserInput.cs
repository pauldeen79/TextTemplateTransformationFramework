using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.Default
{
    public class UserInput : IUserInput
    {
        public string GetValue(TemplateParameter parameter)
            => parameter.Type.IsEnum
                ? Sharprompt.Prompt.Select(parameter.DisplayName.WhenNullOrEmpty(parameter.Name), parameter.PossibleValues)
                : Sharprompt.Prompt.Input<string>(parameter.DisplayName.WhenNullOrEmpty(parameter.Name));
    }
}
