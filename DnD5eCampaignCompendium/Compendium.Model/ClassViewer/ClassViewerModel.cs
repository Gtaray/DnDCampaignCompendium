using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.ClassViewer
{
    public class ClassViewerModel
    {
        private readonly CompendiumModel Compendium;

        public ClassViewerModel(CompendiumModel compendium)
        {
            Compendium = compendium;
            Compendium.ClassViewer = this;
            _Classes = new ObservableList<CharacterClass>();
        }

        #region Properties and Accessors
        private ObservableList<CharacterClass> _Classes;
        public IEnumerable<CharacterClass> Classes
        {
            get { return _Classes; }
        }
        #endregion

        #region Json Parsing
        public void DeserializeClasses(string dataDir, string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var charClass in obj.classes)
            {
                _Classes.Add(DeserializeSingleClass(dataDir, charClass));
            }

            if (Classes.Count() <= 0)
            {
                throw new ArgumentNullException("No classes were loaded from classes json file. Is the file empty?");
            }
        }

        private CharacterClass DeserializeSingleClass(string dataDir, dynamic json)
        {
            string name = json.name != null ? (string)json.name : "Unknown";
            string id = json.id != null ? (string)json.id : "";
            string source = json.source != null ? (string)json.source : null;
            string filePath = Path.Combine(dataDir, (string)json.file);
            CharacterClass newClass = new CharacterClass()
            {
                Name = name,
                ShortName = json.shortName != null ? json.shortName : "",
                ID = id,
                Source = Compendium.GetSourceByID(source),
                Markdown = ResourceHelper.ReadTextFromFile(filePath),
                ShowInClassList = json.showInClassList != null ? json.showInClassList : true,
                ShowInFilterList = json.showInFilterList != null ? json.showInFilterList : true
            };

            if(json.subclasses != null)
            {
                foreach (var subclass in json.subclasses)
                    newClass.AddSubclass(DeserializeSingleClass(dataDir, subclass));
            }

            return newClass;            
        }

        public void DeserializeClassSpells(string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach(var charClass in obj.classes)
            {
                if (charClass.id == null) continue;

                var cc = Classes.Flatten(c => c.Subclasses).FirstOrDefault(c => string.Equals(c.ID, (string)charClass.id));

                if (cc == null) continue;
                if (charClass.spells == null) continue;

                foreach (var s in charClass.spells)
                {
                    if (s == null) continue;
                    var spell = Compendium.SpellViewer.GetSpellByID((string)s);
                    if (spell == null) continue;
                    cc.AddSpell(spell);
                }
            }
        }
        #endregion
    }
}
