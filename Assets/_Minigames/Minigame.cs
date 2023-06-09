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
        [SerializeField] private string nameId;
        [Header("Waiting For Players")]
        [SerializeField] private MinigamePad pad;
        [SerializeField] private int minPlayers;
        [SerializeField] private int maxPlayers;
        [SerializeField] private int waitTime;
        [Header("Introducing")]
        [SerializeField] private MinigameCinematic cinematic;
        [SerializeField] private MinigameZone zone;
        [SerializeField] private float expandTime;
        [SerializeField] private float showBoundsTime;
        [Header("Building")]
        [SerializeField] private Platform platform;
        [SerializeField] private int buildTime;
        [SerializeField] protected TextAsset creaturePreset;
        [Header("Starting")]
        [SerializeField] private string objectiveId;
        [SerializeField] private int startTime;
        [SerializeField] private float teleportTime;
        [SerializeField] private float switchTime;
        [SerializeField] private string musicId;
        [Header("Playing")]
        [SerializeField] private int playTime;
        [SerializeField] protected bool isAscendingOrder;
        [Header("Completing")]
        [SerializeField] private int celebrateTime;
        [SerializeField] private int completeTime;
        [SerializeField] private MinMax minMaxFireworksCooldown;
        [SerializeField] private GameObject[] fireworksPrefabs;

        protected List<MinigameState> states = new List<MinigameState>();
        protected List<ulong> players = new List<ulong>();

        protected MinigameState waitingForPlayers, introducing, building, starting, playing, completing;
        #endregion

        #region Properties
        public string Name => LocalizationUtility.Localize(nameId);

        public int MinPlayers => minPlayers;
        public int MaxPlayers => Mathf.Min(maxPlayers, NetworkPlayersManager.Instance.NumPlayers);

        public int WaitTime => waitTime;

        public bool InMinigame => MinigameManager.Instance.CurrentMinigame == this;


        public NetworkVariable<MinigameStateType> State { get; set; } = new NetworkVariable<MinigameStateType>(MinigameStateType.WaitingForPlayers);

        public NetworkVariable<int> WaitTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> BuildTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> StartTimeLeft { get; set; } = new NetworkVariable<int>(-1);
        public NetworkVariable<int> PlayTimeLeft { get; set; } = new NetworkVariable<int>(-1);

        public NetworkList<Score> Scoreboard { get; set; }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
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

                zone.gameObject.SetActive(State.Value != MinigameStateType.WaitingForPlayers);
                pad.gameObject.SetActive(State.Value == MinigameStateType.WaitingForPlayers);
            }

            if (IsServer)
            {
                Setup();

                StartCoroutine(MinigameRoutine());
            }
        }

        private void OnEnable()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
            }
        }
        private void OnDisable()
        {
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

            while (WaitTimeLeft.Value > 0)
            {
                if ((pad.NumPlayers >= MinPlayers) && (pad.NumPlayers <= MaxPlayers))
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

            foreach (var t in pad.Region.tracked)
            {
                players.Add(t.GetComponent<NetworkObject>().OwnerClientId);
            }
            IncludePlayersClientRpc(NetworkUtils.SendTo(players.ToArray()));

            SetupClientRpc();
        }

        [ClientRpc]
        private void IncludePlayersClientRpc(ClientRpcParams clientRpcParams = default)
        {
            MinigameManager.Instance.CurrentMinigame = this;
            MinigameManager.Instance.Scoreboard.Setup(this);

            NotificationsManager.Instance.IsHidden = true;
        }

        [ClientRpc]
        private void SetupClientRpc()
        {
            zone.gameObject.SetActive(true);
            pad.gameObject.SetActive(false);
        }

        #endregion


        #region Introducing

        private IEnumerator IntroducingRoutine()
        {
            BeginCinematicClientRpc();
            OnCinematic();

            yield return new WaitForSeconds((float)cinematic.Director.duration - 0.5f);
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
                cinematic.Pause();

                this.InvokeUntil(() => State.Value == MinigameStateType.Building, cinematic.Unpause);
            }
        }

        public void ShowBounds()
        {
            ShowBoundsClientRpc();

            this.InvokeOverTime(delegate (float p)
            {
                zone.SetScale(p, false);
            },
            expandTime);
        }

        [ClientRpc]
        private void ShowBoundsClientRpc()
        {
            zone.gameObject.SetActive(true);
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
            Scoreboard.Clear();
            OnSetupScoreboard();

            SwitchToPlayModeClientRpc();
            yield return new WaitForSeconds(switchTime);

            for (int i = 0; i < players.Count; i++)
            {
                TeleportToStartClientRpc();
            }
            yield return new WaitForSeconds(teleportTime);


            StartTimeLeft.Value = startTime;
            while (StartTimeLeft.Value > 0)
            {
                yield return new WaitForSeconds(1f);
                StartTimeLeft.Value--;
            }

            StartClientRpc();
        }

        [ClientRpc]
        private void SwitchToPlayModeClientRpc()
        {
            if (InMinigame)
            {
                EditorManager.Instance.SetMode(EditorManager.EditorMode.Play);

                Player.Instance.Mover.FreezeMove = true;

                MusicManager.Instance.FadeTo(musicId);

                MinigameManager.Instance.SetTitle(LocalizationUtility.Localize(objectiveId));
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
                Player.Instance.Mover.FreezeMove = false;

                MinigameManager.Instance.SetTitle(null);
            }
        }

        public abstract Transform GetSpawnPoint();

        public abstract void OnSetupScoreboard();

        private void OnStartTimeLeftChanged(int oldTime, int newTime)
        {
            if (InMinigame)
            {
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
            yield return new WaitAny(this, TimerRoutine(), GameplayLogicRoutine());
            PlayTimeLeft.Value = 0;
        }

        private IEnumerator TimerRoutine()
        {
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

        #endregion


        #region Completing

        private IEnumerator CompletingRoutine()
        {
            yield return new WaitForSeconds(1f);

            NotifyOfWinnerClientRpc(GetWinnerName());

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

            OnShutdown();
        }
        protected abstract List<ulong> GetWinnerClientIds();

        protected virtual void OnShutdown()
        {
            Scoreboard.Clear();
            zone.SetScale(0f, true);
            players.Clear();

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
        protected abstract string GetWinnerName();

        [ClientRpc]
        private void WinClientRpc(ClientRpcParams sendTo = default)
        {
            if (InMinigame)
            {
                MusicManager.Instance.FadeTo(null);

#if USE_STATS
                StatsManager.Instance.MinigamesWon++;
#endif
            }
        }

        [ClientRpc]
        private void LoseClientRpc(ClientRpcParams sendTo = default)
        {
            if (InMinigame)
            {
                MusicManager.Instance.FadeTo(null);
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
            zone.gameObject.SetActive(false);
            pad.gameObject.SetActive(true);

            if (InMinigame)
            {
                EditorManager.Instance.ResetRestrictions();

                MinigameManager.Instance.SetTitle(null);
                MinigameManager.Instance.Scoreboard.Clear();
                MinigameManager.Instance.CurrentMinigame = null;

                NotificationsManager.Instance.IsHidden = false;

                MusicManager.Instance.FadeTo(SettingsManager.Data.InGameMusicName);
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