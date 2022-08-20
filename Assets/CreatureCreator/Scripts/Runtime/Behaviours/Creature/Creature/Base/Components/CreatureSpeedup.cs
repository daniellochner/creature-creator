using System;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureSpeedup : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private MinMax minMaxSpeedUp;
        [SerializeField] private NetworkVariable<float> speed = new NetworkVariable<float>(0);
        private Coroutine speedUpCoroutine;
        #endregion

        #region Properties
        public CreatureConstructor Constructor { get; private set; }

        public MinMax MinMaxSpeedUp
        {
            get => minMaxSpeedUp;
        }
        public float Speed
        {
            get => speed.Value;
        }

        public Action<float, float> OnSpeedUp { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }
        private void Start()
        {
            speed.OnValueChanged += UpdateSpeed;
            speed.SetDirty(true);
        }

        public void SpeedUp(float s, float t)
        {
            SpeedUpServerRpc(s, t);
        }
        [ServerRpc]
        public void SpeedUpServerRpc(float s, float t)
        {
            if (speedUpCoroutine != null)
            {
                StopCoroutine(speedUpCoroutine);
            }

            speed.Value = minMaxSpeedUp.Clamp(speed.Value + s);
            speedUpCoroutine = InvokeUtility.Invoke(this, delegate
            {
                speed.Value = 0f;
            }, t);

            SpeedUpClientRpc(s, t);
        }
        [ClientRpc]
        public void SpeedUpClientRpc(float s, float t)
        {
            OnSpeedUp?.Invoke(s, t);
        }
        
        private void UpdateSpeed(float oldSpeed, float newSpeed)
        {
            Constructor.Statistics.SpeedBoost = newSpeed;
        }
        #endregion
    }
}