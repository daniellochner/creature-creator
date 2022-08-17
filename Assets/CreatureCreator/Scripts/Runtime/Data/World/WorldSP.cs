// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldSP : World
    {
        #region Methods
        public WorldSP(bool creativeMode, bool spawnNPC, bool enablePVE)
        {
            CreativeMode = creativeMode;
            SpawnNPC = spawnNPC;
            EnablePVE = enablePVE;
        }
        #endregion
    }
}