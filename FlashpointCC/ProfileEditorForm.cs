using FlashpointCurator.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator
{
    public partial class ProfileEditorForm : Form
    {
        private string flashpointPath;

        public Profile CurrentProfile
        {
            get
            {
                return new Profile()
                {
                    Name = profileComboBox.Text,
                    Platform = platformComboBox.Text,
                    ApplicationPath = applicationTextBox.Text,
                    DestinationPath = destinationTextBox.Text,
                    CommandLine = commandLineTextBox.Text
                };
            }
            set
            {
                platformComboBox.Text = value.Platform;
                applicationTextBox.Text = value.ApplicationPath;
                destinationTextBox.Text = value.DestinationPath;
                commandLineTextBox.Text = value.CommandLine;
                ProfileChange?.Invoke(value);
            }
        }

        public event ProfileChanged ProfileChange;
        public delegate void ProfileChanged(Profile profile);

        public ProfileEditorForm(string flashpointPath)
        {
            this.flashpointPath = flashpointPath;
            InitializeComponent();
            if (File.Exists("profiles.json"))
            {
                var profiles = JsonConvert.DeserializeObject<Profile[]>(File.ReadAllText("profiles.json"));
                profileComboBox.Items.AddRange(profiles);
            }
            LoadPlatforms();
            saveButton.Click += (sender, e) => { LoadPlatforms(); };
            helpBox.Image = SystemIcons.Question.ToBitmap();
            helpBox.MouseHover += HelpBox_MouseHover;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            saveButton.Click += (sender, e) => { SaveProfiles(); };
            // Only called when standalone form is closed
            FormClosing += (sender, e) => { SaveProfiles(); };
        }

        private void HelpBox_MouseHover(object sender, EventArgs e)
        {
            helpToolTip.Show("You may use the following variables\n" +
                "%content_path% = Path to the game executable in the curation file.\n" +
                "%dest_path% = Path to the game executable in the destination folder.", helpBox);
        }

        private void LoadPlatforms()
        {
            var platformRepo = Path.Combine(flashpointPath, "Data", "Platforms");
            if (Directory.Exists(platformRepo))
            {
                var platforms = Directory.EnumerateFiles(platformRepo, "*.xml").Select(x => Path.GetFileNameWithoutExtension(x));
                platformComboBox.Items.AddRange(platforms.ToArray());
            }
        }

        private void SaveProfiles()
        {
            var profileName = CurrentProfile.Name;
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                var index = Math.Max(profileComboBox.SelectedIndex, profileComboBox.FindStringExact(profileName));
                if (index == -1)
                {
                    profileComboBox.Items.Add(CurrentProfile);
                }
                else
                {
                    profileComboBox.Items[index] = CurrentProfile;
                }
            }
            var profiles = profileComboBox.Items.Cast<Profile>().ToArray();
            File.WriteAllText("profiles.json", JsonConvert.SerializeObject(profiles));
        }

        private void profileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentProfile = (Profile)profileComboBox.Items[profileComboBox.SelectedIndex];
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Applications (*.exe)|*.exe"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                applicationTextBox.Text = PathUtil.GetRelativePath(path, dialog.FileName);
            }
        }

        private void browseDestinationButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                destinationTextBox.Text = PathUtil.GetRelativePath(path, dialog.SelectedPath);
            }
        }
    }
}
