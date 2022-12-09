// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldSP : World
    {
        #region Properties
        public bool Unlimited { get; private set; }
        #endregion

        #region Methods
        public WorldSP(string mapName, bool creativeMode, bool spawnNPC, bool enablePVE, bool unlimited)
        {
            MapName = mapName;
            CreativeMode = creativeMode;
            SpawnNPC = spawnNPC;
            EnablePVE = enablePVE;
            Unlimited = unlimited;
        }
        #endregion
    }
}