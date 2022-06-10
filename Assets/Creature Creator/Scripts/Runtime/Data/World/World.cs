// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Services.Lobbies.Models;

namespace DanielLochner.Assets.CreatureCreator
{
    public class World
    {
        #region Properties
        public bool IsPrivate { get; private set; }
        public string JoinCode { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsPasswordProtected { get; private set; }
        public string WorldName { get; private set; }
        public string MapName { get; private set; }
        public string Version { get; private set; }
        public bool AllowPVP { get; private set; }
        public bool AllowPVE { get; private set; }
        public bool SpawnNPC { get; private set; }
        #endregion

        #region Methods
        public World(Lobby lobby)
        {
            IsPrivate = bool.Parse(lobby.Data["isPrivate"].Value);
            JoinCode = lobby.Data["joinCode"].Value;
            PasswordHash = lobby.Data["passwordHash"].Value;
            IsPasswordProtected = !string.IsNullOrEmpty(PasswordHash);

            WorldName = lobby.Name;
            MapName = lobby.Data["mapName"].Value;
            Version = lobby.Data["version"].Value;
            AllowPVP = bool.Parse(lobby.Data["allowPVP"].Value);
            AllowPVE = bool.Parse(lobby.Data["allowPVE"].Value);
            SpawnNPC = bool.Parse(lobby.Data["spawnNPC"].Value);
        }
        #endregion
    }
}