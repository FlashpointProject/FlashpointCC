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
using System.Reflection;
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
        private Curation curation;
        private TreeNode executable;
        private string flashpointPath;
        
        public delegate void SetImage(Image image);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClientRect(System.IntPtr hWnd, ref Rectangle lpRECT);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern bool IsIconic(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetForegroundWindow(int hwnd);

        public CurationForm()
        {
            flashpointPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
            profileComboBox.Items.AddRange(ProfileEditorForm.LoadProfiles());
            genreComboBox.Items.AddRange(Curation.Genres);
            playModeComboBox.Items.AddRange(Curation.Modes);
            statusComboBox.Items.AddRange(Curation.Statuses);
            treeView.MouseDown += TreeView_MouseDown;
            treeView.AfterExpand += TreeView_AfterExpand;
            treeView.AfterCollapse += TreeView_AfterCollapse;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            SetImage SetL = SetLogo;
            SetImage SetSS = SetScreenshot;

            //Create instructions and Browse/Capture/Remove/Crop buttons for logo PictureBox
            Label logoinstructions = new Label();
            logoinstructions.TextAlign = ContentAlignment.MiddleCenter;
            logoinstructions.AutoSize = true;
            logoinstructions.Text = "Drag and drop an image here, or...";
            logoPictureBox.Controls.Add(logoinstructions);
            logoinstructions.Location = new Point((logoPictureBox.Width / 2) - (logoinstructions.Width / 2), (logoPictureBox.Height / 2) - (logoinstructions.Height / 2) - 15);

            Button logocapturebutton = new Button();
            logocapturebutton.Text = "Capture";
            logoPictureBox.Controls.Add(logocapturebutton);
            logocapturebutton.Click += (sender, e) => { CaptureWindow(SetL); };
            logocapturebutton.Location = new Point((logoPictureBox.Width / 2) - logocapturebutton.Width - 5, (logoPictureBox.Height / 2) - (logocapturebutton.Height / 2) + 10);

            Button logobrowsebutton = new Button();
            logobrowsebutton.Text = "Browse...";
            logoPictureBox.Controls.Add(logobrowsebutton);
            logobrowsebutton.Click += (sender, e) => { BrowseForImage(SetL); };
            logobrowsebutton.Location = new Point((logoPictureBox.Width / 2) + 5, (logoPictureBox.Height / 2) - (logobrowsebutton.Height / 2) + 10);

            Button logoremovebutton = new Button();
            logoremovebutton.Text = "X";
            logoremovebutton.Name = "logoremovebutton";
            logoremovebutton.Visible = false;
            logoremovebutton.Width = 26;
            logoremovebutton.BackColor = Color.PaleVioletRed;
            logoremovebutton.ForeColor = Color.White;
            logoremovebutton.Font = new Font(logoremovebutton.Font, FontStyle.Bold);
            logoPictureBox.Controls.Add(logoremovebutton);
            logoremovebutton.Click += (sender, e) => { SetLogo(null); };
            logoremovebutton.Location = new Point(logoPictureBox.Width - logoremovebutton.Width - 5, 5);

            //Button logocropbutton = new Button();
            //logocropbutton.Text = "Crop";
            //logocropbutton.Name = "logocropbutton";
            //logocropbutton.Visible = false;
            //logoPictureBox.Controls.Add(logocropbutton);
            //logocropbutton.Location = new Point((logoPictureBox.Width / 2) - (logocropbutton.Width / 2), logoPictureBox.Height - logocropbutton.Height - 8);

            //Create instructions and Browse/Capture/Remove buttons for screenshot PictureBox
            Label screenshotinstructions = new Label();
            screenshotinstructions.TextAlign = ContentAlignment.MiddleCenter;
            screenshotinstructions.AutoSize = true;
            screenshotinstructions.Text = "Drag and drop an image here, or...";
            screenshotPictureBox.Controls.Add(screenshotinstructions);
            screenshotinstructions.Location = new Point((screenshotPictureBox.Width / 2) - (screenshotinstructions.Width / 2), (screenshotPictureBox.Height / 2) - (screenshotinstructions.Height / 2) - 15);

            Button screenshotcapturebutton = new Button();
            screenshotcapturebutton.Text = "Capture";
            screenshotPictureBox.Controls.Add(screenshotcapturebutton);
            screenshotcapturebutton.Click += (sender, e) => { CaptureWindow(SetSS); };
            screenshotcapturebutton.Location = new Point((screenshotPictureBox.Width / 2) - screenshotcapturebutton.Width - 5, (screenshotPictureBox.Height / 2) - (screenshotcapturebutton.Height / 2) + 10);

            Button screenshotbrowsebutton = new Button();
            screenshotbrowsebutton.Text = "Browse...";
            screenshotPictureBox.Controls.Add(screenshotbrowsebutton);
            screenshotbrowsebutton.Click += (sender, e) => { BrowseForImage(SetSS); };
            screenshotbrowsebutton.Location = new Point((screenshotPictureBox.Width / 2) + 5, (screenshotPictureBox.Height / 2) - (screenshotbrowsebutton.Height / 2) + 10);

            Button screenshotremovebutton = new Button();
            screenshotremovebutton.Text = "X";
            screenshotremovebutton.Name = "screenshotremovebutton";
            screenshotremovebutton.Visible = false;
            screenshotremovebutton.Width = 26;
            screenshotremovebutton.BackColor = Color.PaleVioletRed;
            screenshotremovebutton.ForeColor = Color.White;
            screenshotremovebutton.Font = new Font(screenshotremovebutton.Font, FontStyle.Bold);
            screenshotPictureBox.Controls.Add(screenshotremovebutton);
            screenshotremovebutton.Click += (sender, e) => { SetScreenshot(null); };
            screenshotremovebutton.Location = new Point(screenshotPictureBox.Width - screenshotremovebutton.Width - 5, 5);

            //Create swap button
            Button swapbutton = new Button();
            swapbutton.Text = "<- Swap ->";
            swapbutton.Visible = false;
            swapbutton.Name = "swapbutton";
            this.Controls.Add(swapbutton);
            swapbutton.Click += (sender, e) => { SwapPictures(); };
            swapbutton.Location = new Point(tableLayoutPanel2.Location.X + (tableLayoutPanel2.Width/2) - (swapbutton.Width/2), tableLayoutPanel2.Location.Y + tableLayoutPanel2.Height - 10);
            swapbutton.BringToFront();
        }

        private void ProfileEditor_ProfileChange(Profile profile)
        {
            if (curation == null) return;
            var launchCommand = curation.LaunchCommand;
            var pattern = Regex.Replace(Regex.Escape(profile.CommandLine), "%(content|dest)_path%", "(.*)", RegexOptions.IgnoreCase);
            var match = Regex.Match(launchCommand, pattern);
            if (match.Success)
            {
                var executablePath = new UriBuilder(match.Groups[1].Value).Uri.GetComponents(UriComponents.Host & UriComponents.Path, UriFormat.Unescaped);
                executablePath = executablePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                if (launchCommand.IndexOf("%dest_path%", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    executablePath = executablePath.Substring(profile.DestinationPath.Length - 1);
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
            Pen pen = new Pen(Color.FromArgb(255, 170, 170, 170), 2.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            e.Graphics.DrawRectangle(pen, e.ClipRectangle.X + 1, e.ClipRectangle.Y + 1, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            pen.Dispose();
        }

        private void LogoPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(255, 170, 170, 170), 2.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            e.Graphics.DrawRectangle(pen, e.ClipRectangle.X + 1, e.ClipRectangle.Y + 1, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            pen.Dispose();
        }

        public void SetLogo(Image image)
        {
            logoPictureBox.Image = image;
            this.Controls.Find("swapbutton", false)[0].Visible = (logoPictureBox.Image == null && screenshotPictureBox.Image == null) ? false : true;

            foreach (Control child in logoPictureBox.Controls)
            {
                child.Visible = (image == null) ? true : false;
                if (child.Name == "logocropbutton" | child.Name == "logoremovebutton")
                {
                    child.Visible = (image == null) ? false : true;
                }
            }

            this.Refresh();
        }

        public void SetScreenshot(Image image)
        {
            screenshotPictureBox.Image = image;
            this.Controls.Find("swapbutton", false)[0].Visible = (logoPictureBox.Image == null && screenshotPictureBox.Image == null) ? false : true;

            foreach (Control child in screenshotPictureBox.Controls)
            {
                child.Visible = (image == null) ? true : false;
                if (child.Name == "screenshotremovebutton")
                {
                    child.Visible = (image == null) ? false : true;
                }
            }

            this.Refresh();
        }

        public void CaptureWindow(SetImage pass)
        {
            Profile profile = (Profile)profileComboBox.SelectedItem;

            if(profile == null)
            {
                MessageBox.Show("Please select a Profile", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process[] players = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(profile.ApplicationPath));

            if(players.Length == 0)
            {
                MessageBox.Show("No game window detected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IntPtr player = players[0].MainWindowHandle;

            Rectangle pic = new Rectangle();
            GetClientRect(player, ref pic);

            ScreenShot.ScreenCapture screenshot = new ScreenShot.ScreenCapture();
            Bitmap image = (Bitmap)screenshot.CaptureWindow(player);

            int border = (image.Width - pic.Width) / 2;
            int top = image.Height - pic.Height - border;

            Rectangle crop = new Rectangle(border, top, pic.Width, pic.Height);

            if(IsIconic(player) | crop.IsEmpty | crop.Width == 0 | crop.Height == 0)
            {
                MessageBox.Show("Please make sure the game is not minimized", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SetForegroundWindow((int)player);
                SendKeys.SendWait("~");
                return;
            }

            else
            {
                Bitmap cropped = image.Clone(crop, image.PixelFormat);
                pass(cropped);
            }
        }

        public void BrowseForImage(SetImage pass)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.FilterIndex = 2;
            browse.Filter = "BMP Image (*.bmp)|*.bmp|PNG Image (*.png)|*.png|JPEG Image (*.jpg, *.jpeg)|*.jpg; *.jpeg|GIF Image (*.gif)|*.gif";
            browse.CheckFileExists = true;
            browse.CheckPathExists = true;
            browse.ValidateNames = true;
            DialogResult result = browse.ShowDialog();
            if(result == DialogResult.OK)
            {
                pass(LoadImage(browse.FileName));
            }
        }

        public void SwapPictures()
        {
            Image temp = logoPictureBox.Image;
            SetLogo(screenshotPictureBox.Image);
            SetScreenshot(temp);
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
            extremeCheckBox.Checked = (curation.Extreme == "Yes");
            playModeComboBox.SelectedIndex = Math.Max(0, playModeComboBox.FindString(curation.PlayMode));
            statusComboBox.SelectedIndex = Math.Max(0, statusComboBox.FindString(curation.Status));
            sourceTextBox.Text = curation.Source;
            publisherTextBox.Text = curation.Publisher;
            notesTextBox.Text = curation.Notes;
            authorNotesTextBox.Text = curation.AuthorNotes;
            this.curation = curation;
        }

        public static string GetLaunchCommand(string commandLine, TreeNode executable, string dest)
        {
            var executablePath = executable.FullPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            commandLine = Regex.Replace(commandLine, "%content_path%", executablePath, RegexOptions.IgnoreCase);
            commandLine = Regex.Replace(commandLine, "%dest_path%", Path.Combine(dest, executablePath), RegexOptions.IgnoreCase);
            return commandLine;
        }

        private void ScreenshotPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void LogoPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        public Image LoadImage(string path)
        {
            return Image.FromFile(path);
        }

        private void ScreenshotPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string pic in ((string[])e.Data.GetData(DataFormats.FileDrop)))
            {
                Image img = LoadImage(pic);
                screenshotPictureBox.Image = img;
                SetScreenshot(img);
            }
        }

        private void LogoPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string pic in ((string[])e.Data.GetData(DataFormats.FileDrop)))
            {
                Image img = LoadImage(pic);
                SetLogo(img);
            }
        }

        private void saveCurationButton_Click(object sender, EventArgs e)
        {
            if (profileComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a profile before saving the curation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            var profile = (Profile)profileComboBox.SelectedItem;
            var dest = Path.Combine(flashpointPath, profile.DestinationPath);
            var commandLine = GetLaunchCommand(profile.CommandLine, executable, dest);
            Curation curation = new Curation
            {
                Title = titleTextBox.Text,
                Genre = genreComboBox.SelectedItem.ToString(),
                Developer = developerTextBox.Text,
                Series = seriesTextBox.Text,
                PlayMode = playModeComboBox.SelectedItem.ToString(),
                Status = statusComboBox.SelectedItem.ToString(),
                Source = sourceTextBox.Text,
                Extreme = extremeCheckBox.Checked ? "Yes" : "No",
                Platform = profile.Platform,
                Publisher = publisherTextBox.Text,
                LaunchCommand = commandLine,
                Notes = notesTextBox.Text,
                AuthorNotes = authorNotesTextBox.Text
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
                    logoPictureBox.Image.Save(entryStream, ImageFormat.Png);
                }
                var ssEntry = zip.CreateEntry(name + "/ss.png");
                using (var entryStream = ssEntry.Open())
                {
                    screenshotPictureBox.Image.Save(entryStream, ImageFormat.Png);
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you wish to reset all fields?", "New", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                SetLogo(null);
                SetScreenshot(null);
                titleTextBox.Text = "";
                seriesTextBox.Text = "";
                profileComboBox.Text = "";
                developerTextBox.Text = "";
                publisherTextBox.Text = "";
                sourceTextBox.Text = "";
                dateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                dateTimePicker.Checked = false;
                genreComboBox.Text = "Genre";
                playModeComboBox.Text = "Playmode";
                statusComboBox.Text = "Status";
                extremeCheckBox.Checked = false;
                notesTextBox.Text = "";
                authorNotesTextBox.Text = "";
                treeView.Nodes.Clear();
            }
        }

        private void importCurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openCurationFileDialog.ShowDialog() == DialogResult.OK)
            {
                var logoStream = new MemoryStream();
                var ssStream = new MemoryStream();
                var metaStream = new MemoryStream();
                using (var zip = ZipFile.OpenRead(openCurationFileDialog.FileName))
                {
                    ZipArchiveEntry logoEntry, ssEntry, metaEntry;
                    if (!zip.TryFind("logo.png", out logoEntry))
                    {
                        MessageBox.Show("Cannot import curation. Missing logo.png.", "Invalid curation.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!zip.TryFind("ss.png", out ssEntry))
                    {
                        MessageBox.Show("Cannot import curation. Missing ss.png.", "Invalid curation.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!zip.TryFind("meta.txt", out metaEntry))
                    {
                        MessageBox.Show("Cannot import curation. Missing meta.txt.", "Invalid curation.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    logoEntry.Open().CopyTo(logoStream);
                    ssEntry.Open().CopyTo(ssStream);
                    metaEntry.Open().CopyTo(metaStream);
                }
                SetLogo(Image.FromStream(logoStream));
                SetScreenshot(Image.FromStream(ssStream));
                SetContent(ZipContentSource.FromPath(openCurationFileDialog.FileName));
                LoadCuration(MetaParser.Deserialize<Curation>(metaStream));
            }
        }

        private void addToFlashpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a profile before saving the curation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (executable == null)
            {
                MessageBox.Show("Please flag a file as an executable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var profile = (Profile)profileComboBox.SelectedItem;
            var dest = Path.Combine(flashpointPath, profile.DestinationPath);
            var commandLine = GetLaunchCommand(profile.CommandLine, executable, dest);
            var now = DateTime.UtcNow;
            Game game = new Game
            {
                ApplicationPath = profile.ApplicationPath,
                CommandLine = commandLine,
                DateAdded = now,
                DateModified = now,
                Developer = developerTextBox.Text,
                Id = Guid.NewGuid(),
                Platform = profile.Platform,
                Publisher = publisherTextBox.Text,
                Source = sourceTextBox.Text,
                Title = titleTextBox.Text,
                Series = seriesTextBox.Text,
                PlayMode = playModeComboBox.SelectedItem.ToString(),
                Hide = extremeCheckBox.Checked,
                Genre = genreComboBox.SelectedItem.ToString()
            };
            var platformRepo = Path.Combine(flashpointPath, "Data", "Platforms");
            var dataFile = Path.Combine(platformRepo, profile.Platform + ".xml");

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
            var platformImagesRepo = Path.Combine(flashpointPath, "Images", profile.Platform);
            var logoPath = Path.Combine(platformImagesRepo, "Box - Front", titleTextBox.Text + "-01.png");
            var ssPath = Path.Combine(platformImagesRepo, "Screenshot - Gameplay", titleTextBox.Text + "-01.png");
            Directory.CreateDirectory(Path.GetDirectoryName(logoPath));
            Directory.CreateDirectory(Path.GetDirectoryName(ssPath));
            logoPictureBox.Image.Save(logoPath, ImageFormat.Png);
            screenshotPictureBox.Image.Save(ssPath, ImageFormat.Png);
            MessageBox.Show("Added to Flashpoint!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void selectFlashpointPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                flashpointPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ProfileEditorForm(flashpointPath).ShowDialog();

            // Reload profiles
            profileComboBox.Items.Clear();
            profileComboBox.Items.AddRange(ProfileEditorForm.LoadProfiles());

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CurationForm_Move(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
