using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;
using System.IO;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class PremiumManager : DataManager<PremiumManager, Premium>
    {
        #region Fields
        [SerializeField] private string iOSGameId;
        [SerializeField] private string androidGameId;
        [SerializeField] private bool testMode;

        private bool wasPrevPurchased;
        private float currVolume;
        #endregion

        #region Properties
        private string BannerAdUnitId
        {
            get
            {
                string id = "Banner_";
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    id += "iOS";
                }
                else
                {
                    id += "Android";
                }
                return id;
            }
        }
        private string RewardAdUnitId
        {
            get
            {
                string id = "Rewarded_";
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    id += "iOS";
                }
                else
                {
                    id += "Android";
                }
                return id;
            }
        }
        private string GameId
        {
            get
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return iOSGameId;
                }
                else
                {
                    return androidGameId;
                }
            }
        }

        public RewardedItem RequestedItem
        {
            get;
            set;
        }

#if UNITY_STANDALONE
        public override string SALT => SteamUser.GetSteamID().ToString();
#elif UNITY_IOS || UNITY_ANDROID
        public override string SALT => SystemInfo.deviceUniqueIdentifier;
#endif
        #endregion

        #region Methods
        protected override void Awake()
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, "settings.dat")) && !File.Exists(Path.Combine(Application.persistentDataPath, "premium.dat")))
            {
                wasPrevPurchased = true;
            }
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            if (wasPrevPurchased)
            {
                Data.IsPremium = true;
                Save();
            }

            //Advertisement.Initialize(GameId, testMode, this);
        }

        public void ShowBanner()
        {
            //Advertisement.Show(BannerAdUnitId, this);
        }
        public void ShowReward()
        {
            //Advertisement.Show(RewardAdUnitId, this);
        }
        public void HideBanner()
        {
            //Advertisement.Banner.Hide();
        }

        public void Access(RewardedItem item)
        {
            switch (item.Type)
            {
                case ItemType.BodyPart:
                    Data.UsableBodyParts.Add(item.Id, true);
                    RewardsMenu.Instance.AddReward(DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", item.Id).Icon);
                    break;

                case ItemType.Pattern:
                    Data.UsablePatterns.Add(item.Id, true);
                    RewardsMenu.Instance.AddReward(DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", item.Id).Icon);
                    break;
            }
            Save();
        }
        public void AccessRandom(int count)
        {
            List<RewardedItem> items = new List<RewardedItem>();

            foreach (var kv in DatabaseManager.GetDatabase("Body Parts").Objects)
            {
                BodyPart bodyPart = kv.Value as BodyPart;
                if (bodyPart.Premium && !Data.UsableBodyParts.ContainsKey(kv.Key))
                {
                    items.Add(new RewardedItem(ItemType.BodyPart, kv.Key));
                }
            }

            foreach (var kv in DatabaseManager.GetDatabase("Patterns").Objects)
            {
                Pattern pattern = kv.Value as Pattern;
                if (pattern.Premium && !Data.UsablePatterns.ContainsKey(kv.Key))
                {
                    items.Add(new RewardedItem(ItemType.Pattern, kv.Key));
                }
            }

            items.Shuffle();

            for (int i = 0; i < count && i < items.Count; i++)
            {
                Access(items[i]);
            }
        }

        public bool IsBodyPartUsable(string bodyPartId)
        {
            if (Data.IsPremium) return true;

            if (DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartId).Premium)
            {
                return Data.UsableBodyParts.ContainsKey(bodyPartId);
            }
            else
            {
                return true;
            }
        }
        public bool IsPatternUsable(string patternId)
        {
            if (Data.IsPremium) return true;

            if (DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", patternId).Premium)
            {
                return Data.UsablePatterns.ContainsKey(patternId);
            }
            else
            {
                return true;
            }
        }

        //public void OnInitializationComplete()
        //{
        //    Advertisement.Load(BannerAdUnitId, this);
        //    Advertisement.Load(RewardAdUnitId, this);
        //}
        //public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        //{
        //}
        //public void OnUnityAdsAdLoaded(string placementId)
        //{
        //}
        //public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        //{
        //}
        //public void OnUnityAdsShowClick(string placementId)
        //{
        //}
        //public void OnUnityAdsShowStart(string placementId)
        //{
        //    currVolume = SettingsManager.Data.MasterVolume;
        //    SettingsManager.Instance.SetMasterVolume(0f);
        //}
        //public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        //{
        //    if (placementId == RewardAdUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        //    {
        //        RewardsMenu.Instance.ClearRewards();
        //        if (RequestedItem != null)
        //        {
        //            Access(RequestedItem);
        //            AccessRandom(3);
        //        }
        //        else
        //        {
        //            AccessRandom(4);
        //        }

        //        PremiumDialog.Instance.Close(true);
        //        RewardsMenu.Instance.Open();

        //        EditorManager.Instance?.UpdateUsability();
        //    }
        //    SettingsManager.Instance.SetMasterVolume(currVolume);
        //}
        //public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        //{
        //}

        public void OnPurchaseComplete(Product product)
        {
            if (product.definition.id == "premium")
            {
                Data.IsPremium = true;
                Save();

                InformationDialog.Inform(LocalizationUtility.Localize("premium_paid_success_title"), LocalizationUtility.Localize("premium_paid_success_message"));

                EditorManager.Instance?.UpdateUsability();
            }
        }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            InformationDialog.Inform(LocalizationUtility.Localize("premium_paid_failed_title"), LocalizationUtility.Localize("premium_paid_failed_message", reason));
        }
        #endregion

        #region Nested
        [Serializable]
        public class RewardedItem
        {
            public ItemType Type;
            public string Id;

            public RewardedItem(ItemType type, string id)
            {
                Type = type;
                Id = id;
            }
        }

        public enum ItemType
        {
            BodyPart,
            Pattern
        }
        #endregion
    }
}