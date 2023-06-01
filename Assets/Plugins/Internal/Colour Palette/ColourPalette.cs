using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ColourPalette : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Color[] colours;
        [SerializeField] private ColourUI colourUIPrefab;
        [SerializeField] private Image preview;
        [SerializeField] private ClickUI clickUI;
        [SerializeField] private RectTransform coloursRT;
        [SerializeField] private TextMeshProUGUI foregroundText;
        [Space]
        [SerializeField] private Color startColour;
        [SerializeField] private Vector2 size;
        [SerializeField] private UnityEvent<Color> onColourPick;
        #endregion

        #region Properties
        public Color Colour { get; private set; }

        public ClickUI ClickUI => clickUI;

        public TextMeshProUGUI Name => foregroundText;
        #endregion

        #region Methods
        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            preview.color = (Colour = startColour);
            foregroundText.color = startColour.grayscale > 0.5f ? Color.black : Color.white;

            int colourIndex = 0;
            for (int y = 0; y < size.y; y++)
            {
                HorizontalLayoutGroup hlg = new GameObject(y.ToString()).AddComponent<HorizontalLayoutGroup>();
                hlg.transform.SetParent(coloursRT);
                hlg.transform.localScale = Vector3.one;

                hlg.childControlHeight = hlg.childControlWidth = true;

                for (int x = 0; x < size.x; x++)
                {
                    ColourUI colourUI = Instantiate(colourUIPrefab, hlg.transform);

                    Color colour = colours[colourIndex];
                    colourUI.SetColour(colour);
                    colourUI.ClickUI.OnLeftClick.AddListener(delegate
                    {
                        SetColour(colour, true);
                    });
                    colourUI.ClickUI.OnMiddleClick.AddListener(delegate
                    {
                        ColourPickerDialog.Pick(colour, delegate(Color c)
                        {
                            colour = c;
                            colourUI.SetColour(c);
                        });
                    });

                    colourIndex++;
                }
            }
        }
        public void SetColour(Color colour, bool notify = false)
        {
            preview.color = (Colour = colour);
            coloursRT.gameObject.SetActive(false);

            foregroundText.color = colour.grayscale > 0.5f ? Color.black : Color.white;

            if (notify)
            {
                onColourPick.Invoke(colour);
            }
        }

        public void PickColour()
        {
            ColourPickerDialog.Pick(Colour, delegate(Color c)
            {
                SetColour(c, true);
            });
        }
        #endregion
    }
}