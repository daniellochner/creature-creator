// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Services.Lobbies.Models;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldMP : World
    {
        #region Properties
        public bool IsPrivate { get; private set; }
        public string JoinCode { get; private set; }
        public string Id { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsPasswordProtected { get; private set; }

        public string WorldName { get; private set; }
        public string MapName { get; private set; }
        public string Version { get; private set; }
        public bool AllowProfanity { get; private set; }
        public bool EnablePVP { get; private set; }
        public bool EnablePVE { get; private set; }
        public bool SpawnNPC { get; private set; }
        #endregion

        #region Methods
        public WorldMP(Lobby lobby)
        {
            Id = lobby.Id;
            IsPrivate = bool.Parse(lobby.Data["isPrivate"].Value);
            JoinCode = lobby.Data["joinCode"].Value;
            PasswordHash = lobby.Data["passwordHash"].Value;
            IsPasswordProtected = !string.IsNullOrEmpty(PasswordHash);

            WorldName = lobby.Name;
            MapName = lobby.Data["mapName"].Value;
            Version = lobby.Data["version"].Value;
            AllowProfanity = bool.Parse(lobby.Data["allowProfanity"].Value);
            EnablePVP = bool.Parse(lobby.Data["enablePVP"].Value);
            EnablePVE = bool.Parse(lobby.Data["enablePVE"].Value);
            SpawnNPC = bool.Parse(lobby.Data["spawnNPC"].Value);

            // TODO: remove this...
            if (lobby.Data.ContainsKey("creativeMode"))
            {
                CreativeMode = bool.Parse(lobby.Data["creativeMode"].Value);
            }
        }
        #endregion
    }
}