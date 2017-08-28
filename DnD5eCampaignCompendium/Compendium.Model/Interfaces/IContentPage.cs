using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Interfaces
{
    public interface IContentPage<T> where T : IContent
    {
        T SelectedItem { get; set; }

        string Header { get; set; }

        IEnumerable<T> Content { get; }

        void DeserializeContent(string dir, string json);
    }
}
