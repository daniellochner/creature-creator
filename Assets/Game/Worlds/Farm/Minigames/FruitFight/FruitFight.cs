using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FruitFight : TeamMinigame
    {
        #region Fields
        [Header("Fruit Fight")]
        [SerializeField] private TrackRegion[] fruitBaskets;
        [SerializeField] private FoodSpawner[] fruitSpawners;
        [SerializeField] private float respawnFruitTime;
        #endregion

        #region Methods
        protected override void Start()
        {
            base.Start();

            if (IsServer)
            {
                for (int i = 0; i < fruitBaskets.Length; i++)
                {
                    int index = i;
                    fruitBaskets[i].OnTrack = fruitBaskets[i].OnLoseTrackOf = delegate
                    {
                        SetTeamScore(index, fruitBaskets[index].tracked.Count);
                    };
                }
            }
        }

        protected override void Setup()
        {
            base.Setup();

            introducing.onEnter += OnIntroducingEnter;
        }

        #region Building
        protected override void OnApplyRestrictions()
        {
            base.OnApplyRestrictions();

            List<string> bodyParts = new List<string>();
            foreach (var obj in DatabaseManager.GetDatabase("Body Parts").Objects)
            {
                BodyPart bodyPart = obj.Value as BodyPart;
                if (bodyPart is Mouth)
                {
                    Mouth mouth = bodyPart as Mouth;
                    if (mouth.Diet != Diet.Carnivore)
                    {
                        bodyParts.Add(obj.Key);
                    }
                }
            }
            EditorManager.Instance.SetRestrictedBodyParts(bodyParts);
        }
        #endregion

        #region Introducing
        private void OnIntroducingEnter()
        {
            DropFruitClientRpc();
        }

        protected override void OnCinematic()
        {
            base.OnCinematic();

            this.Invoke(RespawnFruit, respawnFruitTime);
        }

        private void RespawnFruit()
        {
            foreach (FoodSpawner foodSpawner in fruitSpawners)
            {
                foodSpawner.Despawn();
            }
        }

        [ClientRpc]
        private void DropFruitClientRpc()
        {
            if (InMinigame)
            {
                Player.Instance.Holder.DropAll();
            }
        }
        #endregion
        #endregion
    }
}