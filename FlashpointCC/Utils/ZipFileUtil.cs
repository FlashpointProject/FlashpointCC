using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashpointCurator.Utils
{
    public static class ZipFileUtil
    {
        public static ZipArchiveEntry Find(this ZipArchive zip, string name)
        {
            return (from entry in zip.Entries where string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase)
                    orderby entry.FullName.Length select entry).FirstOrDefault();
        }

        public static bool TryFind(this ZipArchive zip, string name, out ZipArchiveEntry entry)
        {
            entry = zip.Find(name);
            return entry != null;
        }
    }
}
