using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class ColourUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Image colourImage;
        [SerializeField] private ClickUI clickUI;
        #endregion

        #region Properties
        public ClickUI ClickUI => clickUI;
        #endregion

        #region Methods
        public void SetColour(Color colour)
        {
            colourImage.color = colour;
        }
        #endregion
    }
}