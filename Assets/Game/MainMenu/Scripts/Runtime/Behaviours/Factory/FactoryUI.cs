using UnityEngine;
using Steamworks;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryUI : MonoBehaviour, IPointerEnterHandler
    {
        #region Fields
        [SerializeField] private GameObject factoryCreatureUIPrefab;
        [SerializeField] private Toggle togglePrefab;
        [SerializeField] private SimpleScrollSnap.SimpleScrollSnap factoryScrollSnap;
        [SerializeField] private float carouselTime;
        [SerializeField] private int maxCreatures;
        [SerializeField] private RectTransform pagination;
        [SerializeField] private ToggleGroup toggleGroup;

        private bool hasEntered;
        #endregion

        #region Methods
        private void Start()
        {
            if (SettingsManager.Data.Tutorial && ProgressManager.Data.UnlockedBodyParts.Count == 0 && ProgressManager.Data.UnlockedPatterns.Count == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Setup();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hasEntered = true;
        }

        private void Setup()
        {
            CallResult<SteamUGCQueryCompleted_t> query = CallResult<SteamUGCQueryCompleted_t>.Create(OnQueryComplete);

            UGCQueryHandle_t handle = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByTrend, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, SteamUtils.GetAppID(), SteamUtils.GetAppID());
            SteamUGC.SetMatchAnyTag(handle, true);
            SteamUGC.SetRankedByTrendDays(handle, 7);

            SteamAPICall_t call = SteamUGC.SendQueryUGCRequest(handle);
            query.Set(call);

            StartCoroutine(CarouselRoutine());
        }

        private void OnQueryComplete(SteamUGCQueryCompleted_t param, bool hasFailed)
        {
            if (hasFailed)
            {
                return;
            }

            for (uint i = 0; i < param.m_unNumResultsReturned && i < maxCreatures; i++)
            {
                Toggle toggle = Instantiate(togglePrefab, pagination);
                toggle.group = toggleGroup;

                FactoryCreatureUI factoryCreatureUI = factoryScrollSnap.AddToBack(factoryCreatureUIPrefab).GetComponent<FactoryCreatureUI>();
                if (SteamUGC.GetQueryUGCResult(param.m_handle, i, out SteamUGCDetails_t details))
                {
                    factoryCreatureUI.Setup(details.m_rgchTitle, details.m_unVotesUp, details.m_nPublishedFileId);
                }
                if (SteamUGC.GetQueryUGCPreviewURL(param.m_handle, i, out string url, 256))
                {
                    factoryCreatureUI.SetPreview(url);
                }

            }
        }

        private IEnumerator CarouselRoutine()
        {
            while (!hasEntered)
            {
                yield return new WaitForSeconds(carouselTime);
                if (!hasEntered)
                {
                    factoryScrollSnap.GoToNextPanel();
                }
            }
        }
        #endregion
    }
}