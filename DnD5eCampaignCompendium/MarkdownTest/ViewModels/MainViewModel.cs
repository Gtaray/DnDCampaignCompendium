using Assisticant.Fields;
using MarkdownTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTest.ViewModels
{
    public class MainViewModel
    {
        private readonly MarkdownModel _Model;

        public MainViewModel(MarkdownModel model)
        {
            _Model = model;
        }

        public string Markdown => _Model.Markdown;
    }
}
