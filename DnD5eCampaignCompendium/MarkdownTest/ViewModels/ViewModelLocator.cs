using Assisticant;
using Markdig;
using MarkdownTest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTest.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    { 
        private const string MARKDOWN = "MarkdownTest.Markdown.Druid.md";
        private MarkdownModel _Model;

        public object MainViewModel => ViewModel(() => new MainViewModel(_Model));

        public ViewModelLocator()
        {
            _Model = new MarkdownModel();

            if (DesignMode)
            {

            }
            else
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(MARKDOWN))
                using (StreamReader reader = new StreamReader(stream))
                {
                    _Model.Markdown = reader.ReadToEnd();
                }
            }
        }
    }
}
