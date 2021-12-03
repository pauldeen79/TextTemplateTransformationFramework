using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace TextTemplateTransformationFramework.Runtime
{
    public class T4GeneratedTemplateBase
    {
        #region Fields
        private StringBuilder generationEnvironmentField;
        private List<CompilerError> errorsField;
        private List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        #endregion

        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        public StringBuilder GenerationEnvironment
        {
            get
            {
                return generationEnvironmentField ?? (generationEnvironmentField = new StringBuilder());
            }
            set
            {
                generationEnvironmentField = value;
            }
        }

        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public List<CompilerError> Errors
        {
            get
            {
                return errorsField ?? (errorsField = new List<CompilerError>());
            }
        }

        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private List<int> IndentLengths
        {
            get
            {
                return indentLengthsField ?? (indentLengthsField = new List<int>());
            }
        }

        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return currentIndentField;
            }
        }

        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual IDictionary<string, object> Session { get; set; }
        #endregion

        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public virtual void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if ((GenerationEnvironment.Length == 0) || endsWithNewline)
            {
                GenerationEnvironment.Append(currentIndentField);
                endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture)
                || textToAppend.EndsWith("\n", StringComparison.CurrentCulture))
            {
                endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if (currentIndentField.Length == 0)
            {
                GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(Environment.NewLine, Environment.NewLine + currentIndentField);
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (endsWithNewline)
            {
                GenerationEnvironment.Append(textToAppend, 0, textToAppend.Length - currentIndentField.Length);
            }
            else
            {
                GenerationEnvironment.Append(textToAppend);
            }
        }

        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            Write(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            GenerationEnvironment.AppendLine();
            endsWithNewline = true;
        }

        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            CompilerError error = new CompilerError(1, "TemplateError", message, "T4GeneratedTemplateBase.cs", false, 1);
            Errors.Add(error);
        }

        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            CompilerError error = new CompilerError(1, "TemplateWarning", message, "T4GeneratedTemplateBase.cs", true, 1);
            Errors.Add(error);
        }

        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            currentIndentField += indent ?? throw new ArgumentNullException(nameof(indent));
            IndentLengths.Add(indent.Length);
        }

        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if (IndentLengths.Count > 0)
            {
                int indentLength = IndentLengths[IndentLengths.Count - 1];
                IndentLengths.RemoveAt(IndentLengths.Count - 1);
                if (indentLength > 0)
                {
                    returnValue = currentIndentField.Substring(currentIndentField.Length - indentLength);
                    currentIndentField = currentIndentField.Remove(currentIndentField.Length - indentLength);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            IndentLengths.Clear();
            currentIndentField = "";
        }
        #endregion

        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public class ToStringInstanceHelper
        {
            public bool AllowNullExpressions { get; set; }

            private IFormatProvider formatProviderField = new CultureInfo("en-US");

            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public IFormatProvider FormatProvider
            {
                get
                {
                    return formatProviderField;
                }
                set
                {
                    if (value != null)
                    {
                        formatProviderField = value;
                    }
                }
            }

            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if (objectToConvert == null)
                {
                    if (AllowNullExpressions)
                    {
                        return string.Empty;
                    }
                    throw new ArgumentNullException(nameof(objectToConvert));
                }
                Type t = objectToConvert.GetType();
                MethodInfo method = t.GetMethod("ToString", new Type[] { typeof(IFormatProvider) });
                if (method == null)
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return (string)(method.Invoke(objectToConvert, new object[] { formatProviderField }));
                }
            }
        }

        public ToStringInstanceHelper ToStringHelper { get; set; } = new ToStringInstanceHelper();
        #endregion
    }
}
