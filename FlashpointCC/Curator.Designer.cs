namespace FlashpointCurator
{
    partial class Curator
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFlashpointPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnPrioritiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToMediaWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.mediaWikiPage = new System.Windows.Forms.TabPage();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openCurationFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.profileEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.mediaWikiPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.selectFlashpointPathToolStripMenuItem,
            this.importCurationToolStripMenuItem,
            this.newCurationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // selectFlashpointPathToolStripMenuItem
            // 
            this.selectFlashpointPathToolStripMenuItem.Name = "selectFlashpointPathToolStripMenuItem";
            this.selectFlashpointPathToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.selectFlashpointPathToolStripMenuItem.Text = "Select Flashpoint Path...";
            this.selectFlashpointPathToolStripMenuItem.Click += new System.EventHandler(this.selectFlashpointPathToolStripMenuItem_Click);
            // 
            // importCurationToolStripMenuItem
            // 
            this.importCurationToolStripMenuItem.Name = "importCurationToolStripMenuItem";
            this.importCurationToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.importCurationToolStripMenuItem.Text = "Import Curation";
            this.importCurationToolStripMenuItem.Click += new System.EventHandler(this.importCurationToolStripMenuItem_Click);
            // 
            // newCurationToolStripMenuItem
            // 
            this.newCurationToolStripMenuItem.Name = "newCurationToolStripMenuItem";
            this.newCurationToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.newCurationToolStripMenuItem.Text = "New Curation";
            this.newCurationToolStripMenuItem.Click += new System.EventHandler(this.newCurationToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.columnPrioritiesToolStripMenuItem,
            this.profileEditorToolStripMenuItem,
            this.convertToMediaWikiToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // columnPrioritiesToolStripMenuItem
            // 
            this.columnPrioritiesToolStripMenuItem.Enabled = false;
            this.columnPrioritiesToolStripMenuItem.Name = "columnPrioritiesToolStripMenuItem";
            this.columnPrioritiesToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.columnPrioritiesToolStripMenuItem.Text = "Column Priorities";
            this.columnPrioritiesToolStripMenuItem.Click += new System.EventHandler(this.columnPrioritiesToolStripMenuItem_Click);
            // 
            // convertToMediaWikiToolStripMenuItem
            // 
            this.convertToMediaWikiToolStripMenuItem.Name = "convertToMediaWikiToolStripMenuItem";
            this.convertToMediaWikiToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.convertToMediaWikiToolStripMenuItem.Text = "Convert to MediaWiki";
            this.convertToMediaWikiToolStripMenuItem.Click += new System.EventHandler(this.convertToMediaWikiToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xml";
            this.openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(786, 394);
            this.dataGridView.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.mediaWikiPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 426);
            this.tabControl.TabIndex = 1;
            // 
            // mediaWikiPage
            // 
            this.mediaWikiPage.Controls.Add(this.richTextBox);
            this.mediaWikiPage.Location = new System.Drawing.Point(4, 22);
            this.mediaWikiPage.Name = "mediaWikiPage";
            this.mediaWikiPage.Size = new System.Drawing.Size(792, 400);
            this.mediaWikiPage.TabIndex = 1;
            this.mediaWikiPage.Text = "MediaWiki";
            this.mediaWikiPage.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(792, 400);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // openCurationFileDialog
            // 
            this.openCurationFileDialog.DefaultExt = "zip";
            this.openCurationFileDialog.Filter = "ZIP Archives (*.zip)|*.zip";
            // 
            // profileEditorToolStripMenuItem
            // 
            this.profileEditorToolStripMenuItem.Name = "profileEditorToolStripMenuItem";
            this.profileEditorToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.profileEditorToolStripMenuItem.Text = "Profile Editor";
            this.profileEditorToolStripMenuItem.Click += new System.EventHandler(this.profileEditorToolStripMenuItem_Click);
            // 
            // Curator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Curator";
            this.Text = "Curator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.mediaWikiPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnPrioritiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToMediaWikiToolStripMenuItem;
        private System.Windows.Forms.TabPage mediaWikiPage;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ToolStripMenuItem selectFlashpointPathToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem importCurationToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openCurationFileDialog;
        private System.Windows.Forms.ToolStripMenuItem newCurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem profileEditorToolStripMenuItem;
    }
}

