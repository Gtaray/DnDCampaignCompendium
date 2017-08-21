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
                HD = (string)obj.hd,
                ShortName = obj.displayName != null ? (string)obj.displayName : "",
                ArmorProfs = obj.armorProf != null ? (string)obj.armorProf : "",
                WeaponProfs = obj.weaponProf != null ? (string)obj.weaponProf : "",
                ToolProfs = obj.toolProf != null ? (string)obj.toolProf : "",
                SaveProfs = obj.saveProf != null ? (string)obj.saveProf : "",
                SkillProfs = obj.skillProf != null ? (string)obj.skillProf : "",
                SubclassTitle = obj.subclassCategory != null ? (string)obj.subclassCategory : ""
            };

            if(obj.description != null)
            {
                foreach (var line in obj.description)
                    newClass.AddDescriptionLine((string)line);
            }

            if(obj.startingGear != null)
            {
                foreach (var gear in obj.startingGear)
                    newClass.AddStartingGear((string)gear);
            }

            if(obj.features != null)
            {
                foreach (var feature in obj.features)
                {
                    ClassFeature newFeature = new ClassFeature()
                    {
                        Name = feature.name != null ? (string)feature.name : "",
                        Level = feature.level != null ? (int)feature.level : 1,
                        TableName = feature.tableName != null ? (string)feature.tableName : "",
                        TableOnly = feature.tableOnly != null ? (bool)feature.tableOnly : true
                    };

                    if (obj.description != null)
                    {
                        foreach (var line in obj.description)
                            newFeature.AddLineToDescription((string)line);
                    }

                    newClass.AddFeature(newFeature);
                }
            }

            if(obj.featureColumns != null)
            {
                foreach(var column in obj.featureColumns)
                {
                    ClassTableColumn ctc = new ClassTableColumn()
                    {
                        ColumnHeader = column.columnHeader != null ? (string)column.columnHeader : ""
                    };
                    if(obj.values != null)
                    {
                        foreach (var line in obj.values)
                            ctc.AddColumnValue((string)line);
                    }

                    newClass.AddTableColumn(ctc);
                }
            }

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
