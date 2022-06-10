// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class NetworkCreaturePlayer : NetworkCreature
    {
        #region Fields
        [SerializeField] private Player player;
        #endregion

        #region Properties
        public Player Player => player;
        public CreatureSourcePlayer SourcePlayerCreature => SourceCreature as CreatureSourcePlayer;

        public override CreatureTargetBase TargetCreature => IsOWNER ? SourcePlayerCreature : base.TargetCreature;
        #endregion

        #region Methods
        public override void Setup(bool isOwner)
        {
            base.Setup(isOwner);

            if (isOwner && player.Creature.Mover.RequestToMove)
            {
                player.Creature.Mover.OnTurnRequest += RequestToTurn;
                player.Creature.Mover.OnMoveRequest += RequestToMove;
            }
        }

        #region Load Player
        [ClientRpc]
        public void LoadPlayerClientRpc(PlayerData playerData, string creatureData, ClientRpcParams clientRpcParams)
        {
            NetworkCreaturesMenu.Instance.AddPlayer(playerData);
            if (!IsHidden.Value)
            {
                ReconstructAndShowClientRpc(creatureData);
            }
        }
        #endregion

        #region Move
        [ServerRpc]
        private void RequestToMoveServerRpc(Vector3 direction, ulong clientId)
        {
            RequestToMoveClientRpc(direction, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void RequestToMoveClientRpc(Vector3 direction, ClientRpcParams clientRpcParams)
        {
            player.Creature.Mover.Move(direction);
        }
        private void RequestToMove(Vector3 direction)
        {
            RequestToMoveServerRpc(direction, OwnerClientId);
        }
        #endregion

        #region Turn
        [ServerRpc]
        private void RequestToTurnServerRpc(float angle, ulong clientId)
        {
            RequestToTurnClientRpc(angle, NetworkUtils.SendTo(clientId));
        }
        [ClientRpc]
        private void RequestToTurnClientRpc(float angle, ClientRpcParams clientRpcParams)
        {
            player.Creature.Mover.Turn(angle);
        }
        private void RequestToTurn(float angle)
        {
            RequestToTurnServerRpc(angle, OwnerClientId);
        }
        #endregion
        #endregion
    }
}