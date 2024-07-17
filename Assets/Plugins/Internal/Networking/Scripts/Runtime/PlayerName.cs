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
        public void Setup(PlayerData data)
        {
            SetName(data.username, data.level);
            SetColour(Color.white);

            lookAtConstraint.AddSource(new ConstraintSource() { sourceTransform = CameraUtility.MainCamera.transform, weight = 1f });
        }

        public void SetName(string name, int level)
        {
            string n = name.NoParse();
            string l = $"[{level}]".ToBold();
            nameText.text = $"{n} {l}";
        }
        public void SetColour(Color colour)
        {
            nameText.color = colour;
        }
        #endregion
    }
}