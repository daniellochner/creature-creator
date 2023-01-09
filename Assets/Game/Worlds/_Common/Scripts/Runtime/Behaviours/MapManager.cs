using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MapManager : MonoBehaviourSingleton<MapManager>
    {
        #region Fields
        [SerializeField] private Vector2Int resolution;
        [SerializeField] private Color backgroundColor;
        [Space]
        [SerializeField] private Menu mapMenu;
        [SerializeField] private SimpleZoom.SimpleZoom mapZoom;
        [SerializeField] private Camera mapCamera;
        [SerializeField] private Follower mapFollower;
        [SerializeField] private RawImage mapRawImage;
        [SerializeField] private RectTransform mapBounds;
        [SerializeField] private Image maskImage;
        #endregion

        #region Properties
        public Camera MapCamera => mapCamera;
        #endregion

        #region Methods
        public void Setup()
        {
            RenderTexture renderTexture = new RenderTexture(resolution.x, resolution.y, 0);
            mapCamera.targetTexture = renderTexture;
            mapRawImage.texture = renderTexture;

            maskImage.color = backgroundColor;

            mapFollower.SetFollow(Player.Instance.transform, true);
            mapZoom.IsActive = false;

            mapMenu.OnOpen += OnMapOpen;
            mapMenu.OnClose += OnMapClose;
        }

        private void Update()
        {
            HandleClick();
        }

        private void HandleClick()
        {
            if (mapZoom.IsActive && Input.GetMouseButtonDown(0))
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(mapZoom.Viewport, Input.mousePosition))
                {
                    return;
                }

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapBounds, Input.mousePosition, null, out Vector2 coords))
                {
                    Vector2 r = mapBounds.rect.size / 2f;

                    float x = Mathf.InverseLerp(-r.x, r.x, coords.x);
                    float y = Mathf.InverseLerp(-r.y, r.y, coords.y);
                    Vector2 n = new Vector2(x, y);

                    if (Physics.Raycast(mapCamera.ViewportPointToRay(n), out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Map")))
                    {
                        MapIcon mapIcon = hitInfo.collider.GetComponent<MapIcon>();
                        if (mapIcon != null)
                        {
                            mapIcon.Click();
                        }
                    }
                }
            }
        }

        private void OnMapOpen()
        {
            mapZoom.IsActive = true;
            mapZoom.SetZoom(1f, 0f, true);

            mapFollower.SetFollow(transform, true);
        }
        private void OnMapClose()
        {
            mapFollower.SetFollow(Player.Instance.transform, true);

            mapZoom.IsActive = false;

            mapZoom.SetZoom(1f, 0f, true);
            mapZoom.SetPivot(0.5f * Vector2.one);

            mapRawImage.rectTransform.anchoredPosition = Vector2.zero;
        }

        public void SetVisibility(bool isVisible)
        {
            mapMenu.gameObject.SetActive(isVisible);
        }
        #endregion
    }
}