using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Orchard : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject[] trees;
        
        [Header("Settings")]
        [SerializeField] private int count;
        [SerializeField] private float spacing;
        [SerializeField] private MinMax minMaxPosOffset;
        [SerializeField] private MinMax minMaxRotOffset;
        [SerializeField] private MinMax minMaxScale;
        [Space]
        [SerializeField, Button("Setup")] private bool setup;
        #endregion

        #region Methods
        public void Setup()
        {
            transform.DestroyChildren();

            for (int i = 0; i < trees.Length; i++)
            {
                GameObject tree = trees[i];

                GameObject root = new GameObject(tree.name);
                root.transform.parent = transform;
                root.transform.localPosition = Vector3.right * (spacing * i);

                Placer placer = root.AddComponent<Placer>();
                placer.Prefab = tree;
                placer.AlignToSurface = true;
                placer.NormalAlignment = Vector3Int.zero;
                placer.Count = count;
                placer.Spacing = spacing;
                placer.MinMaxPosOffset = minMaxPosOffset;
                placer.MinMaxRotOffset = minMaxRotOffset;
                placer.MinMaxScale = minMaxScale;

                placer.Place();
            }
        }
        #endregion
    }
}