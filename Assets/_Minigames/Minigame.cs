using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Minigame : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private string nameId;

        [Header("Waiting For Players")]
        [SerializeField] private int minPlayers;
        [SerializeField] private int maxPlayers;
        [SerializeField] private int waitTime;

        [Header("Starting")]
        [SerializeField] private MinigamePad pad;
        [SerializeField] private MinigameCinematic cinematic;
        [SerializeField] private MinigameZone zone;
        [SerializeField] private Platform platform;
        [SerializeField] private float expandTime;

        [Header("Building")]
        [SerializeField] private int buildTime;
        [SerializeField] private TextMeshProUGUI playText;
        [SerializeField] private Toggle playToggle;

        [Header("Introducing")]
        [SerializeField] private string objectiveId;
        [SerializeField] private float introduceTime;

        [Header("Playing")]
        [SerializeField] private float scoreboardHeight;
        [SerializeField] private float playTime;
        [SerializeField] private Transform[] spawnPoints;

        private float _waitTimeLeft, _buildTimeLeft, _introduceTimeLeft, _playTimeLeft;
        #endregion

        #region Properties
        public string Name => LocalizationUtility.Localize(nameId);

        public int MinPlayers => minPlayers;
        public int MaxPlayers => Mathf.Min(maxPlayers, NetworkPlayersManager.Instance.NumPlayers);

        public int WaitTime => waitTime;

        public float ScoreboardHeight => scoreboardHeight;


        public NetworkVariable<int> WaitTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> BuildTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> IntroduceTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> PlayTimeLeft { get; set; } = new NetworkVariable<int>(-1);

        public NetworkVariable<MinigameState> State { get; set; } = new NetworkVariable<MinigameState>(MinigameState.WaitingForPlayers);
        #endregion

        #region Methods
        private void Start()
        {
            OnWaitingForPlayersStart();
            OnStartingStart();
            OnBuildingStart();
            OnPlayingStart();
            OnCompletingStart();


            if (!IsServer)
            {
                zone.Bounds.gameObject.SetActive(State.Value != MinigameState.WaitingForPlayers);
            }
        }
        private void Update()
        {
            switch (State.Value)
            {
                case MinigameState.WaitingForPlayers:
                    OnWaitingForPlayersUpdate();
                    break;

                case MinigameState.Starting:
                    OnStartingUpdate();
                    break;

                case MinigameState.Building:
                    OnBuildingUpdate();
                    break;

                case MinigameState.Introducing:
                    OnIntroducingUpdate();
                    break;

                case MinigameState.Playing:
                    OnPlayingUpdate();
                    break;

                case MinigameState.Completing:
                    OnCompletingUpdate();
                    break;
            }
        }

        public void ChangeState(MinigameState state)
        {
            ChangeStateClientRpc(state);
            Debug.Log(state);
        }
        [ClientRpc]
        public void ChangeStateClientRpc(MinigameState state)
        {
            switch (State.Value)
            {
                case MinigameState.WaitingForPlayers:
                    OnWaitingForPlayersExit();
                    break;

                case MinigameState.Starting:
                    OnStartingExit();
                    break;

                case MinigameState.Building:
                    OnBuildingExit();
                    break;

                case MinigameState.Introducing:
                    OnIntroducingExit();
                    break;

                case MinigameState.Playing:
                    OnPlayingExit();
                    break;

                case MinigameState.Completing:
                    OnCompletingExit();
                    break;
            }

            State.Value = state;

            switch (State.Value)
            {
                case MinigameState.WaitingForPlayers:
                    OnWaitingForPlayersEnter();
                    break;

                case MinigameState.Starting:
                    OnStartingEnter();
                    break;

                case MinigameState.Building:
                    OnBuildingEnter();
                    break;

                case MinigameState.Introducing:
                    OnIntroducingEnter();
                    break;

                case MinigameState.Playing:
                    OnPlayingEnter();
                    break;

                case MinigameState.Completing:
                    OnCompletingEnter();
                    break;
            }
        }

        #region Waiting For Players
        public virtual void OnWaitingForPlayersStart()
        {
        }
        public virtual void OnWaitingForPlayersEnter()
        {
            if (IsServer)
            {
                _waitTimeLeft = waitTime;
            }
        }
        public virtual void OnWaitingForPlayersUpdate()
        {
            if (IsServer)
            {
                if ((pad.NumPlayers >= MinPlayers) && (pad.NumPlayers <= MaxPlayers))
                {
                    if (_waitTimeLeft > 0)
                    {
                        _waitTimeLeft -= Time.deltaTime;
                    }
                    else
                    {
                        ChangeState(MinigameState.Starting);
                    }
                }
                else
                {
                    _waitTimeLeft = WaitTime;
                }

                WaitTimeLeft.Value = Mathf.CeilToInt(_waitTimeLeft);
            }
        }
        public virtual void OnWaitingForPlayersExit()
        {
            pad.gameObject.SetActive(false);
        }
        #endregion

        #region Starting
        public virtual void OnStartingStart()
        {
        }
        public virtual void OnStartingEnter()
        {
            MinigameManager.Instance.Setup(this);

            cinematic.OnShow = OnCinematicShow;
            cinematic.OnHide = OnCinematicHide;
            cinematic.Begin();

            zone.gameObject.SetActive(true);
        }
        public virtual void OnStartingUpdate()
        {
        }
        public virtual void OnStartingExit()
        {
        }

        private void OnCinematicShow()
        {
            platform.TeleportTo(true, false);
        }
        private void OnCinematicHide()
        {
            if (IsServer)
            {
                ChangeState(MinigameState.Building);
            }
            else if (State.Value != MinigameState.Building)
            {
                cinematic.Pause();

                this.InvokeUntil(() => State.Value == MinigameState.Building, cinematic.Unpause);
            }
        }

        public void ShowAndExpandBounds()
        {
            if (IsServer)
            {
                ShowBoundsClientRpc();

                this.InvokeOverTime(delegate (float p)
                {
                    zone.Bounds.localScale = new Vector3(p, 1f, p);
                }, 
                expandTime);
            }
        }
        [ClientRpc]
        private void ShowBoundsClientRpc()
        {
            zone.Bounds.gameObject.SetActive(true);
        }
        #endregion

        #region Building
        public virtual void OnBuildingStart()
        {
            BuildTimeLeft.OnValueChanged += OnBuildTimeLeftChanged;
        }
        public virtual void OnBuildingEnter()
        {
            EditorManager.Instance.SetMode(EditorManager.EditorMode.Build, true);

            EditorManager.Instance.ClearHistory();
            EditorManager.Instance.Load(null);

            OnApplyRestrictions();

            MinigameManager.Instance.SetPlayOverride(buildTime.ToString(), false);

            platform.TeleportTo(true, false);

            if (IsServer)
            {
                _buildTimeLeft = buildTime;
            }
        }
        public virtual void OnBuildingUpdate()
        {
            if (IsServer)
            {
                if (_buildTimeLeft > 0)
                {
                    _buildTimeLeft -= Time.deltaTime;
                }
                else
                {
                    ChangeState(MinigameState.Introducing);
                }

                BuildTimeLeft.Value = Mathf.CeilToInt(_buildTimeLeft);
            }
        }
        public virtual void OnBuildingExit()
        {
            MinigameManager.Instance.SetPlayOverride(LocalizationUtility.Localize("cc_pagination_play"), true);
        }

        private void OnBuildTimeLeftChanged(int oldBT, int newBT)
        {
            MinigameManager.Instance.SetPlayOverride(newBT.ToString(), false);
        }

        protected virtual void OnApplyRestrictions()
        {
            EditorManager.Instance.UpdateLoadableCreatures();
        }
        #endregion

        #region Introducing
        public virtual void OnIntroducingStart()
        {
        }
        public virtual void OnIntroducingEnter()
        {
            Player.Instance.Mover.FreezeMove = true;

            EditorManager.Instance.SetMode(EditorManager.EditorMode.Play);

            this.Invoke(delegate
            {
                ShowInfo();
                Teleport();
            }, 
            0.5f);

            if (IsServer)
            {
                _introduceTimeLeft = introduceTime;
            }
        }
        public virtual void OnIntroducingUpdate()
        {
            if (IsServer)
            {
                if (_introduceTimeLeft > 0)
                {
                    _introduceTimeLeft -= Time.deltaTime;
                }
                else
                {
                    ChangeState(MinigameState.Playing);
                }
            }
        }
        public virtual void OnIntroducingExit()
        {
            MinigameManager.Instance.SetTitle(null);

            Player.Instance.Mover.FreezeMove = false;
        }

        protected virtual void ShowInfo()
        {
            MinigameManager.Instance.SetTitle(LocalizationUtility.Localize(objectiveId));
        }
        protected virtual void Teleport()
        {

            int hash = (WorldManager.Instance.World as WorldMP).WorldName.GetHashCode();
            Player.Instance.Mover.Teleport(spawnPoints[0].position, spawnPoints[0].rotation, true);
        }
        #endregion

        #region Playing
        public virtual void OnPlayingStart()
        {
        }
        public virtual void OnPlayingEnter()
        {
        }
        public virtual void OnPlayingUpdate()
        {
        }
        public virtual void OnPlayingExit()
        {
        }
        #endregion

        #region Completing
        public virtual void OnCompletingStart()
        {
        }
        public virtual void OnCompletingEnter()
        {
        }
        public virtual void OnCompletingUpdate()
        {
        }
        public virtual void OnCompletingExit()
        {
            zone.gameObject.SetActive(false);
        }
        #endregion
        #endregion

        #region Enums
        public enum MinigameState
        {
            WaitingForPlayers,
            Starting,
            Building,
            Introducing,
            Playing,
            Completing
        }
        #endregion
    }
}