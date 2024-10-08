using UnityEngine;
using System.IO;
using System.Collections;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryManager : DataManager<FactoryManager, FactoryData>
    {
        [Header("Factory")]
        public SecretKey steamKey;
        public SecretKey serverAddress;
        public int hoursToCache = 1;

        private static ulong STEAM_ID = 1990050;


        public List<string> LoadedWorkshopCreatures { get; } = new();

        public bool IsDownloadingItem { get; private set; }
        public bool IsDownloadingUsername { get; private set; }


        protected override void Start()
        {
            base.Start();
            LoadWorkshopCreatures();
        }

        public void ViewWorkshop()
        {
#if UNITY_STANDALONE
            SteamFriends.ActivateGameOverlayToWebPage($"steam://url/SteamWorkshopPage/{STEAM_ID}");
#else
            string url = $"https://steamcommunity.com/app/{STEAM_ID}/workshop/";
            Application.OpenURL(url);
#endif
        }

        public void ViewWorkshopItem(ulong id)
        {
#if UNITY_STANDALONE
            SteamFriends.ActivateGameOverlayToWebPage($"steam://url/CommunityFilePage/{id}");
#else
            string url = $"https://steamcommunity.com/sharedfiles/filedetails/?id={id}";
            Application.OpenURL(url);
#endif
        }


        public void LikeItem(ulong id)
        {
#if UNITY_STANDALONE
            PublishedFileId_t fileId = new(id);
            SteamUGC.SetUserItemVote(fileId, true);
#endif

            if (!Data.LikedItems.Contains(id))
            {
                Data.LikedItems.Add(id);
            }
        }

        public void DislikeItem(ulong id)
        {
#if UNITY_STANDALONE
            PublishedFileId_t fileId = new(id);
            SteamUGC.SetUserItemVote(fileId, false);
#endif

            if (!Data.DislikedItems.Contains(id))
            {
                Data.DislikedItems.Add(id);
            }
        }

        public void SubscribeItem(ulong id)
        {
#if UNITY_STANDALONE
            PublishedFileId_t fileId = new(id);
            SteamUGC.SubscribeItem(fileId);
#endif

            if (!Data.SubscribedItems.Contains(id))
            {
                Data.SubscribedItems.Add(id);
            }
        }

        public void UnsubscribeItem(ulong id)
        {
#if UNITY_STANDALONE
            PublishedFileId_t fileId = new(id);
            SteamUGC.UnsubscribeItem(fileId);
#endif

            if (Data.SubscribedItems.Contains(id))
            {
                Data.SubscribedItems.Remove(id);
            }
        }


        public void GetItems(FactoryItemQuery itemQuery, Action<List<FactoryItem>, uint> onLoaded, Action<string> onFailed)
        {
            if (!string.IsNullOrEmpty(itemQuery.SearchText))
            {
                itemQuery.SortByType = FactorySortByType.SearchText;
            }

            if (WorldTimeManager.Instance.IsInitialized)
            {
                var now = WorldTimeManager.Instance.UtcNow.Value;
                if (Data.CachedItems.TryGetValue(itemQuery, out FactoryData.CachedItemData data))
                {
                    var time = new DateTime(data.Ticks);

                    TimeSpan diff = now - time;
                    if (diff.Hours > hoursToCache)
                    {
                        Data.CachedItems.Remove(itemQuery);
                    }
                    else
                    {
                        onLoaded(data.Items, data.Total);
                        return;
                    }
                }
            }


            uint days = 0;
            switch (itemQuery.TimePeriodType)
            {
                case FactoryTimePeriodType.Today:
                    days = 1;
                    break;

                case FactoryTimePeriodType.ThisWeek:
                    days = 7;
                    break;

                case FactoryTimePeriodType.ThisMonth:
                    days = 30;
                    break;

                case FactoryTimePeriodType.ThisYear:
                    days = 365;
                    break;

                case FactoryTimePeriodType.AllTime:
                    days = 9999999;
                    break;
            }

#if UNITY_STANDALONE

            EUGCQuery sortBy = default;
            switch (itemQuery.SortByType)
            {
                case FactorySortByType.MostPopular:
                    sortBy = EUGCQuery.k_EUGCQuery_RankedByTrend;
                    break;

                case FactorySortByType.MostSubscribed:
                    sortBy = EUGCQuery.k_EUGCQuery_RankedByTotalUniqueSubscriptions;
                    break;

                case FactorySortByType.MostRecent:
                    sortBy = EUGCQuery.k_EUGCQuery_RankedByPublicationDate;
                    break;

                case FactorySortByType.LastUpdated:
                    sortBy = EUGCQuery.k_EUGCQuery_RankedByLastUpdatedDate;
                    break;

                case FactorySortByType.SearchText:
                    sortBy = EUGCQuery.k_EUGCQuery_RankedByTextSearch;
                    break;
            }

            CallResult<SteamUGCQueryCompleted_t> query = CallResult<SteamUGCQueryCompleted_t>.Create(delegate (SteamUGCQueryCompleted_t param, bool hasFailed)
            {
                if (hasFailed)
                {
                    onFailed?.Invoke(null);
                    return;
                }

                List<FactoryItem> items = new();
                for (uint i = 0; i < param.m_unNumResultsReturned; i++)
                {
                    FactoryItem item = new()
                    {
                        tag = itemQuery.TagType
                    };
                    if (SteamUGC.GetQueryUGCResult(param.m_handle, i, out SteamUGCDetails_t details))
                    {
                        item.id = details.m_nPublishedFileId.m_PublishedFileId;
                        item.name = details.m_rgchTitle;
                        item.description = details.m_rgchDescription;
                        item.upVotes = details.m_unVotesUp;
                        item.timeCreated = details.m_rtimeCreated;
                        item.creatorId = details.m_ulSteamIDOwner;
                    }
                    if (SteamUGC.GetQueryUGCPreviewURL(param.m_handle, i, out string url, 256))
                    {
                        item.previewURL = url;
                    }
                    items.Add(item);
                }

                uint total = param.m_unTotalMatchingResults;

                onLoaded?.Invoke(items, total);

                CacheItems(itemQuery, items, total);
            });

            UGCQueryHandle_t handle = SteamUGC.CreateQueryAllUGCRequest(sortBy, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, SteamUtils.GetAppID(), SteamUtils.GetAppID(), (uint)(itemQuery.Page + 1));
            SteamUGC.SetRankedByTrendDays(handle, days);
            SteamUGC.SetReturnLongDescription(handle, true);
            SteamUGC.SetSearchText(handle, itemQuery.SearchText);
            SteamUGC.SetMatchAnyTag(handle, true); // TODO: Tags

            SteamAPICall_t call = SteamUGC.SendQueryUGCRequest(handle);
            query.Set(call);
#else

            string sortBy = default;
            switch (itemQuery.SortByType)
            {
                case FactorySortByType.MostPopular:
                    sortBy = "3";
                    break;

                case FactorySortByType.MostSubscribed:
                    sortBy = "9";
                    break;

                case FactorySortByType.MostRecent:
                    sortBy = "1";
                    break;

                case FactorySortByType.LastUpdated:
                    sortBy = "21";
                    break;

                case FactorySortByType.SearchText:
                    sortBy = "12";
                    break;
            }

            string url = $"https://api.steampowered.com/IPublishedFileService/QueryFiles/v1/?key={steamKey.Value}&appid={STEAM_ID}&query_type={sortBy}&search_text={itemQuery.SearchText}&days={days}&numperpage={itemQuery.NumPerPage}&page={itemQuery.Page+1}&return_vote_data=true&return_previews=true";
            StartCoroutine(GetItemsRoutine(url, itemQuery, onLoaded, onFailed));
#endif
        }

        private IEnumerator GetItemsRoutine(string url, FactoryItemQuery query, Action<List<FactoryItem>, uint> onLoaded, Action<string> onFailed)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                List<FactoryItem> items = new();

                JObject data = JToken.Parse(request.downloadHandler.text).First.First as JObject;
                uint total = data["total"].Value<uint>();

                if (total > 0)
                {
                    JArray files = data["publishedfiledetails"] as JArray;
                    foreach (JObject file in files)
                    {
                        string title = file["title"].Value<string>();
                        string description = file["file_description"].Value<string>();
                        ulong id = file["publishedfileid"].Value<ulong>();
                        ulong creatorId = file["creator"].Value<ulong>();
                        uint upVotes = file["vote_data"]["votes_up"].Value<uint>();
                        uint downVotes = file["vote_data"]["votes_down"].Value<uint>();
                        string previewURL = file["preview_url"].Value<string>();
                        uint timeCreated = file["time_created"].Value<uint>();

                        FactoryItem item = new()
                        {
                            id = id,
                            name = title,
                            creatorId = creatorId,
                            description = description,
                            upVotes = upVotes,
                            downVotes = downVotes,
                            previewURL = previewURL,
                            timeCreated = timeCreated
                        };
                        items.Add(item);
                    }
                }

                onLoaded?.Invoke(items, total);

                CacheItems(query, items, total);
            }
            else
            {
                onFailed?.Invoke(request.error);
            }
        }


        public void DownloadItem(ulong itemId, Action<string> onDownloaded, Action<string> onFailed)
        {
            if (!IsDownloadingItem)
            {
                StartCoroutine(DownloadItemRoutine(itemId, onDownloaded, onFailed));
            }
        }

        private IEnumerator DownloadItemRoutine(ulong itemId, Action<string> onDownloaded, Action<string> onFailed)
        {
            IsDownloadingItem = true;

            string url = $"http://{serverAddress.Value}/get_workshop_item?id={itemId}";

            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                JObject obj = JToken.Parse(request.downloadHandler.text) as JObject;

                string name = obj["name"].Value<string>();
                string data = obj["data"].Value<string>();

                string creaturesDir = Path.Combine(Application.persistentDataPath, "creature");
                if (!Directory.Exists(creaturesDir))
                {
                    Directory.CreateDirectory(creaturesDir);
                }

                string creaturePath = Path.Combine(creaturesDir, $"{name}.dat");
                File.WriteAllText(creaturePath, data);

                onDownloaded?.Invoke(name);
            }
            else
            {
                onFailed?.Invoke(request.error);
            }

            IsDownloadingItem = false;
        }

        private void CacheItems(FactoryItemQuery query, List<FactoryItem> items, uint total)
        {
            if (!Data.CachedItems.ContainsKey(query))
            {
                Data.CachedItems.Add(query, new FactoryData.CachedItemData()
                {
                    Items = items,
                    Total = total
                });
                Save();
            }
        }


        public void DownloadUsername(ulong userId, Action<string> onLoaded, Action<string> onFailed)
        {
            if (!IsDownloadingUsername)
            {
                StartCoroutine(DownloadUsernameRoutine(userId, onLoaded, onFailed));
            }
        }

        private IEnumerator DownloadUsernameRoutine(ulong userId, Action<string> onLoaded, Action<string> onFailed)
        {
            IsDownloadingUsername = true;

            string url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={steamKey.Value}&steamids={userId}";

            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                JObject data = JToken.Parse(request.downloadHandler.text).First.First as JObject;

                JArray players = data["players"] as JArray;
                if (players.Count > 0)
                {
                    var player = players.First;

                    string username = player["personaname"].Value<string>();

                    onLoaded?.Invoke(username);

                    if (!Data.CachedUsernames.ContainsKey(userId))
                    {
                        Data.CachedUsernames.Add(userId, username);
                        Save();
                    }
                }
            }
            else
            {
                onFailed?.Invoke(request.error);
            }

            IsDownloadingUsername = false;
        }


        public void LoadWorkshopCreatures()
        {
#if UNITY_STANDALONE
            LoadedWorkshopCreatures.Clear();

            uint n = SteamUGC.GetNumSubscribedItems();
            if (n > 0)
            {
                string creaturesDir = Path.Combine(Application.persistentDataPath, "creature");
                if (!Directory.Exists(creaturesDir))
                {
                    Directory.CreateDirectory(creaturesDir);
                }

                PublishedFileId_t[] items = new PublishedFileId_t[n];
                SteamUGC.GetSubscribedItems(items, n);

                foreach (PublishedFileId_t fileId in items)
                {
                    if (SteamUGC.GetItemInstallInfo(fileId, out ulong sizeOnDisk, out string folder, 1024, out uint timeStamp) && Directory.Exists(folder))
                    {
                        string src = Directory.GetFiles(folder)[0];
                        string dst = Path.Combine(creaturesDir, Path.GetFileName(src));
                        if (!File.Exists(dst))
                        {
                            File.Copy(src, dst);
                        }

                        string name = Path.GetFileNameWithoutExtension(src);
                        LoadedWorkshopCreatures.Add(name);
                    }
                }
            }
#endif
        }
    }
}