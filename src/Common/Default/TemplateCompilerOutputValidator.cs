using System;
using System.Collections;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateCompilerOutputValidator<TState> : ITemplateCompilerOutputValidator<TState>
        where TState : class
    {
        public bool Validate(ITemplateProcessorContext<TState> context, out IEnumerable<CompilerError> compilerErrorCollection)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            compilerErrorCollection = context.TemplateCompilerOutput.Errors;

            if (context.TemplateCompilerOutput.Template == null)
            {
                //compilation failed, or typename of template was incorrect
                return false;
            }

            if (compilerErrorCollection.HasErrors())
            {
                return false;
            }

            var errorsProperty2 = context.TemplateCompilerOutput.Template.GetType().GetProperty("Errors");
            var errorsValue = errorsProperty2?.GetValue(context.TemplateCompilerOutput.Template, null);
            if (errorsValue is IEnumerable<CompilerError> errors)
            {
                compilerErrorCollection = errors;
                if (compilerErrorCollection.HasErrors())
                {
                    return false;
                }
            }
            else if (errorsValue is IEnumerable enumerable)
            {
                compilerErrorCollection = GetErrors(enumerable);
                if (compilerErrorCollection.HasErrors())
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<CompilerError> GetErrors(IEnumerable enumerable)
        {
            foreach (object item in enumerable)
            {
                var t = item.GetType();
                yield return new CompilerError
                (
                    Convert.ToInt32(t.GetProperty("Column").GetValue(item, null)),
                    Convert.ToString(t.GetProperty("ErrorNumber").GetValue(item, null)),
                    Convert.ToString(t.GetProperty("ErrorText").GetValue(item, null)),
                    Convert.ToString(t.GetProperty("FileName").GetValue(item, null)),
                    Convert.ToBoolean(t.GetProperty("IsWarning").GetValue(item, null)),
                    Convert.ToInt32(t.GetProperty("Line").GetValue(item, null))
                );
            }
        }
    }
}
