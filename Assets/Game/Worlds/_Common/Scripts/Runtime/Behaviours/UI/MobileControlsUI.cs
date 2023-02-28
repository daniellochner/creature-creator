using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MobileControlsUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Joystick fixedJoystick;
        [SerializeField] private Joystick floatJoystick;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button networkButton;
        [SerializeField] private Button talkButton;
        #endregion

        #region Properties
        public Joystick FixedJoystick => fixedJoystick;
        public Joystick FloatJoystick => floatJoystick;
        #endregion

        #region Methods
        private void Start()
        {
            fixedJoystick.gameObject.SetActive(SettingsManager.Data.Joystick == Settings.JoystickType.Fixed);
            floatJoystick.gameObject.SetActive(SettingsManager.Data.Joystick == Settings.JoystickType.Floating);

            settingsButton.onClick.AddListener(delegate
            {
                PauseMenu.Instance.Open();
            });

            networkButton.gameObject.SetActive(WorldManager.Instance.World is WorldMP);
            networkButton.onClick.AddListener(delegate
            {
                NetworkPlayersMenu.Instance.Toggle();
            });

            talkButton.gameObject.SetActive(WorldManager.Instance.World is WorldMP);
            talkButton.onClick.AddListener(delegate
            {
                Player.Instance.Messenger.Open();
            });
        }
        #endregion
    }
}