﻿using Assisticant.Collections;
using Assisticant.Fields;
using Compendium.Model.Common;
using Compendium.Model.Helpers;
using Compendium.Model.SpellViewer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.CharacterClasses
{
    public class CharacterClass : BaseModel
    {
        public CharacterClass()
        {
            _Spells = new ObservableList<Spell>();
            _Subclasses = new ObservableList<CharacterClass>();
        }

        public CharacterClass(string name, ContentSource source, string markdown)
        {
            _Spells = new ObservableList<Spell>();
            _Subclasses = new ObservableList<CharacterClass>();

            Name = name;
            Source = source;
            Markdown = markdown;
        }

        #region Properties and Accessors
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

        private Observable<bool> _FilterOnly = new Observable<bool>(false);
        public bool FilterOnly
        {
            get { return _FilterOnly; }
            set { _FilterOnly.Value = value; }
        }

        private Observable<string> _Markdown = new Observable<string>("");
        public string Markdown
        {
            get { return _Markdown; }
            set { _Markdown.Value = value; }
        }

        private ObservableList<CharacterClass> _Subclasses = new ObservableList<CharacterClass>();
        public IEnumerable<CharacterClass> Subclasses => _Subclasses;

        private ObservableList<Spell> _Spells = new ObservableList<Spell>();
        public IEnumerable<Spell> Spells => _Spells;

        private Observable<CharacterClass> _Parent = new Observable<CharacterClass>(default(CharacterClass));
        public CharacterClass Parent
        {
            get { return _Parent; }
            set { _Parent.Value = value; }
        }
        #endregion

        #region Computed Properties
        private Computed<bool> _HasSpells;
        [JsonIgnore]
        public bool HasSpells
        {
            get
            {
                if (_HasSpells == null)
                    _HasSpells = new Computed<bool>(() => _Spells.Count() > 0 || Subclasses.Any(s => s.HasSpells));
                return _HasSpells.Value;
            }
        }

        private Computed<bool> _IsRoot;
        [JsonIgnore]
        public bool IsRoot
        {
            get
            {
                if (_IsRoot == null)
                    _IsRoot = new Computed<bool>(() => Parent == null);
                return _IsRoot.Value;
            }
        }

        private Computed<string> _Header;
        [JsonIgnore]
        public string Header
        {
            get
            {
                if (_Header == null)
                    _Header = new Computed<string>(() => IsRoot ? Name : Parent.Header + " > " + Name);
                return _Header.Value;
            }
        }

        private Computed<string> _Path;
        [JsonIgnore]
        public string Path
        {
            get
            {
                if (_Path == null)
                    _Path = new Computed<string>(() => IsRoot ? Name : Parent.Path + "." + Path);
                return _Path.Value;
            }
        }

        [JsonProperty(PropertyName = "spells")]
        public IEnumerable<string> SpellIDs
        {
            get { return _Spells.Select(s => s.ID); }
        }
        #endregion

        #region Add Remove Functions
        public void AddSpell(Spell spell)
        {
            if (Spells.Contains(spell))
                return;

            _Spells.Add(spell);
            spell.AddClass(this);
        }

        public void RemoveSpell(Spell toRemove)
        {
            if (!Spells.Contains(toRemove))
                return;

            _Spells.Remove(toRemove);
            toRemove.RemoveClass(this);
        }

        public void AddSubclass(CharacterClass newClass)
        {
            newClass.Parent = this;
            _Subclasses.Add(newClass);
        }

        public void RemoveSubclass(CharacterClass toRemove)
        {
            toRemove.Parent = null;
            _Subclasses.Remove(toRemove);
        }
        #endregion

        #region Misc Functions
        
        #endregion

        public override string ToString()
        {
            return Header;
        }
    }
}
