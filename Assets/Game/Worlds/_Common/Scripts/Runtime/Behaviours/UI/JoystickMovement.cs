using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class JoystickMovement : MonoBehaviour
    {
        #region Fields
        private Joystick joystick;
        #endregion

        #region Methods
        private void Awake()
        {
            joystick = GetComponent<Joystick>();
        }
        private void Update()
        {
            if (Player.Instance.Mover.CanInput && Player.Instance && Player.Instance.IsSetup)
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            Camera camera = Player.Instance.Camera.MainCamera;

            Vector3 keyboardForward = Vector3.ProjectOnPlane(camera.transform.forward, Player.Instance.transform.up);
            Vector3 keyboardRight = Vector3.ProjectOnPlane(camera.transform.right, Player.Instance.transform.up);

            Vector3 vertical = keyboardForward * joystick.Vertical;
            Vector3 horizontal = keyboardRight * joystick.Horizontal;

            Player.Instance.Mover.CanMove = Player.Instance.Mover.CanTurn = true;

            Player.Instance.Mover.Direction = (vertical + horizontal).normalized;
        }
        #endregion
    }
}