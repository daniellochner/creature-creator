using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class Minigame : NetworkBehaviour
    {
        #region Fields
        [Header("Minigame")]
        [SerializeField] protected string nameId;
        [SerializeField] private Sprite icon;
        [Space]
        [SerializeField] protected MinigamePad pad;
        [SerializeField] protected int waitTime;
        [SerializeField] protected int minPlayers;
        [SerializeField] protected int maxPlayers;
        [Space]
        [SerializeField] protected MinigameCinematic cinematic;
        [SerializeField] protected MinigameZone zone;
        [SerializeField] protected float expandTime;
        [SerializeField] protected float showBoundsTime;
        [Space]
        [SerializeField] protected Platform platform;
        [SerializeField] protected TextAsset creaturePreset;
        [SerializeField] protected int buildTime;
        [Space]
        [SerializeField] protected string objectiveId;
        [SerializeField] protected string musicId;
        [SerializeField] private AudioClip countdownFX;
        [SerializeField] private AudioClip whistleFX;
        [SerializeField] protected float switchTime;
        [SerializeField] protected float teleportTime;
        [SerializeField] protected int startTime;
        [Space]
        [SerializeField] protected int playTime;
        [SerializeField] protected bool enablePVP;
        [SerializeField] protected bool spawnCorpses;
        [SerializeField] protected bool isAscendingOrder;
        [Space]
        [SerializeField] protected MinMax minMaxFireworksCooldown;
        [SerializeField] protected int celebrateTime;
        [SerializeField] protected int completeTime;
        [SerializeField] protected GameObject[] fireworksPrefabs;
        [SerializeField] protected int experience;

        protected List<MinigameState> states = new List<MinigameState>();
        protected List<ulong> players = new List<ulong>();

        protected MinigameState waitingForPlayers, introducing, building, starting, playing, completing;
        private AudioSource audioSource;
        #endregion

        #region Properties
        public string Name => LocalizationUtility.Localize(nameId);
        public int MinPlayers => minPlayers;
        public int MaxPlayers => Mathf.Min(maxPlayers, NetworkPlayersManager.Instance.NumPlayers);
        public int WaitTime => waitTime;
        public bool EnablePVP => enablePVP;


        public bool InPad => MinigameManager.Instance.CurrentPad == pad;
        public bool InMinigame => MinigameManager.Instance.CurrentMinigame == this;


        public NetworkVariable<MinigameStateType> State { get; set; } = new NetworkVariable<MinigameStateType>(MinigameStateType.WaitingForPlayers);

        public NetworkVariable<int> WaitingPlayers { get; set; } = new NetworkVariable<int>(0);

        public NetworkVariable<int> WaitTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> BuildTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> StartTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> PlayTimeLeft { get; set; } = new NetworkVariable<int>(-1);

        public NetworkVariable<bool> IsPadVisible { get; set; } = new NetworkVariable<bool>(true);
        public NetworkVariable<bool> IsZoneVisible { get; set; } = new NetworkVariable<bool>(false);

        public NetworkList<Score> Scoreboard { get; set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            Scoreboard = new NetworkList<Score>();
        }
        protected virtual void Start()
        {
            if (IsClient)
            {
                BuildTimeLeft.OnValueChanged += OnBuildTimeLeftChanged;
                StartTimeLeft.OnValueChanged += OnStartTimeLeftChanged;
                PlayTimeLeft.OnValueChanged += OnPlayTimeLeftChanged;

                Scoreboard.OnListChanged += OnScoreboardChanged;


                IsPadVisible.OnValueChanged += OnIsPadVisibleChanged;
                IsZoneVisible.OnValueChanged += OnIsZoneVisibleChanged;

                OnIsPadVisibleChanged(default, IsPadVisible.Value);
                OnIsZoneVisibleChanged(default, IsZoneVisible.Value);
            }

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

                Setup();

                StartCoroutine(MinigameRoutine());
            }
        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            if (IsServer && NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
        }

        protected virtual void Setup()
        {
            states.Add(waitingForPlayers = new MinigameState(MinigameStateType.WaitingForPlayers, WaitingForPlayersRoutine));
            states.Add(introducing = new MinigameState(MinigameStateType.Introducing, IntroducingRoutine));
            states.Add(building = new MinigameState(MinigameStateType.Building, BuildingRoutine));
            states.Add(starting = new MinigameState(MinigameStateType.Starting, StartingRoutine));
            states.Add(playing = new MinigameState(MinigameStateType.Playing, PlayingRoutine));
            states.Add(completing = new MinigameState(MinigameStateType.Completing, CompletingRoutine));
        }
        private IEnumerator MinigameRoutine()
        {
            while (true)
            {
                foreach (MinigameState state in states)
                {
                    State.Value = state.type;

                    state.onEnter?.Invoke();
                    yield return state.onUpdate();
                    state.onExit?.Invoke();
                }
            }
        }

        #region Waiting For Players
        private IEnumerator WaitingForPlayersRoutine()
        {
            WaitTimeLeft.Value = waitTime;

            // Wait For Players
            while (WaitTimeLeft.Value > 0)
            {
                WaitingPlayers.Value = players.Count;

                if ((WaitingPlayers.Value >= MinPlayers) && (WaitingPlayers.Value <= MaxPlayers))
                {
                    yield return new WaitForSeconds(1f);
                    WaitTimeLeft.Value--;
                }
                else
                {
                    yield return null;
                    WaitTimeLeft.Value = waitTime;
                }
            }

            // Hide Pad
            IsPadVisible.Value = false;

            // Setup
            SetupClientRpc(NetworkUtils.SendTo(players.ToArray()));
        }

        public void SignMeUp(bool isSignUp)
        {
            SignUpServerRpc(NetworkManager.Singleton.LocalClientId, isSignUp);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SignUpServerRpc(ulong clientId, bool isSignUp)
        {
            if (WaitTimeLeft.Value <= 0) return;

            if (isSignUp)
            {
                if (WaitingPlayers.Value == 0)
                {
                    RequestClientRpc(NetworkHostManager.Instance.Players[clientId].username);
                }

                players.Add(clientId);
            }
            else
            {
                players.Remove(clientId);
            }
        }

        [ClientRpc]
        private void SetupClientRpc(ClientRpcParams clientRpcParams = default)
        {
            OnClientSetup();
        }

        [ClientRpc]
        private void RequestClientRpc(string playerName)
        {
            NotificationsManager.Notify(icon, LocalizationUtility.Localize("minigame_request_title", playerName, LocalizationUtility.Localize(nameId)), LocalizationUtility.Localize("minigame_request_description"), delegate
            {
                if (!EditorManager.Instance.IsEditing && !MinigameManager.Instance.CurrentMinigame && !MinigameManager.Instance.CurrentPad)
                {
                    Player.Instance.Mover.Teleport(pad.transform.position, Player.Instance.transform.rotation, true);
                }
            }, 0.75f);
        }

        private void OnIsZoneVisibleChanged(bool oldV, bool newV)
        {
            zone.gameObject.SetActive(newV);
        }
        private void OnIsPadVisibleChanged(bool oldV, bool newV)
        {
            pad.gameObject.SetActive(newV);
        }

        protected virtual void OnClientSetup()
        {
            MinigameManager.Instance.CurrentMinigame = this;

            NotificationsManager.Instance.IsHidden = true;
        }
        #endregion

        #region Introducing
        private IEnumerator IntroducingRoutine()
        {
            BeginCinematicClientRpc();

            yield return new WaitForSeconds(0.5f);
            OnCinematic();
            yield return new WaitForSeconds((float)cinematic.Director.duration - 1f);
        }

        protected virtual void OnCinematic()
        {
            this.Invoke(ShowBounds, showBoundsTime);
        }

        [ClientRpc]
        protected void BeginCinematicClientRpc()
        {
            if (InMinigame)
            {
                cinematic.OnShow = OnCinematicShow;
                cinematic.OnHide = OnCinematicHide;
                cinematic.Begin();
            }
        }
        private void OnCinematicShow()
        {
            platform.TeleportTo(true, false);
        }
        private void OnCinematicHide()
        {
            if (!IsServer && State.Value != MinigameStateType.Building)
            {
                // Wait until the cinematic has ended for everyone before proceeding (i.e., when the state is building)
                cinematic.Pause();
                this.InvokeUntil(() => State.Value == MinigameStateType.Building, cinematic.Unpause);
            }
        }

        public void ShowBounds()
        {
            IsZoneVisible.Value = true;

            this.InvokeOverTime(delegate (float p)
            {
                zone.SetScale(p, false);
            },
            expandTime);
        }
        #endregion

        #region Building
        private IEnumerator BuildingRoutine()
        {
            SwitchToBuildModeClientRpc();

            BuildTimeLeft.Value = buildTime;
            while (BuildTimeLeft.Value > 0)
            {
                yield return new WaitForSeconds(1f);
                BuildTimeLeft.Value--;
            }
        }

        [ClientRpc]
        private void SwitchToBuildModeClientRpc()
        {
            if (InMinigame)
            {
                EditorManager.Instance.SetMode(EditorManager.EditorMode.Build, true);

                OnApplyRestrictions();
                if (!EditorManager.Instance.CanLoadCreature(Player.Instance.Constructor.Data, out string errorTitle, out string errorMessage))
                {
                    CreatureData creatureData = null;
                    if (creaturePreset != null)
                    {
                        creatureData = JsonUtility.FromJson<CreatureData>(creaturePreset.text);
                    }
                    EditorManager.Instance.Load(creatureData);
                }
                EditorManager.Instance.ClearHistory();
                EditorManager.Instance.TakeSnapshot(Change.Load, false);

                EditorManager.Instance.UpdateLoadableCreatures();
            }
        }

        protected virtual void OnApplyRestrictions()
        {
        }

        private void OnBuildTimeLeftChanged(int oldTime, int newTime)
        {
            if (InMinigame)
            {
                bool isInteractable = newTime <= 0;
                string text = !isInteractable ? newTime.ToString() : LocalizationUtility.Localize("cc_pagination_play");

                MinigameManager.Instance.SetPlay(text, isInteractable);
            }
        }
        #endregion

        #region Starting
        private IEnumerator StartingRoutine()
        {
            // Setup the scoreboard
            OnSetupScoreboard();

            // Switch everyone to play mode
            SwitchToPlayModeClientRpc();
            yield return new WaitForSeconds(switchTime);

            // Teleport everyone to their starting positions
            TeleportToStartClientRpc();
            yield return new WaitForSeconds(teleportTime);

            // Countdown to start the minigame
            StartTimeLeft.Value = startTime;
            while (StartTimeLeft.Value > 0)
            {
                yield return new WaitForSeconds(1f);
                StartTimeLeft.Value--;
            }

            // Start minigame
            StartClientRpc();
        }

        [ClientRpc]
        private void SwitchToPlayModeClientRpc()
        {
            if (InMinigame)
            {
                EditorManager.Instance.SetMode(EditorManager.EditorMode.Play);

                Player.Instance.Mover.FreezeMove = Player.Instance.Mover.FreezeTurn = true;
                Player.Instance.Abilities.enabled = false;

                MinigameManager.Instance.SetTitle(LocalizationUtility.Localize(objectiveId));

                MusicManager.Instance.FadeTo(musicId);
            }
        }

        [ClientRpc]
        private void TeleportToStartClientRpc()
        {
            if (InMinigame)
            {
                Transform spawnPoint = GetSpawnPoint();
                Player.Instance.Mover.Teleport(spawnPoint.position, spawnPoint.rotation, true);
            }
        }

        [ClientRpc]
        private void StartClientRpc()
        {
            if (InMinigame)
            {
                Player.Instance.Mover.FreezeMove = Player.Instance.Mover.FreezeTurn = false;
                Player.Instance.Abilities.enabled = true;

                MinigameManager.Instance.SetTitle(null);
            }
            else
            {
                NotificationsManager.Notify(LocalizationUtility.Localize("minigame_notify-started", Name));
            }
        }

        public abstract Transform GetSpawnPoint();
        public abstract void OnSetupScoreboard();

        private void OnStartTimeLeftChanged(int oldTime, int newTime)
        {
            if (InMinigame)
            {
                if (newTime == 3)
                {
                    audioSource.PlayOneShot(countdownFX);
                }

                if (newTime > 0)
                {
                    Color color = (newTime <= 3) ? Color.red : Color.white;

                    MinigameManager.Instance.SetSubtitle(newTime.ToString(), color);
                }
                else
                {
                    MinigameManager.Instance.SetSubtitle(null, Color.white);
                }
            }
        }
        private void OnScoreboardChanged(NetworkListEvent<Score> changeEvent)
        {
            if (InMinigame)
            {
                Score score = changeEvent.Value;

                string id = score.id.ToString();
                string displayName = score.displayName.ToString();
                int value = score.score;

                switch (changeEvent.Type)
                {
                    case NetworkListEvent<Score>.EventType.Add:
                        MinigameManager.Instance.Scoreboard.Add(id, displayName, value);
                        break;

                    case NetworkListEvent<Score>.EventType.Value:
                        MinigameManager.Instance.Scoreboard.Set(id, value);
                        MinigameManager.Instance.Scoreboard.Sort(isAscendingOrder);
                        break;

                    case NetworkListEvent<Score>.EventType.Clear:
                        MinigameManager.Instance.Scoreboard.Clear();
                        break;
                }
            }
        }
        #endregion

        #region Playing
        private IEnumerator PlayingRoutine()
        {
            SetupPlayerCorpses(spawnCorpses);

            yield return new WaitAny(this, TimerRoutine(), GameplayLogicRoutine());
            PlayTimeLeft.Value = 0;

            SetupPlayerCorpses(true);
        }

        private IEnumerator TimerRoutine()
        {
            if (playTime == -1)
            {
                yield return new WaitUntil(() => false);
            }

            PlayTimeLeft.Value = playTime;
            while (PlayTimeLeft.Value > 0)
            {
                yield return new WaitForSeconds(1f);
                PlayTimeLeft.Value--;
            }
        }
        protected virtual IEnumerator GameplayLogicRoutine()
        {
            yield return new WaitUntil(() => false);
        }

        private void OnPlayTimeLeftChanged(int oldTime, int newTime)
        {
            if (InMinigame)
            {
                if (newTime == 0)
                {
                    audioSource.PlayOneShot(whistleFX);
                }

                if (newTime > 0)
                {
                    string time = FormatTime(newTime);
                    Color color = (newTime <= 10) ? Color.red : Color.white;

                    MinigameManager.Instance.SetSubtitle(time, color);
                }
                else
                {
                    MinigameManager.Instance.SetSubtitle(null, Color.white);
                }
            }
        }
        private string FormatTime(int seconds)
        {
            int mins = seconds / 60;
            int secs = seconds % 60;
            return $"{mins:00}:{secs:00}";
        }

        protected void SetupPlayerCorpses(bool isActive)
        {
            foreach (ulong clientId in players)
            {
                SetupCorpse(NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId).GetComponent<CreatureBase>(), isActive);
            }
        }
        protected void SetupCorpse(CreatureBase creature, bool isActive)
        {
            creature.Corpse.GenerateRagdoll.Value = isActive;
        }
        #endregion

        #region Completing
        private IEnumerator CompletingRoutine()
        {
            yield return new WaitForSeconds(1f);

            // Notify Of Winners
            NotifyOfWinnerClientRpc(GetWinnerName());

            // Celebrate
            ulong[] winnerClientIds = GetWinnerClientIds().ToArray();
            if (winnerClientIds.Length > 0)
            {
                LoseClientRpc(NetworkUtils.DontSendTo(winnerClientIds));
                WinClientRpc(NetworkUtils.SendTo(winnerClientIds));

                Coroutine spawnFireworksCoroutine = StartCoroutine(SpawnFireworksRoutine(winnerClientIds));
                yield return new WaitForSeconds(celebrateTime);
                StopCoroutine(spawnFireworksCoroutine);
            }
            else
            {
                LoseClientRpc();
                yield return new WaitForSeconds(completeTime);
            }

            // Shutdown
            OnServerShutdown();
            ShutdownClientRpc();
        }
        private IEnumerator SpawnFireworksRoutine(ulong[] winnerClientIds)
        {
            List<Transform> winnerTransforms = new List<Transform>();
            foreach (ulong clientId in winnerClientIds)
            {
                NetworkObject winner = NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId);
                if (winner != null)
                {
                    winnerTransforms.Add(winner.transform);
                }
            }

            while (true)
            {
                foreach (Transform winner in winnerTransforms)
                {
                    if (winner != null)
                    {
                        SpawnRandomFireworkClientRpc(winner.position);
                    }
                    yield return new WaitForSeconds(minMaxFireworksCooldown.Random);
                }
            }
        }

        protected abstract List<ulong> GetWinnerClientIds();
        protected abstract string GetWinnerName();

        [ClientRpc]
        private void NotifyOfWinnerClientRpc(string winnerName)
        {
            if (InMinigame)
            {
                MinigameManager.Instance.SetTitle(LocalizationUtility.Localize("minigame_notify-winner_internal", winnerName));
            }
            else
            {
                NotificationsManager.Notify(LocalizationUtility.Localize("minigame_notify-winner_external", winnerName, Name));
            }
        }

        [ClientRpc]
        private void WinClientRpc(ClientRpcParams sendTo = default)
        {
            if (InMinigame)
            {
                OnCompleted();
                StatsManager.Instance.MinigamesWon++;
            }
        }

        [ClientRpc]
        private void LoseClientRpc(ClientRpcParams sendTo = default)
        {
            if (InMinigame)
            {
                OnCompleted();
            }
        }

        [ClientRpc]
        private void SpawnRandomFireworkClientRpc(Vector3 position)
        {
            Instantiate(fireworksPrefabs[UnityEngine.Random.Range(0, fireworksPrefabs.Length)], position, Quaternion.identity, Dynamic.Transform);
        }

        [ClientRpc]
        private void ShutdownClientRpc()
        {
            OnClientShutdown();
        }

        protected virtual void OnCompleted()
        {
            MusicManager.Instance.FadeTo(null);

            ProgressManager.Data.Experience += experience;
            ProgressManager.Instance.Save();

            StatsManager.Instance.ExperienceEarned += experience;

            NotificationsManager.Notify(LocalizationUtility.Localize("experience-earned", experience));
        }

        protected virtual void OnServerShutdown()
        {
            Scoreboard.Clear();
            players.Clear();

            zone.SetScale(0f, true);

            WaitingPlayers.Value = 0;
            IsZoneVisible.Value = false;
            IsPadVisible.Value = true;
        }
        protected virtual void OnClientShutdown()
        {
            if (InMinigame)
            {
                StatsManager.Instance.MinigamesCompleted++;

                EditorManager.Instance.ResetRestrictions();

                MinigameManager.Instance.SetTitle(null);
                MinigameManager.Instance.SetSubtitle(null, Color.white);
                MinigameManager.Instance.Scoreboard.Clear();

                MinigameManager.Instance.CurrentMinigame = null;

                NotificationsManager.Instance.IsHidden = false;

                MusicManager.Instance.FadeTo(SettingsManager.Data.InGameMusicId);
            }
        }
        #endregion

        #region Other
        private void OnClientDisconnectCallback(ulong clientId)
        {
            players.Remove(clientId);
        }
        #endregion
        #endregion
        
        #region Enums
        public enum MinigameStateType
        {
            WaitingForPlayers,
            Introducing,
            Building,
            Starting,
            Playing,
            Completing
        }
        #endregion

        #region Nested
        public class MinigameState
        {
            public MinigameStateType type;

            public Func<IEnumerator> onUpdate;
            public Action onEnter;
            public Action onExit;

            public MinigameState(MinigameStateType type, Func<IEnumerator> onUpdate)
            {
                this.type = type;
                this.onUpdate = onUpdate;
            }
        }

        public struct Score : INetworkSerializable, IEquatable<Score>
        {
            public FixedString32Bytes id;
            public FixedString32Bytes displayName;
            public int score;

            public Score(string id, string displayName, int score)
            {
                this.id = id;
                this.displayName = displayName;
                this.score = score;
            }

            public Score(Score s, int score)
            {
                id = s.id;
                displayName = s.displayName;
                this.score = score;
            }

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref id);
                serializer.SerializeValue(ref displayName);
                serializer.SerializeValue(ref score);
            }

            public bool Equals(Score other)
            {
                return id == other.id && displayName == other.displayName && score == other.score;
            }
        }
        #endregion
    }
}