using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SearchAPI.Search
{
    public class SearchProvider
    {

        public IEnumerable<KeyValuePair<int, JToken>> SearchRecords(string search_term)
        {
            //read Json Files for Weight and Data
            var weight_file = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "weights.json"));
            var weight_records = JObject.Parse(weight_file);

            var file = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "LSM_data.json"));
            var records = JObject.Parse(file);

            //DeSerialization with JTokens
            var SearchResultsDict = new List<KeyValuePair<int, JToken>>();

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

            #region search logic

            foreach (var building in buildings)
            {
                if (building["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(building["name"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Building.Name") + 10), building));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Building.Name"), building));
                    }
                }
                if (building["shortCut"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(building["shortCut"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Building.ShortCut") + 10), building));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Building.ShortCut"), building));
                    }
                }
                if (building["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(building["description"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Building.Description") + 10), building));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Building.Description"), building));
                    }
                }
            }

            foreach (var _lock in locks)
            {
                //transient props
                foreach (var building in buildings)
                {
                    if (building["id"].ToString().Equals(_lock["buildingId"].ToString()))
                    {
                        if (building["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            if (exactMatch(building["name"].ToString(), search_term))
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.BuildingName") + 10), _lock));
                            }
                            else
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.BuildingName"), _lock));
                            }
                        }

                        if (building["shortCut"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            if (exactMatch(building["shortCut"].ToString(), search_term))
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.BuildingShortCut") + 10), _lock));
                            }
                            else
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.BuildingShortCutkk"), _lock));
                            }
                        }
                    }
                }

                //own props
                if (_lock["type"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(_lock["type"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.Type") + 10), _lock));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.Type"), _lock));
                    }
                }
                if (_lock["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(_lock["name"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.Name") + 10), _lock));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.Name"), _lock));
                    }
                }
                if (_lock["serialNumber"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(_lock["serialNumber"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.SerialNumber") + 10), _lock));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.SerialNumber"), _lock));
                    }
                }
                if (_lock["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(_lock["description"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.Description") + 10), _lock));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.Description"), _lock));
                    }
                }
                if (_lock["floor"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(_lock["floor"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.Floor") + 10), _lock));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.Floor"), _lock));
                    }
                }
                if (_lock["roomNumber"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(_lock["roomNumber"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Lock.RoomNumber") + 10), _lock));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Lock.RoomNumber"), _lock));
                    }
                }
            }

            foreach (var group in groups)
            {
                //if (group["id"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                //{
                //    SearchResultsDict.Add(WeightProvider(weights, "Group.Id"), group);
                //}
                if (group["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(group["name"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Group.Name") + 10), group));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Group.Name"), group));
                    }
                }
                if (group["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(group["description"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Group.Description") + 10), group));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Group.Description"), group));
                    }
                }
            }

            foreach (var media in medias)
            {
                //transient props
                foreach (var group in groups)
                {
                    if (group["id"].ToString().Equals(media["groupId"].ToString()))
                    {
                        if (group["name"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            if (exactMatch(group["name"].ToString(), search_term))
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Media.GroupName") + 10), media));
                            }
                            else
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Media.GroupName"), media));
                            }
                        }

                        if (group["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            if (exactMatch(group["description"].ToString(), search_term))
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Media.GroupDescription") + 10), media));
                            }
                            else
                            {
                                SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Media.GroupDescription"), media));
                            }
                        }
                    }
                }

                //own props
                if (media["type"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(media["type"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Media.Type") + 10), media));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Media.Type"), media));
                    }
                }
                if (media["owner"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(media["owner"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Media.Owner") + 10), media));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Media.Owner"), media));
                    }
                }
                if (media["description"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(media["description"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Media.Description") + 10), media));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Media.Description"), media));
                    }
                }
                if (media["serialNumber"].ToString().IndexOf(search_term, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (exactMatch(media["serialNumber"].ToString(), search_term))
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>((WeightProvider(weights, "Media.SerialNumber") + 10), media));
                    }
                    else
                    {
                        SearchResultsDict.Add(new KeyValuePair<int, JToken>(WeightProvider(weights, "Media.SerialNumber"), media));
                    }
                }
            }


            #endregion

            IEnumerable<KeyValuePair<int, JToken>> result1 = deleteDuplicates(SearchResultsDict).OrderByDescending(i => i.Key);

            return result1;

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

        public bool exactMatch(string source, string searchterm)
        {
            return string.Equals(source, searchterm, StringComparison.OrdinalIgnoreCase);
        }

        public List<KeyValuePair<int, JToken>> deleteDuplicates(List<KeyValuePair<int, JToken>> _list)
        {
            var ret = _list;

            for (int i = 0; i < _list.Count; i++)
            {
                for (int j = i + 1; j < _list.Count - 1; j++)
                {
                    if (_list[i].Value["id"].ToString().Equals(_list[j].Value["id"].ToString()))
                    {
                        if (_list[i].Key > _list[j].Key)
                        {
                            ret.Remove(_list[j]);
                        }
                        else
                        {
                            ret.Remove(_list[i]);
                        }
                    }
                }
            }


            return ret;
        }

    }
}
