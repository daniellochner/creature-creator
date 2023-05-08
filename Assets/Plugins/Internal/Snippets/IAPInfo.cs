using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(IAPButton))]
    public class IAPInfo : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI priceText;

        private IAPButton button;
        #endregion

        #region Methods
        private void Awake()
        {
            button = GetComponent<IAPButton>();
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => CodelessIAPStoreListener.initializationComplete);

            Product product = CodelessIAPStoreListener.Instance.GetProduct(button.productId);
            if (descriptionText != null)
            {
                descriptionText.text = product.metadata.localizedDescription;
            }
            if (priceText != null)
            {
                priceText.text = product.metadata.localizedPriceString;
            }
        }
        #endregion
    }
}