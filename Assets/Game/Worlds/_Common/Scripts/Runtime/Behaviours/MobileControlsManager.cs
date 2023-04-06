using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MobileControlsManager : MonoBehaviourSingleton<MobileControlsManager>
    {
        #region Fields
        [SerializeField] private MobileControlsUI mobileControlsUI;
        #endregion

        #region Properties
        public MobileControlsUI MobileControlsUI => mobileControlsUI;

        public Joystick Joystick
        {
            get
            {
                switch (SettingsManager.Data.Joystick)
                {
                    case Settings.JoystickType.Fixed:
                        return MobileControlsUI.FixedJoystick;
                    case Settings.JoystickType.Floating:
                        return MobileControlsUI.FloatJoystick;
                }
                return null;
            }
        }
        #endregion
    }
}