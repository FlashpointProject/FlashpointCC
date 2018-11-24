using System;
using System.Xml.Serialization;

namespace FlashpointCurator
{
    [XmlType("Game")]
    public class Game
    {
        public string ApplicationPath { get; set; }

        public string CommandLine { get; set; }

        [Column(true)]
        public string ConfigurationCommandLine { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string Developer { get; set; }

        [XmlElement(ElementName = "ID")]
        public Guid Id { get; set; }

        [Column(true)]
        public string MusicPath { get; set; }

        public string Notes { get; set; }

        public string Platform { get; set; }

        public string Publisher { get; set; }

        public string RootFolder { get; set; }

        [Column(true)]
        public string SortTitle { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public string Series { get; set; }

        public string PlayMode { get; set; }

        [Column(true)]
        public string Region { get; set; }

        public int PlayCount { get; set; }

        [Column(true)]
        public string VideoPath { get; set; }

        public bool Hide { get; set; }

        [Column(true)]
        public bool Broken { get; set; }

        public string Genre { get; set; }
    }
}
