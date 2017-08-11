using Assisticant.Collections;
using Assisticant.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.CharacterClasses
{
    public class ClassViewerModel
    {
        private readonly CompendiumModel Compendium;

        public ClassViewerModel(CompendiumModel compendium)
        {
            Compendium = compendium;
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
        public void DeserializeClasses(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("Classes.json is null, empty, or is not being read properly");

            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var charClass in obj.classes)
                _Classes.Add(DeserializeSingleClass(charClass));

            if (Classes.Count() <= 0)
            {
                throw new NullReferenceException("No classes were loaded from Classes.json. Is the file empty?");
            }
        }

        private CharacterClass DeserializeSingleClass(dynamic obj)
        {
            CharacterClass newClass = new CharacterClass()
            {
                Name = (string)obj.name,
                SubclassTitle = (string)obj.subclassCategory
            };

            // Parse spell list
            foreach(string id in obj.spells)
            {
                var spell = Compendium.SpellViewer.GetSpellByID(id);
                if (spell != null)
                {
                    newClass.AddSpell(spell);
                }
                else
                {
                    // DISPLAY ERROR MESSAGE OR SOMETHING HERE
                }
            }

            // Parse subclasses
            foreach(var subclass in obj.subclasses)
            {
                newClass.AddSubclass(DeserializeSingleClass(subclass));
            }

            return newClass;
        }
        #endregion
    }
}
