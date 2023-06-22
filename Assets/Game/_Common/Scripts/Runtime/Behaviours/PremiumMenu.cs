using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PremiumMenu : Dialog<PremiumMenu>
    {
        #region Fields
        [Header("Paid")]
        [SerializeField] private RectTransform paidRT;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI buyText;
        [SerializeField] private GameObject buyLoadIcon;
        [SerializeField] private GameObject[] premiumButtons;

        [Header("Free")]
        [SerializeField] private RectTransform freeRT;
        [SerializeField] private Image requestedItemImg;
        [SerializeField] private BlinkingCanvasGroup requestedItemBCG;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private TextMeshProUGUI watchAdText;
        [SerializeField] private GameObject watchAdLoadIcon;
        [SerializeField] private Sprite questionMarkIcon;
        #endregion

        #region Properties
        private bool IsLoadingPurchase
        {
            set
            {
                buyLoadIcon.SetActive(value);
                buyText.gameObject.SetActive(!value);
                buyButton.interactable = !value;
            }
        }
        private bool IsLoadingAd
        {
            set
            {
                watchAdLoadIcon.SetActive(value);
                watchAdText.gameObject.SetActive(!value);
                watchAdButton.interactable = !value;
            }
        }
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();

            if (PremiumManager.Instance.IsEverythingUsable())
            {
                paidRT.pivot = new Vector2(0.5f, 0.5f);
                paidRT.anchoredPosition = Vector2.zero;

                freeRT.gameObject.SetActive(false);
            }
            HandlePremiumButtons();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            PremiumManager.Instance.OnPurchaseComplete += OnPurchaseComplete;
        }
        private void OnDisable()
        {
            if (PremiumManager.Instance)
            {
                PremiumManager.Instance.OnPurchaseComplete -= OnPurchaseComplete;
            }
        }

        public void RequestBodyPart(string bodyPartId)
        {
            PremiumManager.Instance.RequestedItem = new PremiumManager.RewardedItem(PremiumManager.ItemType.BodyPart, bodyPartId);
            Setup(DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartId).Icon, false);
        }
        public void RequestPattern(string patternId)
        {
            PremiumManager.Instance.RequestedItem = new PremiumManager.RewardedItem(PremiumManager.ItemType.Pattern, patternId);
            Setup(DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", patternId).Icon, false);
        }
        public void RequestNothing()
        {
            PremiumManager.Instance.RequestedItem = null;
            Setup(questionMarkIcon, true);
        }
        private void Setup(Sprite icon, bool isBlinking)
        {
            if (!PremiumManager.Instance.IsInitialized)
            {
                return;
            }

            // Paid
            buyText.text = PremiumManager.Instance.Controller.products.WithID("cc_premium").metadata.localizedPriceString;

            // Free
            requestedItemBCG.IsBlinking = isBlinking;
            requestedItemBCG.CanvasGroup.alpha = 1f;
            requestedItemImg.sprite = icon;

            Open();
        }

        public override void Open(bool instant = false)
        {
            base.Open(instant);

            if (!PremiumManager.Instance.IsRewardAdLoaded)
            {
                IsLoadingAd = true;
                PremiumManager.Instance.RequestRewardAd(OnRewardAdLoaded);
            }
        }

        #region Free
        public void WatchAd()
        {
            if (PremiumManager.Instance.IsRewardAdLoaded)
            {
                PremiumManager.Instance.ShowRewardAd();
            }
        }
        public void OnRewardAdLoaded(RewardedAd ad, LoadAdError error)
        {
            IsLoadingAd = false;
        }
        #endregion

        #region Paid
        public void OnPurchaseClicked()
        {
            IsLoadingPurchase = true;
            PremiumManager.Instance.Controller.InitiatePurchase("cc_premium");
        }
        public void OnPurchaseComplete()
        {
            IsLoadingPurchase = false;
            HandlePremiumButtons();
            Close();
        }

        private void HandlePremiumButtons()
        {
            foreach (GameObject button in premiumButtons)
            {
                button.SetActive(!PremiumManager.Data.IsPremium && !SettingsManager.Instance.ShowTutorial);
            }
        }
        #endregion
        #endregion
    }
}