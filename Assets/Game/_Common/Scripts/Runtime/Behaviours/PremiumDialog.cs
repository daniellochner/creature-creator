using TMPro;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class PremiumDialog : Dialog<PremiumDialog>
    {
        #region Fields
        [SerializeField] private Image requestedItemImg;
        [SerializeField] private BlinkingCanvasGroup requestedItemBCG;
        [SerializeField] private Sprite questionMarkIcon;
        [SerializeField] private TextMeshProUGUI priceText;
        #endregion

        #region Methods
        public void RequestPremiumBodyPart(string bodyPartId)
        {
            PremiumManager.Instance.RequestedItem = (new PremiumManager.RewardedItem(PremiumManager.ItemType.BodyPart, bodyPartId));
            Setup(DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartId).Icon, false);
        }
        public void RequestPremiumPattern(string patternId)
        {
            PremiumManager.Instance.RequestedItem = (new PremiumManager.RewardedItem(PremiumManager.ItemType.Pattern, patternId));
            Setup(DatabaseManager.GetDatabaseEntry<Pattern>("Patterns", patternId).Icon, false);
        }
        public void RequestNothing()
        {
            PremiumManager.Instance.RequestedItem = null;
            Setup(questionMarkIcon, true);
        }
        private async void Setup(Sprite icon, bool isBlinking)
        {

            //yield return new WaitUntil(() => CodelessIAPStoreListener.initializationComplete);


            //Product product = CodelessIAPStoreListener.Instance.GetProduct("premium");
            //priceText.text = product.metadata.localizedPriceString;


            requestedItemImg.sprite = icon;

            requestedItemBCG.IsBlinking = isBlinking;
            requestedItemBCG.CanvasGroup.alpha = 1f;

            await UnityServices.InitializeAsync();

            Open();
        }
        public void WatchAd()
        {
            PremiumManager.Instance.ShowReward();
        }

        public void OnPurchaseComplete(Product product)
        {
            PremiumManager.Instance.OnPurchaseComplete(product);
        }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            PremiumManager.Instance.OnPurchaseFailed(product, reason);
        }
        #endregion
    }
}