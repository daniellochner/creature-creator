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
            if (Application.internetReachability != NetworkReachability.NotReachable && !(SettingsManager.Data.Tutorial && ProgressManager.Data.UnlockedBodyParts.Count == 0 && ProgressManager.Data.UnlockedPatterns.Count == 0))
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

            List<uint> indices = new List<uint>()
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            };
            indices.Shuffle();

            for (uint i = 0; i < param.m_unNumResultsReturned && i < maxCreatures; i++)
            {
                uint index = indices[(int)i];

                Toggle toggle = Instantiate(togglePrefab, pagination);
                toggle.group = toggleGroup;

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

            if (param.m_unNumResultsReturned > 0)
            {
                StartCoroutine(canvasGroup.Fade(true, 0.25f));
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