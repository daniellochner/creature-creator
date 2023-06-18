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
        [SerializeField] private Material boundsMat;
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
            boundsMatUp   = new Material(boundsMat);
            boundsMatDown = new Material(boundsMat);

            foreach (MeshRenderer renderer in renderersUp)
            {
                renderer.material = boundsMatUp;
            }
            foreach (MeshRenderer renderer in renderersDown)
            {
                renderer.material = boundsMatDown;
            }
        }
        private void Update()
        {
            HandleMat();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.IsPlayer() && minigame.InMinigame)
            {
                Player.Instance.Health.Kill(DamageReason.MinigameZone);
                SpawnElectricShockClientRpc(collision.GetContact(0).point);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsPlayer() && !minigame.InMinigame)
            {
                Transform spawnPoint = spectatorSpawnPoints[Random.Range(0, spectatorSpawnPoints.Length)];
                Player.Instance.Mover.Teleport(spawnPoint.position, spawnPoint.rotation, true);
            }
        }

        [ClientRpc]
        private void SpawnElectricShockClientRpc(Vector3 position)
        {
            Instantiate(electricShockPrefab, position, Quaternion.identity, Dynamic.Transform);
        }

        private void HandleMat()
        {
            Vector2 dir = Vector2.up * speed * Time.deltaTime;
            boundsMatUp.mainTextureOffset -= dir;
            boundsMatDown.mainTextureOffset += dir;
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