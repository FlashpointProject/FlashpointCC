using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlashpointCurator.MetaParser;

namespace FlashpointCurator
{
    public class Curation
    {
        public static string[] Modes { get
        {
            return new string[]
            {
                "Single Player",
                "Multiplayer",
                "Cooperative"
            };
        } }

        public static string[] Statuses { get
        {
            return new string[]
            {
                "Playable",
                "Playable (Partial)",
                "Playable (Hacked)",
                "Playable (Web Browser)",
                "Playable (Web Browser) (Hacked)",
                "Not Working"
            };
        } }

        public static string[] Genres { get; set; }

        public string Title { get; set; }

        public string Series { get; set; }

        public string Platform { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        [MetaIgnore]
        public DateTime? ReleaseDate { get; set; }

        [MetaElement(ElementName = "Date")]
        public string ReleaseDateString
        {
            get
            {
                if (!ReleaseDate.HasValue)
                {
                    return string.Empty;
                }
                return ReleaseDate.Value.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
            }
            set
            {
                if (DateTime.TryParseExact(value, "MMM dd, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime val))
                {
                    ReleaseDate = val;
                }
            }
        }

        public string Genre { get; set; }

        [MetaElement(ElementName = "Play Mode")]
        public string PlayMode { get; set; }

        public string Extreme { get; set; }

        public string Status { get; set; }

        public string Source { get; set; }

        [MetaElement(ElementName = "Launch Command")]
        public string LaunchCommand { get; set; }

        public string Notes { get; set; }

        [MetaElement(ElementName = "Author Notes")]
        public string AuthorNotes { get; set; }
    }
}
