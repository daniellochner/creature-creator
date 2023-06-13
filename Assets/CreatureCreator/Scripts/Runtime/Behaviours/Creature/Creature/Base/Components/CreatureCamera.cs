using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSpeedup))]
    [RequireComponent(typeof(CreatureHealth))]
    public class CreatureCamera : MonoBehaviour, ISetupable
    {
        #region Fields
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private MinMax minMaxDamage;
        [SerializeField] private MinMax minMaxStress;
        #endregion

        #region Properties
        public Transform Root { get; private set; }
        public CameraOrbit CameraOrbit { get; private set; }
        public Follower Follower { get; private set; }
        public Camera MainCamera { get; private set; }
        public Camera ToolCamera { get; private set; }
        public StressReceiver StressReceiver { get; private set; }

        public CreatureHealth Health { get; private set; }

        public bool IsSetup { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Health = GetComponent<CreatureHealth>();
        }
        private void OnEnable()
        {
            if (IsSetup)
            {
                MainCamera.gameObject.SetActive(true);
            }
        }
        private void OnDisable()
        {
            if (IsSetup)
            {
                MainCamera.gameObject.SetActive(false);
            }
        }

        public void Setup()
        {
            GameObject camera = Instantiate(cameraPrefab);

            Root = camera.transform;
            CameraOrbit = camera.GetComponent<CameraOrbit>();
            Follower = camera.GetComponent<Follower>();
            MainCamera = Root.GetChild(0).GetChild(0).GetComponent<Camera>();
            ToolCamera = Root.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>();
            StressReceiver = MainCamera.GetComponent<StressReceiver>();

            Follower.SetFollow(transform, true);

            Health.OnTakeDamage += delegate (float damage, DamageReason reason, string inflicter)
            {
                StressReceiver.InduceStress(Mathf.Lerp(minMaxStress.min, minMaxStress.max, Mathf.InverseLerp(minMaxDamage.min, minMaxDamage.max, damage)));
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            };

            IsSetup = true;
        }
        #endregion
    }
}