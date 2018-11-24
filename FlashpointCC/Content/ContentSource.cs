using FlashpointCurator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator.Content
{
    public class ContentSource : IContentSource
    {
        public string SourcePath { get; }

        private ContentSource(string path)
        {
            SourcePath = path;
        }

        public static ContentSource FromPath(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Folder does not exist."); 
            }
            return new ContentSource(path);
        }

        public void CopyToZip(ZipArchive zip, string parentName)
        {
            foreach (var file in Directory.EnumerateFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                string entry = file.Substring(SourcePath.Length + 1).Replace('\\', '/');
                zip.CreateEntryFromFile(file, parentName + "/content/" + entry);
            }
        }

        public void CopyTo(string targetPath)
        {
            PathUtil.CopyDirectory(SourcePath, targetPath);
        }

        public TreeNode[] GetTree()
        {
            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(SourcePath);
            var node = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            stack.Push(node);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }
                foreach (var file in directoryInfo.GetFiles())
                    currentNode.Nodes.Add(new TreeNode(file.Name));
            }

            return node.Nodes.Cast<TreeNode>().ToArray();
        }
    }
}
