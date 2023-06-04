// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

#if UNITY_STANDALONE
using Steamworks;
#endif

namespace DanielLochner.Assets.CreatureCreator
{
    public class FactoryUI : MonoBehaviour
#if UNITY_STANDALONE
        , IPointerEnterHandler
#endif
    {
        #region Fields
        [SerializeField] private GameObject factoryCreatureUIPrefab;
        [SerializeField] private GameObject moreUIPrefab;
        [SerializeField] private Toggle togglePrefab;
        [SerializeField] private SimpleScrollSnap.SimpleScrollSnap factoryScrollSnap;
        [SerializeField] private float carouselTime;
        [SerializeField] private int maxCreatures;
        [SerializeField] private RectTransform pagination;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private CanvasGroup canvasGroup;

        private bool hasEntered;
        #endregion

        #region Methods
#if UNITY_STANDALONE
        private void Start()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable && !SettingsManager.Instance.ShowTutorial)
            {
                Setup();
            }
        }
        private void Setup()
        {
            CallResult<SteamUGCQueryCompleted_t> query = CallResult<SteamUGCQueryCompleted_t>.Create(OnQueryComplete);

            UGCQueryHandle_t handle = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByTrend, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items_ReadyToUse, SteamUtils.GetAppID(), SteamUtils.GetAppID());
            SteamUGC.SetMatchAnyTag(handle, true);
            SteamUGC.SetRankedByTrendDays(handle, 7);

            SteamAPICall_t call = SteamUGC.SendQueryUGCRequest(handle);
            query.Set(call);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hasEntered = true;
        }
        public void OnQueryComplete(SteamUGCQueryCompleted_t param, bool hasFailed)
        {
            if (hasFailed || this is null)
            {
                return;
            }

            List<uint> indices = new List<uint>()
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            };
            indices.Shuffle();

            for (uint i = 0; i < param.m_unNumResultsReturned && i < maxCreatures; i++)
            {
                uint index = indices[(int)i];

                Instantiate(togglePrefab, pagination).group = toggleGroup;

                FactoryCreatureUI factoryCreatureUI = factoryScrollSnap.AddToBack(factoryCreatureUIPrefab).GetComponent<FactoryCreatureUI>();
                if (SteamUGC.GetQueryUGCResult(param.m_handle, index, out SteamUGCDetails_t details))
                {
                    factoryCreatureUI.Setup(details.m_rgchTitle, details.m_unVotesUp, details.m_nPublishedFileId);
                }
                if (SteamUGC.GetQueryUGCPreviewURL(param.m_handle, index, out string url, 256))
                {
                    factoryCreatureUI.SetPreview(url);
                }
            }

            Instantiate(togglePrefab, pagination).group = toggleGroup;

            Button moreUI = factoryScrollSnap.AddToBack(moreUIPrefab).GetComponent<Button>();
            moreUI.onClick.AddListener(delegate
            {
                SteamFriends.ActivateGameOverlayToWebPage($"steam://url/SteamWorkshopPage/1990050");
            });

            if (param.m_unNumResultsReturned > 0)
            {
                StartCoroutine(canvasGroup.Fade(true, 0.25f));
                StartCoroutine(CarouselRoutine());
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
#endif
        #endregion
    }
}