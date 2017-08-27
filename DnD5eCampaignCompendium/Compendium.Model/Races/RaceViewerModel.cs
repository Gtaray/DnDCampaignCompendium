using Assisticant.Collections;
using Compendium.Model.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compendium.Model.Races
{
    public class RaceViewerModel
    {
        private readonly CompendiumModel Compendium;

        public RaceViewerModel(CompendiumModel compendium)
        {
            Compendium = compendium;
            Compendium.RaceViewer = this;
            _Races = new ObservableList<Race>();
        }

        #region Properties and Accessors
        private ObservableList<Race> _Races;
        public IEnumerable<Race> Races => _Races;
        #endregion

        #region Json Parsing
        public void DeserializeRaces(string dataDir, string json)
        {
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var race in obj.races)
            {
                _Races.Add(new Race()
                {
                    Name = race.name != null ? (string)race.name : "Unknown",
                    ID = race.id != null ? (string)race.id : "",
                    Source = race.source != null ? Compendium.GetSourceByID((string)race.source) : null,
                    Markdown = ResourceHelper.ReadTextFromFile(Path.Combine(dataDir, (string)race.file))
                });
            }

            if (Races.Count() <= 0)
            {
                throw new ArgumentNullException("No races were loaded from races json file. Is the file empty?");
            }
        }
        #endregion
    }
}
