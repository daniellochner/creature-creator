using UnityEngine;

namespace DanielLochner.Assets
{
    public class JoystickFocus : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float threshold;
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private GameObject[] focus;
        #endregion

        #region Methods
        private void Update()
        {
            focus[0].SetActive(joystick.Vertical   > +threshold);
            focus[1].SetActive(joystick.Horizontal > +threshold);
            focus[2].SetActive(joystick.Vertical   < -threshold);
            focus[3].SetActive(joystick.Horizontal < -threshold);
        }
        #endregion
    }
}