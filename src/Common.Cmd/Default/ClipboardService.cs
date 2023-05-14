using TextTemplateTransformationFramework.Common.Cmd.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.Default
{
    public class ClipboardService : IClipboardService
    {
        public void SetValue(string value) => TextCopy.ClipboardService.SetText(value);
    }
}
