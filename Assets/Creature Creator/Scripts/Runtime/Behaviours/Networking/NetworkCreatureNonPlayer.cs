// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NetworkCreatureNonPlayer : NetworkCreature
    {
        #region Properties
        public CreatureSourceNonPlayer SourceNonPlayerCreature => SourceCreature as CreatureSourceNonPlayer;

        public override CreatureTargetBase TargetCreature => IsOWNER ? SourceNonPlayerCreature : base.TargetCreature;
        #endregion

        #region Methods
        #endregion
    }
}