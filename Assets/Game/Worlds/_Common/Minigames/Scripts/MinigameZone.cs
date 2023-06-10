using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameZone : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Minigame minigame;
        [SerializeField] private NetworkTransform boundsNT;
        [SerializeField] private Bounds bounds;
        [SerializeField] private Material boundsMat;
        [SerializeField] private AudioSource humAS;
        [SerializeField] private GameObject electricShockPrefab;
        [SerializeField] private float speed;
        [SerializeField] private MeshRenderer[] renderersUp;
        [SerializeField] private MeshRenderer[] renderersDown;

        private Material boundsMatUp;
        private Material boundsMatDown;
        #endregion

        #region Properties
        public Bounds Bounds => bounds;
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
            HandleHum();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CreatureBase creature = collision.gameObject.GetComponent<CreatureBase>();
            if (creature != null)
            {
                creature.Health.TakeDamage(creature.Health.Health);
                SpawnElectricShockClientRpc(collision.GetContact(0).point);
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
        private void HandleHum()
        {
            if (CinematicManager.Instance.IsInCinematic)
            {
                humAS.transform.position = bounds.GetClosestPointOnBounds(CinematicManager.Instance.CurrentCinematic.CinematicCamera.transform.position, true);
            }
            else 
            if (Player.Instance != null && Player.Instance.IsSetup)
            {
                humAS.transform.position = bounds.GetClosestPointOnBounds(Player.Instance.Camera.MainCamera.transform.position, true);
            }

            humAS.gameObject.SetActive(transform.localScale.x > 0.001f);
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