using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets
{
    public class PlayerName : MonoBehaviour
    {
        #region Fields
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private LookAtConstraint lookAtConstraint;
        #endregion

        #region Methods
        public void Setup(string name, Color colour)
        {
            SetName(name);
            SetColour(colour);

            lookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = CameraUtility.MainCamera.transform, weight = 1f });
        }

        public void SetName(string name)
        {
            nameText.text = name.NoParse();
        }
        public void SetColour(Color colour)
        {
            nameText.color = colour;
        }
        #endregion
    }
}