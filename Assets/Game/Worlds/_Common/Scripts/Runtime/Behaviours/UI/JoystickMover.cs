using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class JoystickMover : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float threshold;

        private Joystick joystick;
        #endregion

        #region Methods
        private void Awake()
        {
            joystick = GetComponent<Joystick>();
        }
        private void Update()
        {
            if (Player.Instance && Player.Instance.IsSetup && Player.Instance.Mover.CanInput)
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            Camera camera = Player.Instance.Camera.MainCamera;

            Vector3 keyboardForward = Vector3.ProjectOnPlane(camera.transform.forward, Player.Instance.transform.up);
            Vector3 keyboardRight = Vector3.ProjectOnPlane(camera.transform.right, Player.Instance.transform.up);

            float v = Mathf.Abs(joystick.Vertical) > threshold ? joystick.Vertical : 0f;
            float h = Mathf.Abs(joystick.Horizontal) > threshold ? joystick.Horizontal : 0f;

            Vector3 vertical = keyboardForward * v;
            Vector3 horizontal = keyboardRight * h;

            Player.Instance.Mover.CanMove = Player.Instance.Mover.CanTurn = true;

            Player.Instance.Mover.Direction = (vertical + horizontal).normalized;
        }
        #endregion
    }
}