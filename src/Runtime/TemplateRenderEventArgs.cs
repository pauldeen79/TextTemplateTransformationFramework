using System;
using System.Text;

namespace TextTemplateTransformationFramework.Runtime
{
    public class TemplateRenderEventArgs : EventArgs
    {
        public TemplateWrapper TemplateMockWrapper { get; }
        public StringBuilder Builder { get; }

        public TemplateRenderEventArgs(TemplateWrapper templateMockWrapper, StringBuilder builder)
        {
            TemplateMockWrapper = templateMockWrapper;
            Builder = builder;
        }
    }
}