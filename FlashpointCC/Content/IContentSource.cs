using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator.Content
{
    public interface IContentSource
    {
        void CopyToZip(ZipArchive zip, string parentName);

        void CopyTo(string targetPath);

        TreeNode[] GetTree();
    }
}
