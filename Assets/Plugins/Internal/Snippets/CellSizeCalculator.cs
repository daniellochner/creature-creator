using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class CellSizeCalculator : MonoBehaviour
    {
        [SerializeField] private int numberOfColumns = 1;
        [SerializeField] private float aspectRatio = 1f;

        private GridLayoutGroup gridLayoutGroup;
        private RectTransform rectTransform;

        public int NumberOfColumns { get { return numberOfColumns; } set { numberOfColumns = value; } }

        public void Initialize()
        {
            if (gridLayoutGroup == null || rectTransform == null)
            {
                gridLayoutGroup = GetComponent<GridLayoutGroup>();
                rectTransform = GetComponent<RectTransform>();
            }

            float cellWidth = (rectTransform.rect.width - (gridLayoutGroup.spacing.x * (numberOfColumns - 1)) - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right) / numberOfColumns;
            float cellHeight = cellWidth / aspectRatio;

            gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
        }

        private void Start()
        {
            Initialize();
        }
    }
}