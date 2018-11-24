namespace FlashpointCurator
{
    partial class PlatformEditorForm
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
            this.components = new System.ComponentModel.Container();
            this.platforms = new System.Windows.Forms.ListBox();
            this.platformLogo = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.browseDestinationButton = new System.Windows.Forms.Button();
            this.applicationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.commandLineTextBox = new System.Windows.Forms.TextBox();
            this.helpBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.destinationTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.platformLogo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpBox)).BeginInit();
            this.SuspendLayout();
            // 
            // platforms
            // 
            this.platforms.FormattingEnabled = true;
            this.platforms.Location = new System.Drawing.Point(12, 12);
            this.platforms.Name = "platforms";
            this.platforms.Size = new System.Drawing.Size(174, 147);
            this.platforms.TabIndex = 0;
            this.platforms.SelectedIndexChanged += new System.EventHandler(this.platforms_SelectedIndexChanged);
            // 
            // platformLogo
            // 
            this.platformLogo.Enabled = false;
            this.platformLogo.Location = new System.Drawing.Point(192, 12);
            this.platformLogo.Name = "platformLogo";
            this.platformLogo.Size = new System.Drawing.Size(150, 147);
            this.platformLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.platformLogo.TabIndex = 1;
            this.platformLogo.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.browseDestinationButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.applicationTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.browseButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.commandLineTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.helpBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.destinationTextBox, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 165);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(330, 90);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // browseDestinationButton
            // 
            this.browseDestinationButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browseDestinationButton.Enabled = false;
            this.browseDestinationButton.Location = new System.Drawing.Point(295, 33);
            this.browseDestinationButton.Name = "browseDestinationButton";
            this.browseDestinationButton.Size = new System.Drawing.Size(30, 23);
            this.browseDestinationButton.TabIndex = 8;
            this.browseDestinationButton.Text = "...";
            this.browseDestinationButton.UseVisualStyleBackColor = true;
            this.browseDestinationButton.Click += new System.EventHandler(this.browseDestinationButton_Click);
            // 
            // applicationTextBox
            // 
            this.applicationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.applicationTextBox.Enabled = false;
            this.applicationTextBox.Location = new System.Drawing.Point(89, 5);
            this.applicationTextBox.Name = "applicationTextBox";
            this.applicationTextBox.Size = new System.Drawing.Size(200, 20);
            this.applicationTextBox.TabIndex = 2;
            this.applicationTextBox.TextChanged += new System.EventHandler(this.applicationTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application:";
            // 
            // browseButton
            // 
            this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browseButton.Enabled = false;
            this.browseButton.Location = new System.Drawing.Point(295, 3);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(30, 23);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Command Line:";
            // 
            // commandLineTextBox
            // 
            this.commandLineTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.commandLineTextBox.Enabled = false;
            this.commandLineTextBox.Location = new System.Drawing.Point(89, 65);
            this.commandLineTextBox.Name = "commandLineTextBox";
            this.commandLineTextBox.Size = new System.Drawing.Size(200, 20);
            this.commandLineTextBox.TabIndex = 3;
            this.commandLineTextBox.TextChanged += new System.EventHandler(this.commandLineTextBox_TextChanged);
            // 
            // helpBox
            // 
            this.helpBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.helpBox.Location = new System.Drawing.Point(295, 63);
            this.helpBox.Name = "helpBox";
            this.helpBox.Size = new System.Drawing.Size(30, 23);
            this.helpBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.helpBox.TabIndex = 6;
            this.helpBox.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Destination:";
            // 
            // destinationTextBox
            // 
            this.destinationTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.destinationTextBox.Enabled = false;
            this.destinationTextBox.Location = new System.Drawing.Point(89, 35);
            this.destinationTextBox.Name = "destinationTextBox";
            this.destinationTextBox.Size = new System.Drawing.Size(200, 20);
            this.destinationTextBox.TabIndex = 6;
            this.destinationTextBox.TextChanged += new System.EventHandler(this.destinationTextBox_TextChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(267, 276);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(12, 276);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(93, 276);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 5;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "exe";
            this.openFileDialog.Filter = "Applications (*.exe)|*.exe";
            // 
            // PlatformEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 311);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.platformLogo);
            this.Controls.Add(this.platforms);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "PlatformEditorForm";
            this.Text = "Platform Editor";
            ((System.ComponentModel.ISupportInitialize)(this.platformLogo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox platforms;
        private System.Windows.Forms.PictureBox platformLogo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox commandLineTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox applicationTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox helpBox;
        private System.Windows.Forms.ToolTip helpToolTip;
        private System.Windows.Forms.TextBox destinationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button browseDestinationButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}