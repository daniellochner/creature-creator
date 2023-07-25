using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class KaijuHunt : Minigame
    {
        #region Fields
        [Header("Kaiju Hunt")]
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float trackHealthCooldown;

        private int myIndex;
        private CreatureBase kaiju;
        private bool isKaijuDead;
        #endregion

        #region Properties
        private bool IsKaiju => InMinigame && (myIndex == 0);
        #endregion

        #region Methods
        protected override void Setup()
        {
            base.Setup();

            waitingForPlayers.onExit += OnWaitingForPlayersExit;

            playing.onEnter += OnPlayingEnter;
        }

        #region Waiting For Players
        private void OnWaitingForPlayersExit()
        {
            Assign();
        }

        private void Assign()
        {
            // Randomize ordering of players
            players.Shuffle();

            // Assign indices
            for (int i = 0; i < players.Count; i++)
            {
                AssignClientRpc(i, NetworkUtils.SendTo(players[i]));
            }

            // Assign kaiju
            kaiju = NetworkManager.SpawnManager.GetPlayerNetworkObject(players[0]).GetComponent<CreatureBase>();
        }
        [ClientRpc]
        private void AssignClientRpc(int index, ClientRpcParams sendTo)
        {
            myIndex = index;
        }
        #endregion

        #region Building
        protected override void OnApplyRestrictions()
        {
            base.OnApplyRestrictions();

            if (!IsKaiju)
            {
                EditorManager.Instance.SetRestrictedBones(5);
                EditorManager.Instance.SetRestrictedCash(250);
                EditorManager.Instance.SetRestrictedComplexity(50);

                List<string> bodyParts = new List<string>();
                foreach (var obj in DatabaseManager.GetDatabase("Body Parts").Objects)
                {
                    BodyPart bodyPart = obj.Value as BodyPart;
                    if (bodyPart.Abilities.Find(x => x is Abilities.Emit || x is Abilities.Launch || x is Abilities.Spin || (x is Abilities.Bite && x.Level > 1)))
                    {
                        bodyParts.Add(obj.Key);
                    }
                }
                EditorManager.Instance.SetRestrictedBodyParts(bodyParts);
            }
        }
        #endregion

        #region Starting
        public override Transform GetSpawnPoint()
        {
            return spawnPoints[myIndex];
        }

        public override void OnSetupScoreboard()
        {
            Scoreboard.Add(new Score()
            {
                id = players[0].ToString(),
                score = (int)kaiju.Health.Health,
                displayName = NetworkHostManager.Instance.Players[players[0]].username
            });
        }
        #endregion

        #region Playing
        private void OnPlayingEnter()
        {
            StartCoroutine(KaijuTrackHealthRoutine());
        }

        private IEnumerator KaijuTrackHealthRoutine()
        {
            while (State.Value == MinigameStateType.Playing)
            {
                Scoreboard[0] = new Score(Scoreboard[0], (int)kaiju.Health.Health);
                yield return new WaitForSeconds(trackHealthCooldown);
            }
            if (isKaijuDead)
            {
                Scoreboard[0] = new Score(Scoreboard[0], 0);
            }
        }

        protected override IEnumerator GameplayLogicRoutine()
        {
            yield return new WaitUntil(() => kaiju == null || (isKaijuDead = kaiju.Health.IsDead));
        }
        #endregion

        #region Completing
        protected override List<ulong> GetWinnerClientIds()
        {
            if (isKaijuDead)
            {
                List<ulong> hunters = new List<ulong>(players);
                hunters.RemoveAt(0);
                return hunters;
            }
            else
            {
                return new List<ulong>() { players[0] };
            }
        }
        protected override string GetWinnerName()
        {
            if (isKaijuDead)
            {
                return LocalizationUtility.Localize("minigame_kaiju-hunt_hunters");
            }
            else
            {
                return LocalizationUtility.Localize("minigame_kaiju-hunt_kaiju");
            }
        }

        protected override void OnServerShutdown()
        {
            kaiju = null;
            isKaijuDead = false;

            base.OnServerShutdown();
        }
        protected override void OnClientShutdown()
        {
            if (InMinigame)
            {
                myIndex = 0;
            }

            base.OnClientShutdown();
        }
        #endregion
        #endregion
    }
}