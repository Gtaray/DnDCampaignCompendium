using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Interfaces;

namespace Compendium.Model.Models
{
    public class ContentItemModel : IContent
    {
        public ContentItemModel()
        { }

        private Observable<string> _Name = new Observable<string>(default(string));
        public string Name
        {
            get { return _Name; }
            set { _Name.Value = value; }
        }

        private Observable<string> _ID = new Observable<string>(default(string));
        public string ID
        {
            get { return _ID; }
            set { _ID.Value = value; }
        }

        private Observable<ContentSource> _Source = new Observable<ContentSource>(default(ContentSource));
        public ContentSource Source
        {
            get { return _Source; }
            set { _Source.Value = value; }
        }

        private Observable<string> _Markdown = new Observable<string>("");
        public string Markdown
        {
            get { return _Markdown; }
            set { _Markdown.Value = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Source?.ToString());
        }
    }
}
