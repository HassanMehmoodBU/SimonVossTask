using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace SearchAPI.Search
{
    public class SearchProvider
    {

        public SortedDictionary<int, JToken> SearchRecords(string search_term)
        {
            var weight_file = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "weights.json"));
            var weight_records = JObject.Parse(weight_file);

            var file = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "LSM_data.json"));
            var records = JObject.Parse(file);

            //List<JToken> SearchResults = new List<JToken>();
            var SearchResultsDict = new Dictionary<int, JToken>();


            var buildingsjson = records["buildings"];
            var locksjson = records["locks"];
            var groupsjson = records["groups"];
            var mediajson = records["media"];

            var weightsjson = weight_records["Weights"];

            List<JToken> buildings = buildingsjson.Children().ToList();
            List<JToken> locks = locksjson.Children().ToList();
            List<JToken> groups = groupsjson.Children().ToList();
            List<JToken> medias = mediajson.Children().ToList();

            List<JToken> weights = weightsjson.Children().ToList();

            foreach (var building in buildings)
            {
                if (building["id"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Building.Id"), building);
                }
                //if (building["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                //{
                //    SearchResultsDict.Add(WeightProvider(weights, "Building.Name"), building);
                //}
                if (building["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Buiklding.Name"), building);
                }
                if (building["shortCut"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Building.ShortCut"), building);
                }
                if (building["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Building.Description"), building);
                }
            }

            foreach (var _lock in locks)
            {
                if (_lock["id"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.Id"), _lock);
                }
                if (_lock["buildingId"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.BuildingId"), _lock);
                }
                if (_lock["type"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.Type"), _lock);
                }
                if (_lock["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.Name"), _lock);
                }
                if (_lock["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.SerialNumber"), _lock);
                }
                if (_lock["serialNumber"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.Floor"), _lock);
                }
                if (_lock["floor"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.RoomNumber"), _lock);
                }
                if (_lock["roomNumber"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Lock.Description"), _lock);
                }
            }

            foreach (var group in groups)
            {
                if (group["id"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Group.Id"), group);
                }
                if (group["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Group.Name"), group);
                }
                if (group["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Group.Description"), group);
                }
            }

            foreach (var media in medias)
            {
                if (media["id"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Media.Id"), media);
                }
                if (media["groupId"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Media.GroupId"), media);
                }
                if (media["type"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Media.Type"), media);
                }
                if (media["owner"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Media.Owner"), media);
                }
                if (media["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Media.Description"), media);
                }
                if (media["serialNumber"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SearchResultsDict.Add(WeightProvider(weights, "Media.SerialNumber"), media);
                }
            }

            //SearchResults = SearchResults.Distinct().ToList();


            SortedDictionary<int, JToken> sortedResult = new SortedDictionary<int, JToken>(SearchResultsDict);
            //var ret = dupeLists.ToDictionary(c => c.Key, c => c.Value.Distinct().ToList());

            var ret = SearchResultsDict.OrderByDescending(k => k.Key).ToList();

            return sortedResult;

        }

        public int WeightProvider(List<JToken> weights, string request)
        {
            foreach (var weight in weights)
            {
                JProperty parentProp = (JProperty)weight;
                string name = parentProp.Name;
                if (name == request)
                {
                    return Convert.ToInt32(weight.Last.ToString());
                }
            }
            return 0;
        }


    }
}
