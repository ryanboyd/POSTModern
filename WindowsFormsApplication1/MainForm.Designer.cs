namespace WindowsFormsApplication1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BgWorker = new System.ComponentModel.BackgroundWorker();
            this.ScanSubfolderCheckbox = new System.Windows.Forms.CheckBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.EncodingDropdown = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.IncludeOrderedPOSTagsCheckbox = new System.Windows.Forms.CheckBox();
            this.NormalizeOutputCheckbox = new System.Windows.Forms.CheckBox();
            this.SavePOStextCheckbox = new System.Windows.Forms.CheckBox();
            this.ModelSelectionBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BgWorker
            // 
            this.BgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgWorkerClean_DoWork);
            this.BgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgWorker_RunWorkerCompleted);
            // 
            // ScanSubfolderCheckbox
            // 
            this.ScanSubfolderCheckbox.AutoSize = true;
            this.ScanSubfolderCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScanSubfolderCheckbox.Location = new System.Drawing.Point(163, 376);
            this.ScanSubfolderCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ScanSubfolderCheckbox.Name = "ScanSubfolderCheckbox";
            this.ScanSubfolderCheckbox.Size = new System.Drawing.Size(161, 24);
            this.ScanSubfolderCheckbox.TabIndex = 2;
            this.ScanSubfolderCheckbox.Text = "Scan subfolders?";
            this.ScanSubfolderCheckbox.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.Color.Black;
            this.StartButton.Location = new System.Drawing.Point(141, 322);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(203, 39);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // FolderBrowser
            // 
            this.FolderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.FolderBrowser.ShowNewFolderButton = false;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoEllipsis = true;
            this.FilenameLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FilenameLabel.Location = new System.Drawing.Point(16, 418);
            this.FilenameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(453, 26);
            this.FilenameLabel.TabIndex = 6;
            this.FilenameLabel.Text = "Waiting to analyze texts...";
            this.FilenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "Repeatalizer.csv";
            this.saveFileDialog.Filter = "CSV Files|*.csv";
            this.saveFileDialog.Title = "Please choose where to save your output";
            // 
            // EncodingDropdown
            // 
            this.EncodingDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EncodingDropdown.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EncodingDropdown.FormattingEnabled = true;
            this.EncodingDropdown.Location = new System.Drawing.Point(8, 60);
            this.EncodingDropdown.Margin = new System.Windows.Forms.Padding(4);
            this.EncodingDropdown.Name = "EncodingDropdown";
            this.EncodingDropdown.Size = new System.Drawing.Size(437, 27);
            this.EncodingDropdown.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Encoding of Text Files:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.IncludeOrderedPOSTagsCheckbox);
            this.groupBox2.Controls.Add(this.NormalizeOutputCheckbox);
            this.groupBox2.Controls.Add(this.SavePOStextCheckbox);
            this.groupBox2.Controls.Add(this.ModelSelectionBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.EncodingDropdown);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(16, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(453, 281);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input / Output Options";
            // 
            // IncludeOrderedPOSTagsCheckbox
            // 
            this.IncludeOrderedPOSTagsCheckbox.AutoSize = true;
            this.IncludeOrderedPOSTagsCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncludeOrderedPOSTagsCheckbox.Location = new System.Drawing.Point(8, 213);
            this.IncludeOrderedPOSTagsCheckbox.Name = "IncludeOrderedPOSTagsCheckbox";
            this.IncludeOrderedPOSTagsCheckbox.Size = new System.Drawing.Size(228, 24);
            this.IncludeOrderedPOSTagsCheckbox.TabIndex = 15;
            this.IncludeOrderedPOSTagsCheckbox.Text = "Include ordered POS Tags";
            this.IncludeOrderedPOSTagsCheckbox.UseVisualStyleBackColor = true;
            // 
            // NormalizeOutputCheckbox
            // 
            this.NormalizeOutputCheckbox.AutoSize = true;
            this.NormalizeOutputCheckbox.Checked = true;
            this.NormalizeOutputCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NormalizeOutputCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NormalizeOutputCheckbox.Location = new System.Drawing.Point(8, 244);
            this.NormalizeOutputCheckbox.Name = "NormalizeOutputCheckbox";
            this.NormalizeOutputCheckbox.Size = new System.Drawing.Size(283, 24);
            this.NormalizeOutputCheckbox.TabIndex = 14;
            this.NormalizeOutputCheckbox.Text = "Normalize Output by Token Count";
            this.NormalizeOutputCheckbox.UseVisualStyleBackColor = true;
            // 
            // SavePOStextCheckbox
            // 
            this.SavePOStextCheckbox.AutoSize = true;
            this.SavePOStextCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SavePOStextCheckbox.Location = new System.Drawing.Point(8, 183);
            this.SavePOStextCheckbox.Name = "SavePOStextCheckbox";
            this.SavePOStextCheckbox.Size = new System.Drawing.Size(290, 24);
            this.SavePOStextCheckbox.TabIndex = 13;
            this.SavePOStextCheckbox.Text = "Include POS-tagged Text in Output";
            this.SavePOStextCheckbox.UseVisualStyleBackColor = true;
            // 
            // ModelSelectionBox
            // 
            this.ModelSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModelSelectionBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModelSelectionBox.FormattingEnabled = true;
            this.ModelSelectionBox.Location = new System.Drawing.Point(8, 129);
            this.ModelSelectionBox.Margin = new System.Windows.Forms.Padding(4);
            this.ModelSelectionBox.Name = "ModelSelectionBox";
            this.ModelSelectionBox.Size = new System.Drawing.Size(437, 27);
            this.ModelSelectionBox.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Tagging Model:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "DictionaryFile.txt";
            this.openFileDialog.Filter = "Dictionary Files|*.dic";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(482, 453);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ScanSubfolderCheckbox);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 500);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POSTModern";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker BgWorker;
        private System.Windows.Forms.CheckBox ScanSubfolderCheckbox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ComboBox EncodingDropdown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox ModelSelectionBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox SavePOStextCheckbox;
        private System.Windows.Forms.CheckBox NormalizeOutputCheckbox;
        private System.Windows.Forms.CheckBox IncludeOrderedPOSTagsCheckbox;
    }
}

