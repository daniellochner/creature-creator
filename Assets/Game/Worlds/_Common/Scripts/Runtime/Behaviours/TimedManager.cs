using TMPro;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TimedManager : MonoBehaviourSingleton<TimedManager>
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI bestTimeText;
    }
}