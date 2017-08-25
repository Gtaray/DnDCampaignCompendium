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

namespace Compendium.Model.CharacterClasses
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
        public void DeserializeClasses(string dataDir, string classesIndex)
        {
            dynamic obj = JsonConvert.DeserializeObject(classesIndex);

            if (obj == null)
                throw new ArgumentNullException(classesIndex + " is null, empty, or is not being read properly");

            foreach (var charClass in obj.classes)
            {
                _Classes.Add(DeserializeSingleClass(dataDir, charClass));
            }

            if (Classes.Count() <= 0)
            {
                throw new ArgumentNullException("No classes were loaded from Classes.json. Is the file empty?");
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
                ID = id,
                Source = Compendium.GetSourceByID(source),
                Markdown = ResourceHelper.ReadTextFromFile(filePath),
                FilterOnly = json.filterOnly != null ? json.filterOnly : false
            };

            if(json.subclasses != null)
            {
                foreach (var subclass in json.subclasses)
                    newClass.AddSubclass(DeserializeSingleClass(dataDir, subclass));
            }

            return newClass;            
        }
        #endregion
    }
}
