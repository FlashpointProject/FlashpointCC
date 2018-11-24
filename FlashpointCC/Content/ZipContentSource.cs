using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator.Content
{
    public class ZipContentSource : IContentSource
    {
        public string SourcePath { get; }
        public Dictionary<string, string> Entries { get; }

        private ZipContentSource(string path, Dictionary<string, string> entries)
        {
            SourcePath = path;
            Entries = entries;
        }

        public static IContentSource FromPath(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File does not exist.");
            }
            Dictionary<string, string> entries;
            using (var zip = ZipFile.OpenRead(path))
            {
                entries =
                    (from entry in zip.Entries
                     let match = Regex.Match(entry.FullName, @"(?i)(.*\/)?content\/(.*)")
                     where match.Success
                     select match).ToDictionary(x => x.Value, x => x.Groups[2].Value);
            }
            return new ZipContentSource(path, entries);
        }

        public void CopyToZip(ZipArchive zip, string parentName)
        {
            using (var contentZip = ZipFile.OpenRead(SourcePath))
            {
                foreach (var entry in 
                    from entry in contentZip.Entries
                    let path = entry.FullName
                    where Entries.ContainsKey(path)
                    select new { Entry = entry, Name = path.Substring(path.Length - Entries[path].Length) })
                {
                    using (var from = entry.Entry.Open())
                    using (var to = zip.CreateEntry(parentName + "/content/" + entry.Name).Open())
                    {
                        from.CopyTo(to);
                    }
                }
            }
        }

        // TODO: Refactor this
        public void CopyTo(string targetPath)
        {
            using (var contentZip = ZipFile.OpenRead(SourcePath))
            {
                foreach (var entry in
                    from entry in contentZip.Entries
                    let path = entry.FullName
                    where Entries.ContainsKey(path)
                    select new { Entry = entry, Name = path.Substring(path.Length - Entries[path].Length) })
                {
                    if (entry.Name == string.Empty || entry.Name.EndsWith("/"))
                    {
                        continue;
                    }
                    var file = Path.GetFileName(entry.Name);
                    var destFolder = Path.Combine(targetPath, Path.GetDirectoryName(entry.Name));
                    var destPath = Path.Combine(destFolder, file);
                    Directory.CreateDirectory(destFolder);
                    using (var from = entry.Entry.Open())
                    using (var to = File.Open(destPath, FileMode.CreateNew))
                    {
                        from.CopyTo(to);
                    }
                }
            }
        }

        public TreeNode[] GetTree()
        {
            var zipNode = new TreeNode();
            foreach (string path in Entries.Values)
            {
                var split = path.Split(new char[] { '/' });
                if (split[split.Length - 1] == string.Empty)
                {
                    continue;
                }
                var lastNode = zipNode;
                for (int i = 0; i < split.Length; i++)
                {
                    string entry = split[i];
                    var currentNode = new TreeNode(entry) { Name = entry };
                    var find = lastNode.Nodes.Find(entry, false).FirstOrDefault();
                    if (find != null)
                    {
                        lastNode = find;
                        continue;
                    }
                    lastNode.Nodes.Add(currentNode);
                    lastNode = currentNode;
                }
            }
            return zipNode.Nodes.Cast<TreeNode>().ToArray();
        }
    }
}
