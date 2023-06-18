using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class FoodSpawner : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private NetworkObject[] foodPrefabs;
        [SerializeField] private MinMax spawnCooldown;

        private NetworkObject food;
        private float spawnTimeLeft;
        #endregion

        #region Methods
        private void Update()
        {
            if (IsServer && !food)
            {
                TimerUtility.OnTimer(ref spawnTimeLeft, spawnCooldown.Random, Time.deltaTime, Spawn);
            }
        }

        public void Spawn()
        {
            food = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)], transform.position, transform.rotation);
            food.Spawn(true);
        }
        public void Despawn()
        {
            food.Despawn();
            spawnTimeLeft = 0;
        }
        #endregion
    }
}