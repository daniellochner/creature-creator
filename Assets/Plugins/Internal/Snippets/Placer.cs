using UnityEngine;

namespace DanielLochner.Assets
{
    public class Placer : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject prefab;
        [SerializeField] private int count;
        [SerializeField] private bool alignToSurface;

        [Header("Position")]
        [SerializeField] private float spacing = 1f;
        [SerializeField] private MinMax minMaxPosOffset;

        [Header("Rotation")]
        [SerializeField] private Vector3 defaultRotation;
        [SerializeField] private Vector3Int normalAlignment = Vector3Int.one;
        [SerializeField] private MinMax minMaxRotOffset;

        [Header("Scale")]
        [SerializeField] private MinMax minMaxScale = new MinMax(1f, 1f);

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
        public bool AlignToSurface
        {
            get => alignToSurface;
            set => alignToSurface = value;
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

        public Vector3 DefaultRotation
        {
            get => defaultRotation;
            set => defaultRotation = value;
        }
        public MinMax MinMaxRotOffset
        {
            get => minMaxRotOffset;
            set => minMaxRotOffset = value;
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
                GameObject obj = Instantiate(prefab, transform, false);
                obj.name = prefab.name;

                Vector3 origin = transform.position + transform.forward * (spacing * i + minMaxPosOffset.Random);
                obj.transform.position = origin;
                if (alignToSurface && Physics.Raycast(origin, -transform.up, out RaycastHit hitInfo))
                {
                    obj.transform.position = hitInfo.point;
                    obj.transform.up = hitInfo.normal.Multiply(transform.TransformDirection(normalAlignment));
                }
                obj.transform.localScale *= minMaxScale.Random;
                obj.transform.localRotation *= Quaternion.Euler(defaultRotation) * Quaternion.Euler(0f, minMaxRotOffset.Random, 0f);
            }
        }
        #endregion
    }
}