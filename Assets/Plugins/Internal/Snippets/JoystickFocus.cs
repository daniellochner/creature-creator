using UnityEngine;

namespace DanielLochner.Assets
{
    public class JoystickFocus : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float threshold;
        [Space]
        [SerializeField] private GameObject top;
        [SerializeField] private GameObject right;
        [SerializeField] private GameObject bottom;
        [SerializeField] private GameObject left;

        private Joystick joystick;
        #endregion

        #region Methods
        private void Awake()
        {
            joystick = GetComponent<Joystick>();
        }
        private void Update()
        {
            top.SetActive(joystick.Vertical > +threshold);
            right.SetActive(joystick.Horizontal > +threshold);
            bottom.SetActive(joystick.Vertical < -threshold);
            left.SetActive(joystick.Horizontal < -threshold);
        }
        #endregion
    }
}