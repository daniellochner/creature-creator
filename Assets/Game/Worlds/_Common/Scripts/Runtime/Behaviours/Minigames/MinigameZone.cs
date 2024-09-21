using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameZone : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Minigame minigame;
        [Space]
        [SerializeField] private NetworkTransform boundsNT;
        [SerializeField] private GameObject electricShockPrefab;
        [SerializeField] private float speed;
        [SerializeField] private MeshRenderer[] renderersUp;
        [SerializeField] private MeshRenderer[] renderersDown;
        [SerializeField] private Transform[] spectatorSpawnPoints;

        private Material boundsMatUp;
        private Material boundsMatDown;
        #endregion

        #region Methods
        private void Start()
        {
            foreach (MeshRenderer renderer in renderersUp)
            {
                if (boundsMatUp == null)
                {
                    boundsMatUp = new Material(renderer.sharedMaterial);
                }
                renderer.material = boundsMatUp;
            }
            foreach (MeshRenderer renderer in renderersDown)
            {
                if (boundsMatDown == null)
                {
                    boundsMatDown = new Material(renderer.sharedMaterial);
                }
                renderer.material = boundsMatDown;
            }
        }
        private void Update()
        {
            Vector2 dir = Vector2.up * speed * Time.deltaTime;
            if (boundsMatUp != null)
            {
                boundsMatUp.mainTextureOffset -= dir;
            }
            if (boundsMatDown != null)
            {
                boundsMatDown.mainTextureOffset += dir;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsServer && minigame.State.Value != Minigame.MinigameStateType.Introducing)
            {
                CreatureBase creature = collision.gameObject.GetComponent<CreatureBase>();
                if (creature != null && !creature.Health.IsDead)
                {
                    creature.Health.Kill(DamageReason.ElectricShock);
                    SpawnShockAtPointClientRpc(collision.GetContact(0).point);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!minigame.InMinigame)
            {
                CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
                if (player != null)
                {
                    Transform spawnPoint = spectatorSpawnPoints[Random.Range(0, spectatorSpawnPoints.Length)];
                    player.Mover.Teleport(spawnPoint.position, spawnPoint.rotation, true, false);
                }
            }
        }

        [ClientRpc]
        private void SpawnShockAtPointClientRpc(Vector3 point)
        {
            Instantiate(electricShockPrefab, point, Quaternion.identity, Dynamic.Transform);
        }

        public void SetScale(float scale, bool instant)
        {
            Vector3 localScale = new Vector3(scale, 1f, scale);
            if (instant)
            {
                boundsNT.Teleport(boundsNT.transform.position, boundsNT.transform.rotation, localScale);
            }
            else
            {
                boundsNT.transform.localScale = localScale;
            }
        }
        #endregion
    }
}