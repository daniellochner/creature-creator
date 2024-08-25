using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TimeUI : MonoBehaviour
    {
        public Color defaultColor;
        public Color myColor;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI timeText;
        public RawImage backgroundRawImg;

        public void Setup(string name, int rank, int time, bool isMine = false)
        {
            nameText.text = $"{rank+1}.\t {name}".NoParse();
            timeText.text = TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss");

            SetMine(isMine);
        }
        public void SetMine(bool isMine)
        {
            backgroundRawImg.color = isMine ? myColor : defaultColor;
        }
    }
}