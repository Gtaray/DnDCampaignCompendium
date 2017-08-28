using Assisticant.Fields;
using Compendium.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Interfaces
{
    public interface IContent
    {
        string Name { get; set; }

        string ID { get; set; }

        ContentSource Source { get; set; }

        string Markdown { get; set; }
    }
}
