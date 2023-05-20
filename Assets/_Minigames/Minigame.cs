using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Minigame : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float scoreboardHeight;
        [SerializeField] private int minPlayers;
        [SerializeField] private int maxPlayers;
        [SerializeField] private string nameId;

        [SerializeField] private Platform platform;
        #endregion

        #region Properties
        public float ScoreboardHeight => scoreboardHeight;
        public int MinPlayers => minPlayers;
        public int MaxPlayers => Mathf.Min(maxPlayers, NetworkPlayersMenu.Instance.NumPlayers);
        public string Name => LocalizationUtility.Localize(nameId);

        public GameState State { get; set; }
        #endregion

        #region Methods
        public void Begin()
        {
            State = GameState.Starting;
            

        }

        public virtual void OnWaitForPlayers()
        {
            
        }

        public virtual void OnStart()
        {
            // cinematic?
        }

        public virtual void OnBuild()
        {
            // impose all constraints in the editor

            // fade to black then teleport to platform, then instant switch to build mode
        }

        public virtual void OnPlay()
        {
            // fade to black then set to play mode and teleport to starting points for game

            // disable all editing platforms
            // hide other players
        }


        public virtual void OnComplete()
        {

            // enable all editing platforms
            // show other players


            
        }
        #endregion


        #region Enums
        public enum GameState
        {
            Stopped,
            WaitingForPlayers,
            Starting,
            Building,
            Playing,
            Completed
        }
        #endregion
    }
}