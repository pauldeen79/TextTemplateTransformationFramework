using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
#if NETCOREAPP3_1 || NET5_0_OR_GREATER
using TextTemplateTransformationFramework.T4.Plus.Core.Extensions;
#else
using TextTemplateTransformationFramework.T4.Plus.NetFramework.Extensions;
#endif
using Utilities.GridOutput;

namespace TextTemplateTransformationFramework.T4.Plus.GUI
{
    public partial class Form1 : Form
    {
        private ParametersViewModel _parametersViewModel;
        private readonly IScriptBuilder<TokenParserState> _scriptBuilder;
        private readonly ServiceProvider _provider;

        private const string FileName = "TextTemplateTransformationFramework.T4.Plus.GUI.tt";

        public Form1()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
#if NETCOREAPP3_1 || NET5_0_OR_GREATER
            serviceCollection.AddTextTemplateTransformationT4PlusNetCore();
#else
            serviceCollection.AddTextTemplateTransformationT4PlusNetFramework();
#endif
            _provider = serviceCollection.BuildServiceProvider();
            _scriptBuilder = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            var displayDelegate = new Func<ITemplateSectionProcessor<TokenParserState>, string>((p) => p.GetDirectiveName());
            directivesListBox.Items.AddRange
                (
                    _scriptBuilder
                        .GetKnownDirectives()
                        .OrderBy(displayDelegate)
                        .Select(d => new ItemWrapper<ITemplateSectionProcessor<TokenParserState>>(d, displayDelegate))
                        .ToArray()
                );
        }

