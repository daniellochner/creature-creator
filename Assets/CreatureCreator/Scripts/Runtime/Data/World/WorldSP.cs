// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldSP : World
    {
        #region Methods
        public WorldSP(string mapName, bool creativeMode, bool spawnNPC, bool enablePVE)
        {
            MapName = mapName;
            CreativeMode = creativeMode;
            SpawnNPC = spawnNPC;
            EnablePVE = enablePVE;
        }
        #endregion
    }
}