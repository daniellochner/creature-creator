using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigamesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private ScoreboardUI scoreboard;

        public string Title
        {
            get => titleText.text;
            set
            {
                titleText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(titleText.text = value));
            }
        }
        public string Subtitle
        {
            get => subtitleText.text;
            set
            {
                subtitleText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(subtitleText.text = value));
            }
        }
    }
}