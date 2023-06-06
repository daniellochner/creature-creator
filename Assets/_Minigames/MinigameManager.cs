using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameManager : MonoBehaviourSingleton<MinigameManager>
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private ScoreboardUI scoreboard;

        [SerializeField] private TextMeshProUGUI playText;
        [SerializeField] private Toggle playToggle;

        public ScoreboardUI Scoreboard => scoreboard;
        public Minigame CurrentMinigame { get; set; }

        public void Setup(Minigame minigame)
        {
            CurrentMinigame = minigame;

            scoreboard.Setup(minigame);
        }

        public void SetTitle(string title)
        {
            titleText.text = title;
            titleText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(title));
        }
        public void SetSubtitle(string subtitle)
        {
            subtitleText.text = subtitle;
            subtitleText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(subtitle));
        }
        public void SetPlayOverride(string text, bool isInteractable)
        {
            playText.text = text;
            playToggle.interactable = isInteractable;
        }
    }
}