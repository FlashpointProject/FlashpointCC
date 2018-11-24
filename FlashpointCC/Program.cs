using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Curation.Genres = JsonConvert.DeserializeObject<string[]>(File.ReadAllText("genres.json"));
            if (File.Exists("platforms.json"))
            {
                Platform.Platforms = JsonConvert.DeserializeObject<Platform[]>(File.ReadAllText("platforms.json"));
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Curator());
        }
    }
}
