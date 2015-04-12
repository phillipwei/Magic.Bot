namespace Magic.App
{
    partial class DraftRecorder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DraftRecorder));
            this.screenShotButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.folderCombo = new System.Windows.Forms.ComboBox();
            this.setNameCombo = new System.Windows.Forms.ComboBox();
            this.folderButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // screenShotButton
            // 
            this.screenShotButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screenShotButton.Location = new System.Drawing.Point(3, 351);
            this.screenShotButton.Name = "screenShotButton";
            this.screenShotButton.Size = new System.Drawing.Size(120, 39);
            this.screenShotButton.TabIndex = 0;
            this.screenShotButton.Text = "Screenshot";
            this.screenShotButton.UseVisualStyleBackColor = true;
            this.screenShotButton.Click += new System.EventHandler(this.ScreenShotButtonClick);
            // 
            // logTextBox
            // 
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.logTextBox, 2);
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(3, 3);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(246, 288);
            this.logTextBox.TabIndex = 2;
            this.logTextBox.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.folderCombo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.logTextBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.setNameCombo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.screenShotButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.folderButton, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(252, 393);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // folderCombo
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.folderCombo, 2);
            this.folderCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderCombo.FormattingEnabled = true;
            this.folderCombo.Location = new System.Drawing.Point(3, 297);
            this.folderCombo.Name = "folderCombo";
            this.folderCombo.Size = new System.Drawing.Size(246, 21);
            this.folderCombo.TabIndex = 5;
            // 
            // setNameCombo
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.setNameCombo, 2);
            this.setNameCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setNameCombo.FormattingEnabled = true;
            this.setNameCombo.Location = new System.Drawing.Point(3, 324);
            this.setNameCombo.Name = "setNameCombo";
            this.setNameCombo.Size = new System.Drawing.Size(246, 21);
            this.setNameCombo.TabIndex = 4;
            // 
            // folderButton
            // 
            this.folderButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderButton.Location = new System.Drawing.Point(129, 351);
            this.folderButton.Name = "folderButton";
            this.folderButton.Size = new System.Drawing.Size(120, 39);
            this.folderButton.TabIndex = 3;
            this.folderButton.Text = "Folder";
            this.folderButton.UseVisualStyleBackColor = true;
            this.folderButton.Click += new System.EventHandler(this.FolderButtonClick);
            // 
            // DraftRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 393);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DraftRecorder";
            this.Text = "Draft Record";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button screenShotButton;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button folderButton;
        private System.Windows.Forms.ComboBox folderCombo;
        private System.Windows.Forms.ComboBox setNameCombo;
    }
}

