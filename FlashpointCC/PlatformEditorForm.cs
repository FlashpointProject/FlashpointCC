using FlashpointCurator.Utils;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator
{
    public partial class PlatformEditorForm : Form
    {
        public PlatformEditorForm()
        {
            InitializeComponent();
            platformLogo.Paint += PlatformLogo_Paint;
            platformLogo.AllowDrop = true;
            platformLogo.DragEnter += PlatformLogo_DragEnter;
            platformLogo.DragDrop += PlatformLogo_DragDrop;
            helpBox.Image = SystemIcons.Question.ToBitmap();
            helpBox.MouseHover += HelpBox_MouseHover;
            if (Platform.Platforms != null)
            {
                platforms.Items.AddRange(Platform.Platforms);
            }
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void HelpBox_MouseHover(object sender, EventArgs e)
        {
            helpToolTip.Show("You may use the following variables\n" +
                "%content_path% = Path to the game executable in the curation file.\n" +
                "%dest_path% = Path to the game executable in the destination folder.", helpBox);
        }

        private void PlatformLogo_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PlatformLogo_DragDrop(object sender, DragEventArgs e)
        {
            var file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            var platform = (Platform)platforms.SelectedItem;
            var logoRepo = PathUtil.GetProgramPath("logos");
            var logoFile = Path.Combine(logoRepo, string.Format("{0}.png", platform.Name));
            Directory.CreateDirectory(logoRepo);
            Image img;
            try
            {
                img = Image.FromFile(file);
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Image cannot be read.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(logoFile))
            {
                File.Delete(logoFile);
            }
            platformLogo.Image = img;
            img.Save(logoFile, ImageFormat.Png);
        }

        private void PlatformLogo_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string name = Interaction.InputBox("Enter a name for the platform:", "Platform name");
            if (name != string.Empty)
            {
                int idx = platforms.Items.Add(new Platform() { Name = name, CommandLine = "%dest_path%" });
                platforms.SelectedIndex = idx;
                EnableIfConfigured();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            platforms.Items.RemoveAt(platforms.SelectedIndex);
        }

        private void platforms_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = platforms.SelectedIndex != -1;

            // Defaults
            string applicationPath = string.Empty;
            string destination = string.Empty;
            string commandLine = string.Empty;
            Image img = null;

            // Load selected platform.
            if (selected)
            {
                var platform = (Platform)platforms.SelectedItem;
                var logoRepo = PathUtil.GetProgramPath("logos");
                var logoFile = Path.Combine(logoRepo, string.Format("{0}.png", platform.Name));
                if (File.Exists(logoFile))
                {
                    img = Image.FromFile(logoFile);
                }
                applicationPath = platform.ApplicationPath;
                destination = platform.DestinationPath;
                commandLine = platform.CommandLine;
            }

            // Establish control values.
            platformLogo.Image = img;
            applicationTextBox.Text = applicationPath;
            destinationTextBox.Text = destination;
            commandLineTextBox.Text = commandLine;

            // Disables components that should not be accessible if no platform is selected.
            platformLogo.Enabled = selected;
            applicationTextBox.Enabled = selected;
            destinationTextBox.Enabled = selected;
            commandLineTextBox.Enabled = selected;
            browseButton.Enabled = selected;
            browseDestinationButton.Enabled = selected;
            removeButton.Enabled = selected;

            // You should be able to add new platforms if none present, of course.
            EnableIfConfigured();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                applicationTextBox.Text = PathUtil.GetRelativePath(path, openFileDialog.FileName);
            }
        }

        private void browseDestinationButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                destinationTextBox.Text = PathUtil.GetRelativePath(path, folderBrowserDialog.SelectedPath);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var platforms = this.platforms.Items.Cast<Platform>().ToArray();
            File.WriteAllText("platforms.json", JsonConvert.SerializeObject(platforms));
            Platform.Platforms = platforms;
        }

        private void applicationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (platforms.SelectedIndex != -1)
            {
                ((Platform)platforms.SelectedItem).ApplicationPath = applicationTextBox.Text;
                EnableIfConfigured();
            }
        }

        private void destinationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (platforms.SelectedIndex != -1)
            {
                ((Platform)platforms.SelectedItem).DestinationPath = destinationTextBox.Text;
                EnableIfConfigured();
            }
        }

        private void commandLineTextBox_TextChanged(object sender, EventArgs e)
        {
            if (platforms.SelectedIndex != -1)
            {
                ((Platform)platforms.SelectedItem).CommandLine = commandLineTextBox.Text;
                EnableIfConfigured();
            }
        }

        // Disables controls that should not be accessible if the platform is not correctly configured
        // like the save button, add button or platform list box (the latter two change your selected index).
        // The remove button also changes your selected index but you can only remove the current platform.
        private bool EnableIfConfigured()
        {
            bool enable = platforms.SelectedIndex == -1 || ((Platform)platforms.SelectedItem).IsConfigured();
            addButton.Enabled = enable;
            saveButton.Enabled = enable;
            platforms.Enabled = enable;
            return enable;
        }
    }
}
