using FlashpointCurator.Content;
using FlashpointCurator.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static FlashpointCurator.Utils.TreeNodeCollectionUtil;

namespace FlashpointCurator
{
    public partial class CurationForm : Form
    {
        private IContentSource source;
        private TreeNode executable;
        private string flashpointPath;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClientRect(System.IntPtr hWnd, ref Rectangle lpRECT);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern bool IsIconic(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetForegroundWindow(int hwnd);

        public static Bitmap logo;
        public static Bitmap gamescreen;

        public CurationForm(string flashpointPath)
        {
            this.flashpointPath = flashpointPath;
            InitializeComponent();
            treeView.AllowDrop = true;
            treeView.DragEnter += TreeView_DragEnter;
            treeView.DragDrop += TreeView_DragDrop;
            logoPictureBox.Paint += LogoPictureBox_Paint;
            logoPictureBox.DragEnter += LogoPictureBox_DragEnter;
            logoPictureBox.AllowDrop = true;
            logoPictureBox.DragDrop += LogoPictureBox_DragDrop;
            screenshotPictureBox.Paint += ScreenshotPictureBox_Paint;
            screenshotPictureBox.DragEnter += ScreenshotPictureBox_DragEnter;
            screenshotPictureBox.AllowDrop = true;
            screenshotPictureBox.DragDrop += ScreenshotPictureBox_DragDrop;
            genreComboBox.Items.AddRange(Curation.Genres);
            genreComboBox.SelectedIndex = 0;
            playModeComboBox.Items.AddRange(Curation.Modes);
            playModeComboBox.SelectedIndex = 0;
            statusComboBox.Items.AddRange(Curation.Statuses);
            statusComboBox.SelectedIndex = 0;
            platformComboBox.Items.AddRange(Platform.Platforms);
            platformComboBox.SelectedIndex = 0;
            treeView.MouseDown += TreeView_MouseDown;
            treeView.AfterExpand += TreeView_AfterExpand;
            treeView.AfterCollapse += TreeView_AfterCollapse;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void TreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }

        private void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 2;
            e.Node.SelectedImageIndex = 2;
        }

        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure this is the right button.
            if (e.Button != MouseButtons.Right) return;

            // Select this node.
            TreeNode node_here = treeView.GetNodeAt(e.X, e.Y);
            treeView.SelectedNode = node_here;

            // See if we got a node.
            if (node_here == null) return;

            if (node_here.Nodes.Count == 0)
            {
                flagAsExecutableToolStripMenuItem.Checked = node_here == executable;
                contextMenuStrip.Show(treeView, new Point(e.X, e.Y));
            }
        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void TreeView_DragDrop(object sender, DragEventArgs e)
        {
            SetContent(ContentSource.FromPath(((string[])e.Data.GetData(DataFormats.FileDrop))[0]));
        }

