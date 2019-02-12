using System.Linq;

namespace FlashpointCurator
{
    public class Profile
    {
        public string Name { get; set; }

        public string Platform { get; set; }

        public string ApplicationPath { get; set; }

        public string DestinationPath { get; set; }

        public string CommandLine { get; set; }

        public bool Validate()
        {
            return !GetType().GetProperties()
                .Select(p => (string)p.GetValue(this))
                .Any(value => string.IsNullOrEmpty(value));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
