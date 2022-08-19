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

        public MinMax MinMaxSpeedUp => minMaxSpeedUp;
        public float Speed
        {
            get => speed.Value;
        }
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

            speed.Value = minMaxSpeedUp.Clamp(s);
            speedUpCoroutine = InvokeUtility.Invoke(this, delegate
            {
                speed.Value = 0f;
            }, t);
        }
        
        private void UpdateSpeed(float oldSpeedUp, float newSpeedUp)
        {
            Constructor.Statistics.SpeedBoost = newSpeedUp;
        }
        #endregion
    }
}