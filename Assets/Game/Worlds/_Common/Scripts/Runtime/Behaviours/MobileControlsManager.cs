using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MobileControlsManager : MonoBehaviourSingleton<MobileControlsManager>
    {
        #region Fields
        [SerializeField] private MobileControlsUI mobileControlsUI;
        #endregion

        #region Properties
        public Joystick FixedJoystick => mobileControlsUI.FixedJoystick;
        public Joystick FloatJoystick => mobileControlsUI.FloatJoystick;

        public Joystick Joystick => (SettingsManager.Data.Joystick == Settings.JoystickType.Fixed) ? mobileControlsUI.FixedJoystick : mobileControlsUI.FloatJoystick;
        #endregion
    }
}