// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class UnlockableBodyPart : MonoBehaviour
    {
        #region Fields
        [SerializeField] private string bodyPartID;
        [SerializeField] private bool isUnlockable = true;

        [Header("Setup")]
        [SerializeField] private Database bodyParts;
        [SerializeField] private Material lockedMat;
        #endregion

        #region Properties
        public BodyPart BodyPart => DatabaseManager.GetDatabaseEntry<BodyPart>("Body Parts", bodyPartID);
        #endregion

        #region Methods
        private void Start()
        {
            if (ProgressManager.Data.UnlockedBodyParts.Contains(bodyPartID))
            {
                gameObject.SetActive(false);
            }
        }

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            BodyPart bodyPart = bodyParts.Objects[bodyPartID] as BodyPart;

            GameObject bp = Instantiate(bodyPart.Prefab.constructible, transform);

            BodyPartConstructor bpc = bp.GetComponent<BodyPartConstructor>();
            Renderer renderer = bpc.Model.GetComponentInChildren<Renderer>();

            Material[] mats = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = lockedMat;
            }
            renderer.sharedMaterials = mats;
        }

        [ContextMenu("Align")]
        public void Align()
        {
            BodyPartConstructor bpc = transform.GetChild(0).GetComponent<BodyPartConstructor>();

            Renderer r = bpc.Model.GetComponentInChildren<Renderer>();

            Mesh mesh = null;
            SkinnedMeshRenderer smr = r.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                mesh = smr.sharedMesh;
            }
            else
            {
                mesh = r.GetComponent<MeshFilter>().sharedMesh;
            }

            Vector2 avgXZ = Vector2.zero;
            float minY = Mathf.Infinity;

            foreach (Vector3 vertex in mesh.vertices)
            {
                Vector3 worldVertex = r.transform.L2WSpace(vertex);

                if (worldVertex.y < minY)
                {
                    minY = worldVertex.y;
                }
                avgXZ += new Vector2(worldVertex.x, worldVertex.z);
            }
            avgXZ /= mesh.vertexCount;

            Vector3 point = new Vector3(avgXZ.x, minY, avgXZ.y);
            Vector3 offset = point - bpc.transform.position;

            bpc.transform.position -= offset;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isUnlockable)
            {
                EditorManager.Instance.UnlockBodyPart(bodyPartID);
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}