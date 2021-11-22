using UnityEngine;

namespace DanielLochner.Assets
{
    public class SkyManager : MonoBehaviourSingleton<SkyManager>
    {
        #region Fields
        [SerializeField, Range(0, 1)] private float time;
        [Space]
        [SerializeField] private Transform celestialSphere;
        [SerializeField] private Transform atmosphere;
        [SerializeField] private Transform player;
        [SerializeField] private new Transform camera;
        [SerializeField] private Transform sun;
        [Space]
        [SerializeField] private float rotationPeriod;
        [SerializeField] private float axialTilt;
        [SerializeField] private MinMax atmosphereMinMaxHeight;
        [Space]
        [SerializeField] private float horizonHeight;
        [SerializeField] private MinMax horizonMinMaxHeight;
        [SerializeField] private Gradient horizonOverTime;
        [SerializeField] private Gradient zenithOverTime;

        private Material atmosphereMaterial;
        #endregion

        #region Properties
        public Transform Player
        {
            get => player;
            set => player = value;
        }
        public Transform Camera
        {
            get => camera;
            set => camera = value;
        }
        #endregion

        #region Methods
        private void Start()
        {
            atmosphereMaterial = atmosphere.GetComponent<Renderer>().material;
        }
        private void Update()
        {
            HandleAtmosphere();
            HandleCelestialSphere();
        }

        private void HandleAtmosphere()
        {
            // Colours
            float dot = Vector3.Dot(player.up, sun.forward);
            time = (dot + 1f) / 2f;

            atmosphereMaterial.SetColor("_HorizonColour", horizonOverTime.Evaluate(time));
            atmosphereMaterial.SetColor("_ZenithColour", zenithOverTime.Evaluate(time));

            // Horizon-Zenith
            float camHeight = camera.transform.position.magnitude;
            float a1 = Mathf.Acos(atmosphereMinMaxHeight.min / atmosphereMinMaxHeight.max);
            float a2 = Mathf.Acos(atmosphereMinMaxHeight.min / camHeight);
            float a3 = (Mathf.PI / 2f) - (a1 + a2);
            float h = atmosphereMinMaxHeight.max * Mathf.Sin(a3);
            float y = Mathf.Clamp((h + atmosphereMinMaxHeight.max) / (2 * atmosphereMinMaxHeight.max), horizonMinMaxHeight.min, horizonMinMaxHeight.max);

            atmosphereMaterial.SetFloat("_BlendHeightA", y);
            atmosphereMaterial.SetFloat("_BlendHeightB", y + horizonHeight);

            // Rotation
            atmosphere.rotation = player.rotation;
        }
        private void HandleCelestialSphere()
        {
            celestialSphere.Rotate(Quaternion.Euler(axialTilt, 0, 0) * Vector3.up, Time.deltaTime * 360f / rotationPeriod);

            //Vector3 axis = (Quaternion.Euler(axialTilt, 0, 0) * Vector3.up) * 200;
            //Debug.DrawLine(-axis, axis, Color.red);
        }
        #endregion
    }
}