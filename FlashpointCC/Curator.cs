using FlashpointCurator.Content;
using FlashpointCurator.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FlashpointCurator
{
    public partial class Curator : Form
    {
        private string flashpointPath;

        public Curator()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            flashpointPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            folderBrowserDialog.SelectedPath = flashpointPath;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            List<Game> games;
            using (var reader = new StreamReader(openFileDialog.FileName))
            {
                var deserializer = new XmlSerializer(typeof(List<Game>),
                    new XmlRootAttribute("LaunchBox"));
                games = (List<Game>)deserializer.Deserialize(reader);
            }
            dataGridView.DataSource = games;
            foreach (var prop in from prop in typeof(Game).GetProperties() where Attribute.IsDefined(prop, typeof(Column)) select prop)
            {
                Column col = (Column)prop.GetCustomAttribute(typeof(Column));
                if (col.HideByDefault)
                {
                    dataGridView.Columns[prop.Name].Visible = false;
                }
            }
            columnPrioritiesToolStripMenuItem.Enabled = true;
        }

        private void columnPrioritiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ColumnPriorityForm(dataGridView).Show();
        }

        private void convertToMediaWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder("{| class=\"wikitable sortable\"");
            var visibleColumns = (from DataGridViewColumn col in dataGridView.Columns where col.Visible orderby col.DisplayIndex select col).ToArray();
            sb.Append("\n|-\n!");
            for (int i = 0; i < visibleColumns.Length; i++)
            {
                if (i > 0) sb.Append("!!");
                sb.Append(" " + visibleColumns[i].Name + " ");
            }
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                sb.Append("\n|-\n|");
                for (int i = 0; i < visibleColumns.Length; i++)
                {
                    if (i > 0) sb.Append("||");
                    string value = row.Cells[dataGridView.Columns.IndexOf(visibleColumns[i])].FormattedValue.ToString();
                    sb.Append(" " + (value == string.Empty ? "N/A" : value) + " ");
                }
            }
            sb.Append("\n|}");
            richTextBox.Text = sb.ToString();
        }

        private void selectFlashpointPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                flashpointPath = folderBrowserDialog.SelectedPath;
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
                var form = new CurationForm(flashpointPath);
                form.SetLogo(Image.FromStream(logoStream));
                form.SetScreenshot(Image.FromStream(ssStream));
                form.SetContent(ZipContentSource.FromPath(openCurationFileDialog.FileName));
                form.LoadCuration(MetaParser.Deserialize<Curation>(metaStream));
                form.ShowDialog();
            }
        }

        private void newCurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CurationForm(flashpointPath).ShowDialog();
        }

        private void profileEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ProfileEditorForm(flashpointPath).ShowDialog();
        }
    }
}
