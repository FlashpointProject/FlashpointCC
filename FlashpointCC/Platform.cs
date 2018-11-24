using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashpointCurator
{
    public class Platform
    {
        public static Platform[] Platforms { get; set; }

        public string Name { get; set; }

        public string ApplicationPath { get; set; }

        public string DestinationPath { get; set; }

        public string CommandLine { get; set; }

        public bool IsConfigured()
        {
            return ApplicationPath != null && DestinationPath != null && CommandLine != null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