        private void generateTemplateButton_Click(object sender, EventArgs e)
        {
            sourcecodeTextBox.SetText("");
            diagnosticDumpTextBox.SetText("");
            compilationErrorsTextBox.SetText("");
            exceptionTextBox.SetText("");
            templateOutputTextBox.SetText("");
            multipleOutputFileNameListView.Items.Clear();
            multipleOutputContentsTextBox.SetText("");
            templateParametersPropertyGrid.DoRefresh(); //force update of parameter values. (sometimes the event is processed after starting the generation process)
            TemplateParameter[] parameters;

            try
            {
                parameters = _parametersViewModel?.GetParameters().ToArray() ?? Array.Empty<TemplateParameter>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error getting parameters: " + ex);
                return;
            }

            templateTextBox.SetEnabled(false);
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var processor = _provider.GetRequiredService<ITextTemplateProcessor>();

                    var result = processor.Process(new TextTemplate(templateTextBox.Text, FileName), parameters);
                    sourcecodeTextBox.SetText(result.SourceCode);
                    diagnosticDumpTextBox.SetText(result.DiagnosticDump ?? string.Empty);
                    compilationErrorsTextBox.SetText(string.Join(Environment.NewLine, result.CompilerErrors.Select(c => c.ToString())));
                    exceptionTextBox.SetText(result.Exception ?? string.Empty);
                    templateOutputTextBox.SetText(result.Output);
                    gridListView.ClearItems();
                    gridListView.ClearColumns();
                    if (result.Output?.IndexOf("<MultipleContents xmlns:i=") > -1)
                    {
                        try
                        {
                            var multipleOutputBuilder = Runtime.MultipleContentBuilder.FromString(result.Output);
                            if (multipleOutputBuilder != null)
                            {
                                multipleOutputFileNameListView.Items.AddRange(multipleOutputBuilder.Contents.Select(c => new ListViewItem(c.FileName) { Tag = c.Builder.ToString() }).ToArray());
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionTextBox.SetText("Error getting multiple content output: " + ex);
                        }
                    }
                    else if (result.Output?.IndexOf('\t') > -1)
                    {
                        try
                        {
                            var gridOutput = GridOutput.FromString(result.Output);
                            foreach (var column in gridOutput.ColumnNames)
                            {
                                gridListView.AddColumn(column);
                            }
                            foreach (var item in gridOutput.Data)
                            {
                                if (item.Cells.Any())
                                {
                                    gridListView.AddWithSubItems(item.Cells.First().Value, item.Cells.Skip(1).Select(v => v.Value));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionTextBox.SetText("Error getting grid output: " + ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    exceptionTextBox.SetText("Error running template: " + ex);
                }
                finally
                {
                    multipleOutputFileNameListView.AutoResize();
                    gridListView.AutoResize();
                    templateTextBox.SetEnabled(true);
                }
            });
        }

        private void generateSourceCodeButton_Click(object sender, EventArgs e)
        {
            sourcecodeTextBox.SetText("");
            diagnosticDumpTextBox.SetText("");
            compilationErrorsTextBox.SetText("");
            exceptionTextBox.SetText("");
            templateTextBox.SetEnabled(false);
            TemplateParameter[] parameters;

            try
            {
                parameters = _parametersViewModel?.GetParameters().ToArray() ?? Array.Empty<TemplateParameter>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error getting parameters: " + ex);
                return;
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var processor = _provider.GetRequiredService<ITextTemplateProcessor>();

                    var result = processor.PreProcess(new TextTemplate(templateTextBox.Text, FileName), parameters);
                    sourcecodeTextBox.SetText(result.SourceCode ?? string.Empty);
                    diagnosticDumpTextBox.SetText(result.DiagnosticDump ?? string.Empty);
                    compilationErrorsTextBox.SetText(string.Join(Environment.NewLine, result.CompilerErrors.Select(c => c.ToString())));
                    exceptionTextBox.SetText(result.Exception ?? string.Empty);
                }
                catch (Exception ex)
                {
                    exceptionTextBox.SetText("Error generating source code: " + ex);
                }
                finally
                {
                    templateTextBox.SetEnabled(true);
                }
            });
        }

        private void fillTemplateParametersButton_Click(object sender, EventArgs e)
        {
            templateParametersPropertyGrid.SetSelectedObject(null);
            _parametersViewModel = null;
            sourcecodeTextBox.SetText("");
            diagnosticDumpTextBox.SetText("");
            compilationErrorsTextBox.SetText("");
            exceptionTextBox.SetText("");

            templateTextBox.SetEnabled(false);
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var processor = _provider.GetRequiredService<ITextTemplateProcessor>();

                    var result = processor.ExtractParameters(new TextTemplate(templateTextBox.Text, FileName));
                    sourcecodeTextBox.SetText(result.SourceCode ?? string.Empty);
                    diagnosticDumpTextBox.SetText(result.DiagnosticDump ?? string.Empty);
                    compilationErrorsTextBox.SetText(string.Join(Environment.NewLine, result.CompilerErrors.Select(c => c.ToString())));
                    exceptionTextBox.SetText(result.Exception ?? string.Empty);
                    _parametersViewModel = new ParametersViewModel(result.Parameters ?? Array.Empty<TemplateParameter>());
                    templateParametersPropertyGrid.SetSelectedObject(_parametersViewModel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error updating parameters: " + ex);
                }
                finally
                {
                    templateTextBox.SetEnabled(true);
                }
            });
        }

        private void saveTemplateButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Save template output";
                dialog.Filter = "All files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, templateOutputTextBox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format("Error saving template output to file {0}: {1}", dialog.FileName, ex));
                    }
                }
            }
        }

        private void saveSourceCodeButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Save source code";
                dialog.Filter = "C# files|*.cs|All files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, sourcecodeTextBox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format("Error saving source code to file {0}: {1}", dialog.FileName, ex));
                    }
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            templateParametersPropertyGrid.ResetSelectedProperty();
        }

        private void directivesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (directivesListBox.SelectedItem != null)
            {
                try
                {
                    var itemWrapper = (ItemWrapper<ITemplateSectionProcessor<TokenParserState>>)directivesListBox.SelectedItem;
                    var item = itemWrapper.Item;
                    builderPropertyGrid.SetSelectedObject
                        (
                            item.GetModel()
                        );
                    builderPropertyGrid.DoRefresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Error showing information about directive: {0}", ex));
                }
            }
        }

        private void generateScriptButton_Click(object sender, EventArgs e)
        {
            if (directivesListBox.SelectedItem != null && builderPropertyGrid.SelectedObject != null)
            {
                try
                {
                    var itemWrapper = (ItemWrapper<ITemplateSectionProcessor<TokenParserState>>)directivesListBox.SelectedItem;
                    var templateSectionProcessor = itemWrapper.Item;
                    templateTextBox.Append(_scriptBuilder.Build(templateSectionProcessor, builderPropertyGrid.SelectedObject));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Error generating script for directive: {0}", ex));
                }
            }
        }

        private void multipleOutputFileNameListView_DoubleClick(object sender, EventArgs e)
        {
            multipleOutputContentsTextBox.Clear();
            try
            {
                if (multipleOutputFileNameListView.SelectedItems?.Count > 0)
                {
                    var item = multipleOutputFileNameListView.SelectedItems[0];
                    var contents = item.Tag;
                    if (contents != null)
                    {
                        multipleOutputContentsTextBox.Text = contents.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Error opening contents of file: {0}", ex));
            }
        }

        private void generateDiagnosticDumpButton_Click(object sender, EventArgs e)
        {
            generateSourceCodeButton_Click(sender, e);
        }

        private void saveDiagnosticDumpButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Save diagnostic dump";
                dialog.Filter = "txt files|*.txt|All files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, diagnosticDumpTextBox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format("Error saving diagnostic dump to file {0}: {1}", dialog.FileName, ex));
                    }
                }
            }
        }
    }
}
