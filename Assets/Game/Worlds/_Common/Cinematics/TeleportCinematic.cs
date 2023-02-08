using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TeleportCinematic : CCCinematic
    {
        #region Fields
        [SerializeField] protected Transform spawnPoint;
        #endregion

        #region Properties
        public UnityAction OnTeleport { get; set; }
        #endregion

        #region Methods
        protected CreatureConstructor SpawnCreature(Transform parent, CreatureData data = null)
        {
            CreatureConstructor creature = Player.Instance.Cloner.Clone(data);
            creature.transform.SetZeroParent(parent);

            if (creature.Legs.Count == 0)
            {
                Mesh bodyMesh = new Mesh();
                creature.SkinnedMeshRenderer.BakeMesh(bodyMesh);
                float minY = Mathf.Infinity;
                foreach (Vector3 vertex in bodyMesh.vertices)
                {
                    if (vertex.y < minY)
                    {
                        minY = vertex.y;
                    }
                }
                creature.Body.localPosition = Vector3.up * -minY;
            }

            return creature;
        }
        
        public override void Show()
        {
            base.Show();
            EditorManager.Instance.SetVisibility(false, 0f);
            Player.Instance.Loader.HideFromOthers();
        }
        public override void Hide()
        {
            base.Hide();
            EditorManager.Instance.SetVisibility(true, 0f);
        }
        public void Teleport()
        {
            OnTeleport?.Invoke();
        }
        #endregion
    }
}