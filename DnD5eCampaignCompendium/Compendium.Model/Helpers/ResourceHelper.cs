using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Helpers
{
    public class ResourceHelper
    {
        public static string ReadEmbeddedResourceContent(string location)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(location))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        // TODO:
        // Change to raise popup event instead of returning null
        public static string ReadTextFromFile(string file)
        {
            if (!File.Exists(file))
                return null;

            return File.ReadAllText(file);
        }
    }
}
