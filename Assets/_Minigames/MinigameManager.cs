using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameManager : MonoBehaviourSingleton<MinigameManager>
    {
        [SerializeField] private ScoreboardUI scoreboard;

        public Minigame CurrentMinigame { get; set; }

        
    }
}