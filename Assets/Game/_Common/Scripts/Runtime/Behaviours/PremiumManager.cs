using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;
using System.IO;
using GoogleMobileAds.Api;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class PremiumManager : DataManager<PremiumManager, Premium>
    {
        #region Fields
        private BannerView bannerAd;
        private RewardedAd rewardAd;

        private bool wasPrevPurchased;
        #endregion

        #region Properties
        private string BannerAdUnitId
        {
            get
            {
#if UNITY_EDITOR
                string adUnitId = "unused";
#elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-8574849693522303/8775844882";
#elif UNITY_IOS
                string adUnitId = "ca-app-pub-8574849693522303/2350037330";
#else
                string adUnitId = "unexpected_platform";
#endif
                return adUnitId;
            }
        }
        private string RewardAdUnitId
        {
            get
            {
#if UNITY_EDITOR
                string adUnitId = "unused";
#elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-8574849693522303/4330129572";
#elif UNITY_IOS
                string adUnitId = "ca-app-pub-8574849693522303/3208619599";
#else
                string adUnitId = "unexpected_platform";
#endif
                return adUnitId;
            }
        }

        public override string SALT
        {
            get
            {
#if UNITY_STANDALONE
                return SteamUser.GetSteamID().ToString();
#elif UNITY_IOS || UNITY_ANDROID
                return SystemInfo.deviceUniqueIdentifier;
#endif
            }
        }

        public RewardedItem RequestedItem
        {
            get;
            set;
        }

        public bool IsRewardAdLoaded
        {
            get => rewardAd != null && rewardAd.CanShowAd();
        }
        private int BannerAdWidth
        {
            get
            {
                return 160 * Mathf.RoundToInt((/*Display.main.systemWidth*/Screen.safeArea.width / Screen.dpi) / 3f);
            }
        }
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

            if (SystemUtility.IsDevice(DeviceType.Handheld))
            {
                MobileAds.SetiOSAppPauseOnBackground(true);
                MobileAds.RaiseAdEventsOnUnityMainThread = true;

                MobileAds.Initialize(OnInitialized);
            }
        }

        public void OnInitialized(InitializationStatus status)
        {
            RequestBannerAd();
        }

        #region Banner
        public void RequestBannerAd()
        {
            //if (Data.IsPremium) return;

            //bannerAd?.Destroy();
            //bannerAd = new BannerView(BannerAdUnitId, /*AdSize.Banner*/AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(BannerAdWidth), AdPosition.Top);

            //bannerAd.OnBannerAdLoaded += OnBannerAdLoaded;
            //bannerAd.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

            //bannerAd.LoadAd(new AdRequest());
            //bannerAd.Hide();
        }

        public void ShowBannerAd()
        {
            //if (Data.IsPremium) return;
            //bannerAd?.Show();
        }
        public void HideBannerAd()
        {
            //bannerAd?.Hide();
        }

        private void OnBannerAdLoadFailed(LoadAdError error)
        {
            if (error.GetCode() == 2)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    this.InvokeUntil(() => Application.internetReachability != NetworkReachability.NotReachable, RequestBannerAd);
                }
            }
        }
        private void OnBannerAdLoaded()
        {
        }
        #endregion

        #region Reward
        public void RequestRewardAd(Action<RewardedAd, LoadAdError> onLoaded)
        {
            if (IsRewardAdLoaded) return;

            RewardedAd.Load(RewardAdUnitId, new AdRequest(), delegate (RewardedAd ad, LoadAdError error) 
            {
                rewardAd = ad;

                if (ad == null || error != null)
                {
                    InformationDialog.Inform($"Error ({error?.GetCode()})", error?.GetMessage());
                    return;
                }

                ad.OnAdFullScreenContentOpened += OnRewardAdOpened;
                ad.OnAdFullScreenContentClosed += OnRewardAdClosed;

                onLoaded(ad, error);
            });
        }

        public void ShowRewardAd()
        {
            rewardAd?.Show(OnRewardAdCompleted);
        }

        private void OnRewardAdOpened()
        {
            MobileAds.SetApplicationMuted(true);
        }
        private void OnRewardAdClosed()
        {
            MobileAds.SetApplicationMuted(false);
        }
        private void OnRewardAdCompleted(Reward reward)
        {
            PremiumMenu.Instance.Close(true);

            RewardsMenu.Instance.Clear();
            if (RequestedItem != null)
            {
                Access(RequestedItem);
                AccessRandom(3);
            }
            else
            {
                AccessRandom(4);
            }
            RewardsMenu.Instance.Open();

            EditorManager.Instance?.UpdateUsability();
            OnRewardAdClosed();
        }

        private void Access(RewardedItem item)
        {
            switch (item.Type)
            {
                case ItemType.BodyPart:
                    RewardsMenu.Instance.Add(DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", item.Id));
                    Data.UsableBodyParts.Add(item.Id, true);
                    break;

                case ItemType.Pattern:
                    RewardsMenu.Instance.Add(DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", item.Id));
                    Data.UsablePatterns.Add(item.Id, true);
                    break;
            }
            Save();
        }
        private void AccessRandom(int count)
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
        #endregion

        #region Premium
        public void OnPremiumPurchased()
        {
            Data.IsPremium = true;
            Save();

            InformationDialog.Inform(LocalizationUtility.Localize("premium_paid_success_title"), LocalizationUtility.Localize("premium_paid_success_message"));

            EditorManager.Instance?.UpdateUsability();
            HideBannerAd();
        }
        public void OnPremiumFailed(string reason)
        {
            InformationDialog.Inform(LocalizationUtility.Localize("premium_paid_failed_title"), LocalizationUtility.Localize("premium_paid_failed_message", reason));
        }
        #endregion

        #region Helper
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

        public bool IsEverythingUsable()
        {
            if (Data.IsPremium) return true;

            foreach (string id in DatabaseManager.GetDatabase("Body Parts").Objects.Keys)
            {
                if (!IsBodyPartUsable(id))
                {
                    return false;
                }
            }
            foreach (string id in DatabaseManager.GetDatabase("Patterns").Objects.Keys)
            {
                if (!IsPatternUsable(id))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
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