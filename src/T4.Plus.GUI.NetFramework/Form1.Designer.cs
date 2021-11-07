namespace TextTemplateTransformationFramework.T4.Plus.GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.templateOutputTabPage = new System.Windows.Forms.TabPage();
            this.templateOutputTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.templateOutputTextBox = new System.Windows.Forms.TextBox();
            this.generateTemplateButton = new System.Windows.Forms.Button();
            this.saveTemplateButton = new System.Windows.Forms.Button();
            this.sourceCodeTabPage = new System.Windows.Forms.TabPage();
            this.sourceCodeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.sourcecodeTextBox = new System.Windows.Forms.TextBox();
            this.saveSourceCodeButton = new System.Windows.Forms.Button();
            this.generateSourceCodeButton = new System.Windows.Forms.Button();
            this.compilationErrorsTabPage = new System.Windows.Forms.TabPage();
            this.compilationErrorsTextBox = new System.Windows.Forms.TextBox();
            this.exceptionTabPage = new System.Windows.Forms.TabPage();
            this.exceptionTextBox = new System.Windows.Forms.TextBox();
            this.templateParametersTabPage = new System.Windows.Forms.TabPage();
            this.templateParametersTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.resetButton = new System.Windows.Forms.Button();
            this.fillTemplateParametersButton = new System.Windows.Forms.Button();
            this.templateParametersPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.diagnosticDumpTabPage = new System.Windows.Forms.TabPage();
            this.diagnosticDumpTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.diagnosticDumpTextBox = new System.Windows.Forms.TextBox();
            this.saveDiagnosticDumpButton = new System.Windows.Forms.Button();
            this.generateDiagnosticDumpButton = new System.Windows.Forms.Button();
            this.builderTabPage = new System.Windows.Forms.TabPage();
            this.builderTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.builderPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.generateScriptButton = new System.Windows.Forms.Button();
            this.directivesListBox = new System.Windows.Forms.ListBox();
            this.multipleOutputTabPage = new System.Windows.Forms.TabPage();
            this.multipeOutputTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.multipleOutputContentsTextBox = new System.Windows.Forms.TextBox();
            this.multipleOutputFileNameListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gridOutputTabPage = new System.Windows.Forms.TabPage();
            this.gridListView = new System.Windows.Forms.ListView();
            this.templateTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1.SuspendLayout();
            this.templateOutputTabPage.SuspendLayout();
            this.templateOutputTableLayoutPanel.SuspendLayout();
            this.sourceCodeTabPage.SuspendLayout();
            this.sourceCodeTableLayoutPanel.SuspendLayout();
            this.compilationErrorsTabPage.SuspendLayout();
            this.exceptionTabPage.SuspendLayout();
            this.templateParametersTabPage.SuspendLayout();
            this.templateParametersTableLayoutPanel.SuspendLayout();
            this.diagnosticDumpTabPage.SuspendLayout();
            this.diagnosticDumpTableLayoutPanel.SuspendLayout();
            this.builderTabPage.SuspendLayout();
            this.builderTableLayoutPanel.SuspendLayout();
            this.multipleOutputTabPage.SuspendLayout();
            this.multipeOutputTableLayoutPanel.SuspendLayout();
            this.gridOutputTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.templateOutputTabPage);
            this.tabControl1.Controls.Add(this.sourceCodeTabPage);
            this.tabControl1.Controls.Add(this.compilationErrorsTabPage);
            this.tabControl1.Controls.Add(this.exceptionTabPage);
            this.tabControl1.Controls.Add(this.templateParametersTabPage);
            this.tabControl1.Controls.Add(this.diagnosticDumpTabPage);
            this.tabControl1.Controls.Add(this.builderTabPage);
            this.tabControl1.Controls.Add(this.multipleOutputTabPage);
            this.tabControl1.Controls.Add(this.gridOutputTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(940, 176);
            this.tabControl1.TabIndex = 0;
            // 
            // templateOutputTabPage
            // 
            this.templateOutputTabPage.Controls.Add(this.templateOutputTableLayoutPanel);
            this.templateOutputTabPage.Location = new System.Drawing.Point(4, 25);
            this.templateOutputTabPage.Name = "templateOutputTabPage";
            this.templateOutputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.templateOutputTabPage.Size = new System.Drawing.Size(932, 147);
            this.templateOutputTabPage.TabIndex = 0;
            this.templateOutputTabPage.Text = "Template output";
            this.templateOutputTabPage.UseVisualStyleBackColor = true;
            // 
            // templateOutputTableLayoutPanel
            // 
            this.templateOutputTableLayoutPanel.ColumnCount = 2;
            this.templateOutputTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.templateOutputTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.templateOutputTableLayoutPanel.Controls.Add(this.templateOutputTextBox, 1, 0);
            this.templateOutputTableLayoutPanel.Controls.Add(this.generateTemplateButton, 0, 0);
            this.templateOutputTableLayoutPanel.Controls.Add(this.saveTemplateButton, 0, 1);
            this.templateOutputTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateOutputTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.templateOutputTableLayoutPanel.Name = "templateOutputTableLayoutPanel";
            this.templateOutputTableLayoutPanel.RowCount = 2;
            this.templateOutputTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.templateOutputTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.templateOutputTableLayoutPanel.Size = new System.Drawing.Size(926, 141);
            this.templateOutputTableLayoutPanel.TabIndex = 0;
            // 
            // templateOutputTextBox
            // 
            this.templateOutputTextBox.AcceptsReturn = true;
            this.templateOutputTextBox.AcceptsTab = true;
            this.templateOutputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateOutputTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateOutputTextBox.Location = new System.Drawing.Point(103, 3);
            this.templateOutputTextBox.Multiline = true;
            this.templateOutputTextBox.Name = "templateOutputTextBox";
            this.templateOutputTextBox.ReadOnly = true;
            this.templateOutputTableLayoutPanel.SetRowSpan(this.templateOutputTextBox, 2);
            this.templateOutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.templateOutputTextBox.Size = new System.Drawing.Size(820, 135);
            this.templateOutputTextBox.TabIndex = 2;
            this.templateOutputTextBox.WordWrap = false;
            // 
            // generateTemplateButton
            // 
            this.generateTemplateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generateTemplateButton.Location = new System.Drawing.Point(3, 3);
            this.generateTemplateButton.Name = "generateTemplateButton";
            this.generateTemplateButton.Size = new System.Drawing.Size(94, 64);
            this.generateTemplateButton.TabIndex = 0;
            this.generateTemplateButton.Text = "Generate";
            this.generateTemplateButton.UseVisualStyleBackColor = true;
            this.generateTemplateButton.Click += new System.EventHandler(this.generateTemplateButton_Click);
            // 
            // saveTemplateButton
            // 
            this.saveTemplateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveTemplateButton.Location = new System.Drawing.Point(3, 73);
            this.saveTemplateButton.Name = "saveTemplateButton";
            this.saveTemplateButton.Size = new System.Drawing.Size(94, 65);
            this.saveTemplateButton.TabIndex = 1;
            this.saveTemplateButton.Text = "Save";
            this.saveTemplateButton.UseVisualStyleBackColor = true;
            this.saveTemplateButton.Click += new System.EventHandler(this.saveTemplateButton_Click);
            // 
            // sourceCodeTabPage
            // 
            this.sourceCodeTabPage.Controls.Add(this.sourceCodeTableLayoutPanel);
            this.sourceCodeTabPage.Location = new System.Drawing.Point(4, 25);
            this.sourceCodeTabPage.Name = "sourceCodeTabPage";
            this.sourceCodeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sourceCodeTabPage.Size = new System.Drawing.Size(932, 147);
            this.sourceCodeTabPage.TabIndex = 1;
            this.sourceCodeTabPage.Text = "Source code";
            this.sourceCodeTabPage.UseVisualStyleBackColor = true;
            // 
            // sourceCodeTableLayoutPanel
            // 
            this.sourceCodeTableLayoutPanel.ColumnCount = 2;
            this.sourceCodeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.sourceCodeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sourceCodeTableLayoutPanel.Controls.Add(this.sourcecodeTextBox, 1, 0);
            this.sourceCodeTableLayoutPanel.Controls.Add(this.saveSourceCodeButton, 0, 1);
            this.sourceCodeTableLayoutPanel.Controls.Add(this.generateSourceCodeButton, 0, 0);
            this.sourceCodeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.sourceCodeTableLayoutPanel.Name = "sourceCodeTableLayoutPanel";
            this.sourceCodeTableLayoutPanel.RowCount = 2;
            this.sourceCodeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.sourceCodeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.sourceCodeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.sourceCodeTableLayoutPanel.Size = new System.Drawing.Size(926, 141);
            this.sourceCodeTableLayoutPanel.TabIndex = 0;
            // 
            // sourcecodeTextBox
            // 
            this.sourcecodeTextBox.AcceptsReturn = true;
            this.sourcecodeTextBox.AcceptsTab = true;
            this.sourcecodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourcecodeTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourcecodeTextBox.Location = new System.Drawing.Point(103, 3);
            this.sourcecodeTextBox.Multiline = true;
            this.sourcecodeTextBox.Name = "sourcecodeTextBox";
            this.sourcecodeTextBox.ReadOnly = true;
            this.sourceCodeTableLayoutPanel.SetRowSpan(this.sourcecodeTextBox, 2);
            this.sourcecodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sourcecodeTextBox.Size = new System.Drawing.Size(820, 135);
            this.sourcecodeTextBox.TabIndex = 3;
            this.sourcecodeTextBox.WordWrap = false;
            // 
            // saveSourceCodeButton
            // 
            this.saveSourceCodeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveSourceCodeButton.Location = new System.Drawing.Point(3, 73);
            this.saveSourceCodeButton.Name = "saveSourceCodeButton";
            this.saveSourceCodeButton.Size = new System.Drawing.Size(94, 65);
            this.saveSourceCodeButton.TabIndex = 2;
            this.saveSourceCodeButton.Text = "Save";
            this.saveSourceCodeButton.UseVisualStyleBackColor = true;
            this.saveSourceCodeButton.Click += new System.EventHandler(this.saveSourceCodeButton_Click);
            // 
            // generateSourceCodeButton
            // 
            this.generateSourceCodeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generateSourceCodeButton.Location = new System.Drawing.Point(3, 3);
            this.generateSourceCodeButton.Name = "generateSourceCodeButton";
            this.generateSourceCodeButton.Size = new System.Drawing.Size(94, 64);
            this.generateSourceCodeButton.TabIndex = 1;
            this.generateSourceCodeButton.Text = "Generate";
            this.generateSourceCodeButton.UseVisualStyleBackColor = true;
            this.generateSourceCodeButton.Click += new System.EventHandler(this.generateSourceCodeButton_Click);
            // 
            // compilationErrorsTabPage
            // 
            this.compilationErrorsTabPage.Controls.Add(this.compilationErrorsTextBox);
            this.compilationErrorsTabPage.Location = new System.Drawing.Point(4, 25);
            this.compilationErrorsTabPage.Name = "compilationErrorsTabPage";
            this.compilationErrorsTabPage.Size = new System.Drawing.Size(932, 147);
            this.compilationErrorsTabPage.TabIndex = 2;
            this.compilationErrorsTabPage.Text = "Compilation errors";
            this.compilationErrorsTabPage.UseVisualStyleBackColor = true;
            // 
            // compilationErrorsTextBox
            // 
            this.compilationErrorsTextBox.AcceptsReturn = true;
            this.compilationErrorsTextBox.AcceptsTab = true;
            this.compilationErrorsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compilationErrorsTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.compilationErrorsTextBox.Location = new System.Drawing.Point(0, 0);
            this.compilationErrorsTextBox.Multiline = true;
            this.compilationErrorsTextBox.Name = "compilationErrorsTextBox";
            this.compilationErrorsTextBox.ReadOnly = true;
            this.compilationErrorsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.compilationErrorsTextBox.Size = new System.Drawing.Size(932, 147);
            this.compilationErrorsTextBox.TabIndex = 4;
            this.compilationErrorsTextBox.WordWrap = false;
            // 
            // exceptionTabPage
            // 
            this.exceptionTabPage.Controls.Add(this.exceptionTextBox);
            this.exceptionTabPage.Location = new System.Drawing.Point(4, 25);
            this.exceptionTabPage.Name = "exceptionTabPage";
            this.exceptionTabPage.Size = new System.Drawing.Size(932, 147);
            this.exceptionTabPage.TabIndex = 3;
            this.exceptionTabPage.Text = "Exception";
            this.exceptionTabPage.UseVisualStyleBackColor = true;
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.AcceptsReturn = true;
            this.exceptionTextBox.AcceptsTab = true;
            this.exceptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exceptionTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exceptionTextBox.Location = new System.Drawing.Point(0, 0);
            this.exceptionTextBox.Multiline = true;
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.ReadOnly = true;
            this.exceptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.exceptionTextBox.Size = new System.Drawing.Size(932, 147);
            this.exceptionTextBox.TabIndex = 4;
            this.exceptionTextBox.WordWrap = false;
            // 
            // templateParametersTabPage
            // 
            this.templateParametersTabPage.Controls.Add(this.templateParametersTableLayoutPanel);
            this.templateParametersTabPage.Location = new System.Drawing.Point(4, 25);
            this.templateParametersTabPage.Name = "templateParametersTabPage";
            this.templateParametersTabPage.Size = new System.Drawing.Size(932, 147);
            this.templateParametersTabPage.TabIndex = 4;
            this.templateParametersTabPage.Text = "Template parameters";
            this.templateParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // templateParametersTableLayoutPanel
            // 
            this.templateParametersTableLayoutPanel.ColumnCount = 2;
            this.templateParametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.templateParametersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.templateParametersTableLayoutPanel.Controls.Add(this.resetButton, 0, 1);
            this.templateParametersTableLayoutPanel.Controls.Add(this.fillTemplateParametersButton, 0, 0);
            this.templateParametersTableLayoutPanel.Controls.Add(this.templateParametersPropertyGrid, 1, 0);
            this.templateParametersTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateParametersTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.templateParametersTableLayoutPanel.Name = "templateParametersTableLayoutPanel";
            this.templateParametersTableLayoutPanel.RowCount = 2;
            this.templateParametersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.templateParametersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.templateParametersTableLayoutPanel.Size = new System.Drawing.Size(932, 147);
            this.templateParametersTableLayoutPanel.TabIndex = 0;
            // 
            // resetButton
            // 
            this.resetButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resetButton.Location = new System.Drawing.Point(3, 76);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(94, 68);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // fillTemplateParametersButton
            // 
            this.fillTemplateParametersButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillTemplateParametersButton.Location = new System.Drawing.Point(3, 3);
            this.fillTemplateParametersButton.Name = "fillTemplateParametersButton";
            this.fillTemplateParametersButton.Size = new System.Drawing.Size(94, 67);
            this.fillTemplateParametersButton.TabIndex = 0;
            this.fillTemplateParametersButton.Text = "Fill";
            this.fillTemplateParametersButton.UseVisualStyleBackColor = true;
            this.fillTemplateParametersButton.Click += new System.EventHandler(this.fillTemplateParametersButton_Click);
            // 
            // templateParametersPropertyGrid
            // 
            this.templateParametersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateParametersPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.templateParametersPropertyGrid.Location = new System.Drawing.Point(103, 3);
            this.templateParametersPropertyGrid.Name = "templateParametersPropertyGrid";
            this.templateParametersPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.templateParametersTableLayoutPanel.SetRowSpan(this.templateParametersPropertyGrid, 2);
            this.templateParametersPropertyGrid.Size = new System.Drawing.Size(826, 141);
            this.templateParametersPropertyGrid.TabIndex = 2;
            this.templateParametersPropertyGrid.ToolbarVisible = false;
            // 
            // diagnosticDumpTabPage
            // 
            this.diagnosticDumpTabPage.Controls.Add(this.diagnosticDumpTableLayoutPanel);
            this.diagnosticDumpTabPage.Location = new System.Drawing.Point(4, 25);
            this.diagnosticDumpTabPage.Name = "diagnosticDumpTabPage";
            this.diagnosticDumpTabPage.Size = new System.Drawing.Size(932, 147);
            this.diagnosticDumpTabPage.TabIndex = 9;
            this.diagnosticDumpTabPage.Text = "Diagnostic dump";
            this.diagnosticDumpTabPage.UseVisualStyleBackColor = true;
            // 
            // diagnosticDumpTableLayoutPanel
            // 
            this.diagnosticDumpTableLayoutPanel.ColumnCount = 2;
            this.diagnosticDumpTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.diagnosticDumpTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.diagnosticDumpTableLayoutPanel.Controls.Add(this.diagnosticDumpTextBox, 1, 0);
            this.diagnosticDumpTableLayoutPanel.Controls.Add(this.saveDiagnosticDumpButton, 0, 1);
            this.diagnosticDumpTableLayoutPanel.Controls.Add(this.generateDiagnosticDumpButton, 0, 0);
            this.diagnosticDumpTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticDumpTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.diagnosticDumpTableLayoutPanel.Name = "diagnosticDumpTableLayoutPanel";
            this.diagnosticDumpTableLayoutPanel.RowCount = 2;
            this.diagnosticDumpTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.diagnosticDumpTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.diagnosticDumpTableLayoutPanel.Size = new System.Drawing.Size(932, 147);
            this.diagnosticDumpTableLayoutPanel.TabIndex = 0;
            // 
            // diagnosticDumpTextBox
            // 
            this.diagnosticDumpTextBox.AcceptsReturn = true;
            this.diagnosticDumpTextBox.AcceptsTab = true;
            this.diagnosticDumpTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticDumpTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diagnosticDumpTextBox.Location = new System.Drawing.Point(103, 3);
            this.diagnosticDumpTextBox.Multiline = true;
            this.diagnosticDumpTextBox.Name = "diagnosticDumpTextBox";
            this.diagnosticDumpTextBox.ReadOnly = true;
            this.diagnosticDumpTableLayoutPanel.SetRowSpan(this.diagnosticDumpTextBox, 2);
            this.diagnosticDumpTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.diagnosticDumpTextBox.Size = new System.Drawing.Size(826, 141);
            this.diagnosticDumpTextBox.TabIndex = 4;
            this.diagnosticDumpTextBox.WordWrap = false;
            // 
            // saveDiagnosticDumpButton
            // 
            this.saveDiagnosticDumpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveDiagnosticDumpButton.Location = new System.Drawing.Point(3, 76);
            this.saveDiagnosticDumpButton.Name = "saveDiagnosticDumpButton";
            this.saveDiagnosticDumpButton.Size = new System.Drawing.Size(94, 68);
            this.saveDiagnosticDumpButton.TabIndex = 3;
            this.saveDiagnosticDumpButton.Text = "Save";
            this.saveDiagnosticDumpButton.UseVisualStyleBackColor = true;
            this.saveDiagnosticDumpButton.Click += new System.EventHandler(this.saveDiagnosticDumpButton_Click);
            // 
            // generateDiagnosticDumpButton
            // 
            this.generateDiagnosticDumpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generateDiagnosticDumpButton.Location = new System.Drawing.Point(3, 3);
            this.generateDiagnosticDumpButton.Name = "generateDiagnosticDumpButton";
            this.generateDiagnosticDumpButton.Size = new System.Drawing.Size(94, 67);
            this.generateDiagnosticDumpButton.TabIndex = 2;
            this.generateDiagnosticDumpButton.Text = "Generate";
            this.generateDiagnosticDumpButton.UseVisualStyleBackColor = true;
            this.generateDiagnosticDumpButton.Click += new System.EventHandler(this.generateDiagnosticDumpButton_Click);
            // 
            // builderTabPage
            // 
            this.builderTabPage.Controls.Add(this.builderTableLayoutPanel);
            this.builderTabPage.Location = new System.Drawing.Point(4, 25);
            this.builderTabPage.Name = "builderTabPage";
            this.builderTabPage.Size = new System.Drawing.Size(932, 147);
            this.builderTabPage.TabIndex = 6;
            this.builderTabPage.Text = "Builder";
            this.builderTabPage.UseVisualStyleBackColor = true;
            // 
            // builderTableLayoutPanel
            // 
            this.builderTableLayoutPanel.ColumnCount = 2;
            this.builderTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.builderTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.builderTableLayoutPanel.Controls.Add(this.builderPropertyGrid, 1, 0);
            this.builderTableLayoutPanel.Controls.Add(this.generateScriptButton, 0, 0);
            this.builderTableLayoutPanel.Controls.Add(this.directivesListBox, 0, 1);
            this.builderTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.builderTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.builderTableLayoutPanel.Name = "builderTableLayoutPanel";
            this.builderTableLayoutPanel.RowCount = 2;
            this.builderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.builderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.builderTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.builderTableLayoutPanel.Size = new System.Drawing.Size(932, 147);
            this.builderTableLayoutPanel.TabIndex = 0;
            // 
            // builderPropertyGrid
            // 
            this.builderPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.builderPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.builderPropertyGrid.Location = new System.Drawing.Point(253, 3);
            this.builderPropertyGrid.Name = "builderPropertyGrid";
            this.builderPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.builderTableLayoutPanel.SetRowSpan(this.builderPropertyGrid, 2);
            this.builderPropertyGrid.Size = new System.Drawing.Size(676, 141);
            this.builderPropertyGrid.TabIndex = 1;
            this.builderPropertyGrid.ToolbarVisible = false;
            // 
            // generateScriptButton
            // 
            this.generateScriptButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generateScriptButton.Location = new System.Drawing.Point(3, 3);
            this.generateScriptButton.Name = "generateScriptButton";
            this.generateScriptButton.Size = new System.Drawing.Size(244, 24);
            this.generateScriptButton.TabIndex = 2;
            this.generateScriptButton.Text = "Generate";
            this.generateScriptButton.UseVisualStyleBackColor = true;
            this.generateScriptButton.Click += new System.EventHandler(this.generateScriptButton_Click);
            // 
            // directivesListBox
            // 
            this.directivesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directivesListBox.FormattingEnabled = true;
            this.directivesListBox.Location = new System.Drawing.Point(3, 33);
            this.directivesListBox.Name = "directivesListBox";
            this.directivesListBox.Size = new System.Drawing.Size(244, 111);
            this.directivesListBox.TabIndex = 3;
            this.directivesListBox.SelectedIndexChanged += new System.EventHandler(this.directivesListBox_SelectedIndexChanged);
            // 
            // multipleOutputTabPage
            // 
            this.multipleOutputTabPage.Controls.Add(this.multipeOutputTableLayoutPanel);
            this.multipleOutputTabPage.Location = new System.Drawing.Point(4, 25);
            this.multipleOutputTabPage.Name = "multipleOutputTabPage";
            this.multipleOutputTabPage.Size = new System.Drawing.Size(932, 147);
            this.multipleOutputTabPage.TabIndex = 7;
            this.multipleOutputTabPage.Text = "Multiple output";
            this.multipleOutputTabPage.UseVisualStyleBackColor = true;
            // 
            // multipeOutputTableLayoutPanel
            // 
            this.multipeOutputTableLayoutPanel.ColumnCount = 2;
            this.multipeOutputTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.42489F));
            this.multipeOutputTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.57511F));
            this.multipeOutputTableLayoutPanel.Controls.Add(this.multipleOutputContentsTextBox, 1, 0);
            this.multipeOutputTableLayoutPanel.Controls.Add(this.multipleOutputFileNameListView, 0, 0);
            this.multipeOutputTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multipeOutputTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.multipeOutputTableLayoutPanel.Name = "multipeOutputTableLayoutPanel";
            this.multipeOutputTableLayoutPanel.RowCount = 1;
            this.multipeOutputTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.multipeOutputTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.multipeOutputTableLayoutPanel.Size = new System.Drawing.Size(932, 147);
            this.multipeOutputTableLayoutPanel.TabIndex = 0;
            // 
            // multipleOutputContentsTextBox
            // 
            this.multipleOutputContentsTextBox.AcceptsReturn = true;
            this.multipleOutputContentsTextBox.AcceptsTab = true;
            this.multipleOutputContentsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multipleOutputContentsTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multipleOutputContentsTextBox.Location = new System.Drawing.Point(211, 3);
            this.multipleOutputContentsTextBox.Multiline = true;
            this.multipleOutputContentsTextBox.Name = "multipleOutputContentsTextBox";
            this.multipleOutputContentsTextBox.ReadOnly = true;
            this.multipleOutputContentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.multipleOutputContentsTextBox.Size = new System.Drawing.Size(718, 141);
            this.multipleOutputContentsTextBox.TabIndex = 3;
            this.multipleOutputContentsTextBox.WordWrap = false;
            // 
            // multipleOutputFileNameListView
            // 
            this.multipleOutputFileNameListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.multipleOutputFileNameListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multipleOutputFileNameListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.multipleOutputFileNameListView.HideSelection = false;
            this.multipleOutputFileNameListView.Location = new System.Drawing.Point(3, 3);
            this.multipleOutputFileNameListView.Name = "multipleOutputFileNameListView";
            this.multipleOutputFileNameListView.Size = new System.Drawing.Size(202, 141);
            this.multipleOutputFileNameListView.TabIndex = 0;
            this.multipleOutputFileNameListView.UseCompatibleStateImageBehavior = false;
            this.multipleOutputFileNameListView.View = System.Windows.Forms.View.Details;
            this.multipleOutputFileNameListView.DoubleClick += new System.EventHandler(this.multipleOutputFileNameListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            // 
            // gridOutputTabPage
            // 
            this.gridOutputTabPage.Controls.Add(this.gridListView);
            this.gridOutputTabPage.Location = new System.Drawing.Point(4, 25);
            this.gridOutputTabPage.Name = "gridOutputTabPage";
            this.gridOutputTabPage.Size = new System.Drawing.Size(932, 147);
            this.gridOutputTabPage.TabIndex = 8;
            this.gridOutputTabPage.Text = "Grid output";
            this.gridOutputTabPage.UseVisualStyleBackColor = true;
            // 
            // gridListView
            // 
            this.gridListView.AllowColumnReorder = true;
            this.gridListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridListView.FullRowSelect = true;
            this.gridListView.GridLines = true;
            this.gridListView.Location = new System.Drawing.Point(0, 0);
            this.gridListView.Name = "gridListView";
            this.gridListView.Size = new System.Drawing.Size(932, 147);
            this.gridListView.TabIndex = 0;
            this.gridListView.UseCompatibleStateImageBehavior = false;
            this.gridListView.View = System.Windows.Forms.View.Details;
            // 
            // templateTextBox
            // 
            this.templateTextBox.AcceptsReturn = true;
            this.templateTextBox.AcceptsTab = true;
            this.templateTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateTextBox.Font = new System.Drawing.Font("Courier New", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateTextBox.Location = new System.Drawing.Point(0, 0);
            this.templateTextBox.Multiline = true;
            this.templateTextBox.Name = "templateTextBox";
            this.templateTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.templateTextBox.Size = new System.Drawing.Size(940, 291);
            this.templateTextBox.TabIndex = 1;
            this.templateTextBox.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.templateTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(940, 471);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 471);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "TextTemplateTransformationFramework";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.templateOutputTabPage.ResumeLayout(false);
            this.templateOutputTableLayoutPanel.ResumeLayout(false);
            this.templateOutputTableLayoutPanel.PerformLayout();
            this.sourceCodeTabPage.ResumeLayout(false);
            this.sourceCodeTableLayoutPanel.ResumeLayout(false);
            this.sourceCodeTableLayoutPanel.PerformLayout();
            this.compilationErrorsTabPage.ResumeLayout(false);
            this.compilationErrorsTabPage.PerformLayout();
            this.exceptionTabPage.ResumeLayout(false);
            this.exceptionTabPage.PerformLayout();
            this.templateParametersTabPage.ResumeLayout(false);
            this.templateParametersTableLayoutPanel.ResumeLayout(false);
            this.diagnosticDumpTabPage.ResumeLayout(false);
            this.diagnosticDumpTableLayoutPanel.ResumeLayout(false);
            this.diagnosticDumpTableLayoutPanel.PerformLayout();
            this.builderTabPage.ResumeLayout(false);
            this.builderTableLayoutPanel.ResumeLayout(false);
            this.multipleOutputTabPage.ResumeLayout(false);
            this.multipeOutputTableLayoutPanel.ResumeLayout(false);
            this.multipeOutputTableLayoutPanel.PerformLayout();
            this.gridOutputTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage templateOutputTabPage;
        private System.Windows.Forms.TableLayoutPanel templateOutputTableLayoutPanel;
        private System.Windows.Forms.TextBox templateOutputTextBox;
        private System.Windows.Forms.Button generateTemplateButton;
        private System.Windows.Forms.Button saveTemplateButton;
        private System.Windows.Forms.TabPage sourceCodeTabPage;
        private System.Windows.Forms.TableLayoutPanel sourceCodeTableLayoutPanel;
        private System.Windows.Forms.TextBox sourcecodeTextBox;
        private System.Windows.Forms.Button saveSourceCodeButton;
        private System.Windows.Forms.Button generateSourceCodeButton;
        private System.Windows.Forms.TabPage compilationErrorsTabPage;
        private System.Windows.Forms.TextBox compilationErrorsTextBox;
        private System.Windows.Forms.TabPage exceptionTabPage;
        private System.Windows.Forms.TextBox exceptionTextBox;
        private System.Windows.Forms.TabPage templateParametersTabPage;
        private System.Windows.Forms.TableLayoutPanel templateParametersTableLayoutPanel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button fillTemplateParametersButton;
        private System.Windows.Forms.PropertyGrid templateParametersPropertyGrid;
        private System.Windows.Forms.TabPage builderTabPage;
        private System.Windows.Forms.TableLayoutPanel builderTableLayoutPanel;
        private System.Windows.Forms.PropertyGrid builderPropertyGrid;
        private System.Windows.Forms.Button generateScriptButton;
        private System.Windows.Forms.ListBox directivesListBox;
        private System.Windows.Forms.TabPage multipleOutputTabPage;
        private System.Windows.Forms.TableLayoutPanel multipeOutputTableLayoutPanel;
        private System.Windows.Forms.TextBox multipleOutputContentsTextBox;
        private System.Windows.Forms.ListView multipleOutputFileNameListView;
        private System.Windows.Forms.TextBox templateTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabPage gridOutputTabPage;
        private System.Windows.Forms.ListView gridListView;
        private System.Windows.Forms.TabPage diagnosticDumpTabPage;
        private System.Windows.Forms.TableLayoutPanel diagnosticDumpTableLayoutPanel;
        private System.Windows.Forms.Button saveDiagnosticDumpButton;
        private System.Windows.Forms.Button generateDiagnosticDumpButton;
        private System.Windows.Forms.TextBox diagnosticDumpTextBox;
    }
}

