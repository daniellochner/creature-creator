using System.Collections;

#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID
using Google.Play.Review;
#endif

namespace DanielLochner.Assets
{
    public class RatingManager : MonoBehaviourSingleton<RatingManager>
    {
        #region Fields
#if UNITY_ANDROID
        private ReviewManager reviewManager;
        private PlayReviewInfo playReviewInfo;
#endif
        #endregion

        #region Methods
        public void Rate()
        {
#if UNITY_IOS
            Device.RequestStoreReview();
#elif UNITY_ANDROID
            StartCoroutine(RequestReview());
#endif
        }

#if UNITY_ANDROID
        private IEnumerator RequestReview()
        {
            reviewManager = new ReviewManager();

            // Request a ReviewInfo object
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                yield break;
            }
            playReviewInfo = requestFlowOperation.GetResult();

            // Launch the in-app review flow
            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
            yield return launchFlowOperation;
            playReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                yield break;
            }
        }
#endif
        #endregion
    }
}