using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FoodCrateSpawner : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkObject[] foodCratePrefabs;
        [SerializeField] private MinMax spawnCooldown;
        [SerializeField] private bool spawnInCreative;

        private NetworkObject foodCrate;
        private float spawnTimeLeft;
        #endregion

        #region Methods
        private void Update()
        {
            if (IsServer && foodCrate == null && (spawnInCreative || !WorldManager.Instance.World.CreativeMode))
            {
                TimerUtility.OnTimer(ref spawnTimeLeft, spawnCooldown.Random, Time.deltaTime, delegate
                {
                    foodCrate = Instantiate(foodCratePrefabs[Random.Range(0, foodCratePrefabs.Length)], transform.position, transform.rotation);
                    foodCrate.Spawn(true);
                });
            }
        }
        public void Reset()
        {
            foodCrate.Despawn();
            spawnTimeLeft = 0;
        }
        #endregion
    }
}