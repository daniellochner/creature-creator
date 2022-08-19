using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureSpeedup))]
    public class CreatureCamera : MonoBehaviour, ISetupable
    {
        #region Fields
        [SerializeField] private GameObject cameraPrefab;
        [SerializeField] private MinMax minMaxFOV;
        [SerializeField] private float fovSmoothing;

        private Camera[] cameras;
        private float targetFOV;
        #endregion

        #region Properties
        public Transform Root { get; private set; }
        public CameraOrbit CameraOrbit { get; private set; }
        public Follower Follower { get; private set; }
        public Camera Camera { get; private set; }

        public CreatureSpeedup Speedup { get; private set; }

        public bool IsSetup { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Speedup = GetComponent<CreatureSpeedup>();
        }
        public void Setup()
        {
            GameObject camera = Instantiate(cameraPrefab);

            cameras = camera.GetComponentsInChildren<Camera>();
            Camera = cameras[0];

            Root = camera.transform;
            CameraOrbit = camera.GetComponent<CameraOrbit>();
            Follower = camera.GetComponent<Follower>();

            Follower.SetFollow(transform, true);

            targetFOV = minMaxFOV.min;

            IsSetup = true;
        }
        private void Update()
        {
            if (!IsSetup) return;

            float s = Mathf.InverseLerp(Speedup.MinMaxSpeedUp.min, Speedup.MinMaxSpeedUp.max, Speedup.Speed);
            float f = Mathf.Lerp(minMaxFOV.min, minMaxFOV.max, s);
            targetFOV = Mathf.Lerp(targetFOV, f, Time.deltaTime * fovSmoothing);

            foreach (Camera cam in cameras)
            {
                cam.fieldOfView = targetFOV;
            }
        }
        #endregion
    }
}