        private void ScreenshotPictureBox_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        private void LogoPictureBox_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Gray, ButtonBorderStyle.Solid);
        }

        public void SetLogo(Image image)
        {
            logoPictureBox.Image = image;
        }

        public void SetScreenshot(Image image)
        {
            screenshotPictureBox.Image = image;
        }

        public void SetContent(IContentSource content)
        {
            source = content;
            treeView.Nodes.Clear();
            treeView.Nodes.AddRange(source.GetTree());
            // Assign icons
            foreach (var node in treeView.Nodes.All(Filter.EXCLUDE_CHILDREN))
            {
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
            }
        }

        public void LoadCuration(Curation curation)
        {
            titleTextBox.Text = curation.Title;
            if (curation.ReleaseDate.HasValue)
            {
                dateTimePicker.Value = curation.ReleaseDate.Value;
                dateTimePicker.Checked = true;
            }
            genreComboBox.SelectedIndex = Math.Max(0, genreComboBox.FindString(curation.Genre));
            developerTextBox.Text = curation.Developer;
            seriesTextBox.Text = curation.Series;
            playModeComboBox.SelectedIndex = Math.Max(0, playModeComboBox.FindString(curation.PlayMode));
            statusComboBox.SelectedIndex = Math.Max(0, statusComboBox.FindString(curation.Status));
            sourceTextBox.Text = curation.Source;
            platformComboBox.SelectedIndex = Math.Max(0, platformComboBox.FindString(curation.PlayMode));
            publisherTextBox.Text = curation.Publisher;

            Platform platform = (Platform)platformComboBox.SelectedItem;
            var launchCommand = curation.LaunchCommand;
            var pattern = Regex.Replace(Regex.Escape(platform.CommandLine), "%(content|dest)_path%", "(.*)", RegexOptions.IgnoreCase);
            var match = Regex.Match(launchCommand, pattern);
            if (match.Success)
            {
                var executablePath = match.Groups[1].Value.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (launchCommand.IndexOf("%dest_path%", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    executablePath = executablePath.Substring(platform.DestinationPath.Length - 1);
                }

                var executable = treeView.Nodes.All(Filter.EXCLUDE_PARENTS)
                    .Where(n => executablePath.Equals(n.FullPath, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (executable != null)
                {
                    this.executable = executable;
                    executable.ImageIndex = 3;
                    executable.SelectedImageIndex = 3;
                }
            }
        }

        public static string GetLaunchCommand(string commandLine, TreeNode executable, string dest)
        {
            var executablePath = executable.FullPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            commandLine = Regex.Replace(commandLine, "%content_path%", executablePath, RegexOptions.IgnoreCase);
            commandLine = Regex.Replace(commandLine, "%dest_path%", Path.Combine(dest, executablePath), RegexOptions.IgnoreCase);
            return commandLine;
        }

        public bool IsValidImage(string path)
        {
            string[] extensions = { "png", "jpg", "jpeg", "gif", "bmp" };

            if (extensions.Contains(Path.GetExtension(path).ToLowerInvariant().Replace(".", "")))
            {
                return true;
            }

            else
            {
                MessageBox.Show("Please use a valid image format (PNG, JPG, JPEG, GIF, BMP)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        private void ScreenshotPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void LogoPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void ScreenshotPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string pic in ((string[])e.Data.GetData(DataFormats.FileDrop)))
            {
                if (IsValidImage(pic))
                {
                    Image img = Image.FromFile(pic);
                    screenshotPictureBox.Image = img;
                }
            }
        }

        private void LogoPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string pic in ((string[])e.Data.GetData(DataFormats.FileDrop)))
            {
                if (IsValidImage(pic))
                {
                    Image img = Image.FromFile(pic);
                    logoPictureBox.Image = img;
                }
            }
        }

        private void saveCurationButton_Click(object sender, EventArgs e)
        {
            if (source == null)
            {
                MessageBox.Show("Please add game content before saving the curation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (logoPictureBox.Image == null || screenshotPictureBox.Image == null)
            {
                MessageBox.Show("Please add a logo and screenshot before saving the curation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (executable == null)
            {
                MessageBox.Show("Please flag a file as an executable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var platform = (Platform)platformComboBox.SelectedItem;
            var dest = Path.Combine(flashpointPath, platform.DestinationPath);
            var commandLine = GetLaunchCommand(platform.CommandLine, executable, dest);
            Curation curation = new Curation
            {
                Title = titleTextBox.Text,
                Genre = genreComboBox.SelectedItem.ToString(),
                Developer = developerTextBox.Text,
                Series = seriesTextBox.Text,
                PlayMode = playModeComboBox.SelectedItem.ToString(),
                Status = statusComboBox.SelectedItem.ToString(),
                Source = sourceTextBox.Text,
                Platform = platform.ToString(),
                Publisher = publisherTextBox.Text,
                LaunchCommand = commandLine
            };
            if (dateTimePicker.Checked)
            {
                curation.ReleaseDate = dateTimePicker.Value;
            }
            string meta = MetaParser.Serialize(curation);
            var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                var metaStream = new MemoryStream(Encoding.UTF8.GetBytes(meta));
                var name = new string(curation.Title.Select(c => Path.GetInvalidPathChars().Contains(c) ? '_' : c).ToArray());
                var metaEntry = zip.CreateEntry(name + "/meta.txt");
                using (var entryStream = metaEntry.Open())
                {
                    metaStream.CopyTo(entryStream);
                }
                var logoEntry = zip.CreateEntry(name + "/logo.png");
                using (var entryStream = logoEntry.Open())
                {
                    logoPictureBox.Image.Save(entryStream, logoPictureBox.Image.RawFormat);
                }
                var ssEntry = zip.CreateEntry(name + "/ss.png");
                using (var entryStream = ssEntry.Open())
                {
                    screenshotPictureBox.Image.Save(entryStream, screenshotPictureBox.Image.RawFormat);
                }
                source.CopyToZip(zip, name);
            }
            File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
        }

        private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            saveCurationButton.Enabled = titleTextBox.Text.Length != 0;
        }

        private void flagAsExecutableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (executable != null)
            {
                executable.ImageIndex = 0;
                executable.SelectedImageIndex = 0;
            }
            // Allow unflagging a file as the executable
            if (flagAsExecutableToolStripMenuItem.Checked)
            {
                executable = null;
                return;
            }
            executable = node;
            node.ImageIndex = 3;
            node.SelectedImageIndex = 3;
        }

        private void curateButton_Click(object sender, EventArgs e)
        {
            if (executable == null)
            {
                MessageBox.Show("Please flag a file as an executable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var platform = (Platform)platformComboBox.SelectedItem;
            var dest = Path.Combine(flashpointPath, platform.DestinationPath);
            var commandLine = GetLaunchCommand(platform.CommandLine, executable, dest);
            var now = DateTime.UtcNow;
            Game game = new Game
            {
                ApplicationPath = platform.ApplicationPath,
                CommandLine = commandLine,
                DateAdded = now,
                DateModified = now,
                Developer = developerTextBox.Text,
                Id = Guid.NewGuid(),
                Platform = platform.Name,
                Publisher = publisherTextBox.Text,
                Source = sourceTextBox.Text,
                Title = titleTextBox.Text,
                Series = seriesTextBox.Text,
                PlayMode = playModeComboBox.SelectedItem.ToString(),
                Hide = extremeCheckBox.Checked,
                Genre = genreComboBox.SelectedItem.ToString()
            };
            var platformRepo = Path.Combine(flashpointPath, "Data", "Platforms");
            var dataFile = Path.Combine(platformRepo, platform.Name + ".xml");

            string xml;
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(List<Game>), new XmlRootAttribute("LaunchBox"));
                serializer.Serialize(writer, new List<Game>() { game });
                xml = Encoding.UTF8.GetString(ms.ToArray());
            }
            var lines = File.Exists(dataFile) ? File.ReadAllLines(dataFile).ToList() : null;
            if (lines == null || lines.Count < 3)
            {
                File.WriteAllText(dataFile, xml);
            }
            else
            {
                var xmlLines = xml.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                // Skip xml declaration and root element tags
                lines.InsertRange(2, xmlLines.ToList().GetRange(2, xmlLines.Length - 3));
                File.WriteAllLines(dataFile, lines);
            }
            source.CopyTo(dest);
            var platformImagesRepo = Path.Combine(flashpointPath, "Images", platform.Name);
            var logoPath = Path.Combine(platformImagesRepo, "Box - Front", titleTextBox.Text + "-01.png");
            var ssPath = Path.Combine(platformImagesRepo, "Screenshot - Gameplay", titleTextBox.Text + "-01.png");
            Directory.CreateDirectory(Path.GetDirectoryName(logoPath));
            Directory.CreateDirectory(Path.GetDirectoryName(ssPath));
            logoPictureBox.Image.Save(logoPath, ImageFormat.Png);
            screenshotPictureBox.Image.Save(ssPath, ImageFormat.Png);
            MessageBox.Show("Added to Flashpoint!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public Bitmap CaptureGameWindow()
        {
            Platform platform = (Platform)platformComboBox.SelectedItem;
            Process[] players = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(platform.ApplicationPath));

            if (players.Length == 0)
            {
                MessageBox.Show("No game window detected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            IntPtr player = players[0].MainWindowHandle;

            Rectangle pic = new Rectangle();
            GetClientRect(player, ref pic);

            ScreenShot.ScreenCapture screenshot = new ScreenShot.ScreenCapture();
            Bitmap image = (Bitmap)screenshot.CaptureWindow(player);

            int border = (image.Width - pic.Width) / 2;
            int top = image.Height - pic.Height - border;

            Rectangle crop = new Rectangle(border, top, pic.Width, pic.Height);

            if (IsIconic(player) | crop.IsEmpty | crop.Width == 0 | crop.Height == 0)
            {
                MessageBox.Show("Please make sure the game is not minimized", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SetForegroundWindow((int)player);
                SendKeys.SendWait("~");
                return null;
            }

            else
            {
                Bitmap cropped = image.Clone(crop, image.PixelFormat);
                return cropped;
            }
        }

        private void captureLogoButton_Click(object sender, EventArgs e)
        {
            logo = CaptureGameWindow();
            logoPictureBox.Image = logo;
        }

        private void captureScreenshotButton_Click(object sender, EventArgs e)
        {
            gamescreen = CaptureGameWindow();
            screenshotPictureBox.Image = gamescreen;
        }
    }
}
