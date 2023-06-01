// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Ladder : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float liftSpeed;
        #endregion

        #region Properties
        public bool IsMovingForward
        {
            get
            {
                if (SystemUtility.IsDevice(DeviceType.Desktop))
                {
                    return InputUtility.GetKey(KeybindingsManager.Data.WalkForwards);
                }
                else 
                if (SystemUtility.IsDevice(DeviceType.Handheld))
                {
                    return Vector2.Dot(MobileControlsManager.Instance.Joystick.Direction, Vector2.up) > 0.5f;
                }
                return false;
            }
        }
        #endregion

        #region Methods
        private void OnTriggerStay(Collider other)
        {
            CreaturePlayerLocal player = other.GetComponent<CreaturePlayerLocal>();
            if (player != null && IsMovingForward)
            {
                player.Rigidbody.velocity = transform.up * liftSpeed;
            }
        }
        #endregion
    }
}