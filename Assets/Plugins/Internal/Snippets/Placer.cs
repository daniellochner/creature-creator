using UnityEngine;

namespace DanielLochner.Assets
{
    public class Placer : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject prefab;
        [SerializeField] private int count;

        [Header("Position")]
        [SerializeField] private float spacing = 1f;
        [SerializeField] private MinMax minMaxPosOffset;
        [SerializeField] private bool alignToSurface;

        [Header("Rotation")]
        [SerializeField] private Vector3 defaultRotation;
        [SerializeField] private bool randomizeRotation;
        [SerializeField, DrawIf("randomizeRotation", true)] private MinMax minMaxRotOffset = new MinMax(0f, 360f);

        [Header("Scale")]
        [SerializeField] private bool randomizeScale;
        [SerializeField, DrawIf("randomizeScale", true)] private MinMax minMaxScale = new MinMax(0.9f, 1.1f);

        [SerializeField, Button("Place")] private bool place;
        #endregion

        #region Properties
        public GameObject Prefab
        {
            get => prefab;
            set => prefab = value;
        }
        public int Count
        {
            get => count;
            set => count = value;
        }

        public float Spacing
        {
            get => spacing;
            set => spacing = value;
        }
        public MinMax MinMaxPosOffset
        {
            get => minMaxPosOffset;
            set => minMaxPosOffset = value;
        }
        public bool AlignToSurface
        {
            get => alignToSurface;
            set => alignToSurface = value;
        }

        public Vector3 DefaultRotation
        {
            get => defaultRotation;
            set => defaultRotation = value;
        }
        public bool RandomizeRotation
        {
            get => randomizeRotation;
            set => randomizeRotation = value;
        }
        public MinMax MinMaxRotOffset
        {
            get => minMaxRotOffset;
            set => minMaxRotOffset = value;
        }

        public bool RandomizeScale
        {
            get => randomizeScale;
            set => randomizeScale = value;
        }
        public MinMax MinMaxScale
        {
            get => minMaxScale;
            set => minMaxScale = value;
        }
        #endregion

        #region Methods
        public void Place()
        {
            transform.DestroyChildren();

            for (int i = 0; i < count; i++)
            {
                GameObject prefabGO = Instantiate(prefab, transform, false);
                prefabGO.name = prefab.name;

                // Position
                Vector3 origin = transform.position + transform.forward * (spacing * i + Random.Range(minMaxPosOffset.min, minMaxPosOffset.max));
                if (alignToSurface && Physics.Raycast(origin, -transform.up, out RaycastHit hitInfo))
                {
                    prefabGO.transform.position = hitInfo.point;
                }
                else
                {
                    prefabGO.transform.position = origin;
                }

                // Rotation
                prefabGO.transform.localRotation = Quaternion.Euler(defaultRotation);
                if (randomizeRotation)
                {
                    prefabGO.transform.localRotation *= Quaternion.Euler(0f, Random.Range(minMaxRotOffset.min, minMaxRotOffset.max), 0f);
                }

                // Scale
                if (randomizeScale)
                {
                    prefabGO.transform.localScale *= Random.Range(minMaxScale.min, minMaxScale.max);
                }
            }
            #endregion
        }
    }
}