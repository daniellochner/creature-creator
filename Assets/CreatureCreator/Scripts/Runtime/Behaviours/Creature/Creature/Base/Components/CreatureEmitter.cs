// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureEmitter : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<string, Emission> emissions;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }

        public Dictionary<string, Emission> Emitting { get; set; } = new Dictionary<string, Emission>();
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        public void Setup()
        {
            Constructor.OnAddBodyPartPrefab += delegate (GameObject main, GameObject flipped)
            {
                BodyPartEmitter mainBPE = main.GetComponent<BodyPartEmitter>();
                mainBPE?.Setup(this);

                BodyPartEmitter flippedBPE = flipped.GetComponent<BodyPartEmitter>();
                flippedBPE?.SetFlipped(mainBPE);
                flippedBPE?.Setup(this);
            };
        }

        public void EmitFrom(BodyPartEmitter emitter, float duration)
        {
            EmitFromServerRpc(emitter.name, duration);
        }
        [ServerRpc]
        private void EmitFromServerRpc(string emitterGUID, float duration)
        {
            if (TryGetEmitter(emitterGUID, out BodyPartEmitter emitter))
            {
                Emission emission = Instantiate(emissions[emitter.EmissionId], emitter.SpawnPoint.position, emitter.SpawnPoint.rotation, Dynamic.Transform);
                emission.Emit(emitter, duration);
                emission.NetworkObject.SpawnWithOwnership(OwnerClientId, true);
            }
        }
        private bool TryGetEmitter(string emitterGUID, out BodyPartEmitter emitter)
        {
            foreach (BodyPartConstructor constructor in Constructor.BodyParts)
            {
                if (constructor.name == emitterGUID && constructor.IsVisible)
                {
                    emitter = constructor.GetComponent<BodyPartEmitter>();
                    return true;
                }
                else
                if (constructor.Flipped.name == emitterGUID && constructor.Flipped.IsVisible)
                {
                    emitter = constructor.Flipped.GetComponent<BodyPartEmitter>();
                    return true;
                }
            }
            emitter = null;
            return false;
        }

        public void StopEmittingFrom(BodyPartEmitter emitter)
        {
            StopEmittingFromServerRpc(emitter.name);
        }
        [ServerRpc]
        private void StopEmittingFromServerRpc(string emitterGUID)
        {
            if (Emitting.TryGetValue(emitterGUID, out Emission emission))
            {
                emission?.StopEmitting();
            }
        }

        public void StopEmitting()
        {
            StopEmittingServerRpc();
        }
        [ServerRpc]
        private void StopEmittingServerRpc()
        {
            foreach (Emission emission in (new List<Emission>(Emitting.Values)))
            {
                emission.StopEmitting();
            }
        }
        #endregion
    }
}