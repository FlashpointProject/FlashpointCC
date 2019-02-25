using System;
using System.Xml.Serialization;

namespace FlashpointCurator
{
    [XmlType("Game")]
    public class Game
    {
        public string ApplicationPath { get; set; }

        public string CommandLine { get; set; }

        public string ConfigurationCommandLine { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string Developer { get; set; }

        [XmlElement(ElementName = "ID")]
        public Guid Id { get; set; }

        public string MusicPath { get; set; }

        public string Notes { get; set; }

        public string Platform { get; set; }

        public string Publisher { get; set; }

        public string RootFolder { get; set; }

        public string SortTitle { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public string Series { get; set; }

        public string PlayMode { get; set; }

        public string Region { get; set; }

        public int PlayCount { get; set; }

        public string VideoPath { get; set; }

        public bool Hide { get; set; }

        public bool Broken { get; set; }

        public string Genre { get; set; }
    }
}
