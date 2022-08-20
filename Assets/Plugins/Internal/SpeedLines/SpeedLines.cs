using UnityEngine;

namespace DanielLochner.Assets
{
    public class SpeedLines : MonoBehaviour
    {
        private ParticleSystem particles;
        private Camera mainCamera;
        private SelfDestructor selfDestructor;

        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
            selfDestructor = GetComponent<SelfDestructor>();
        }
        private void Update()
        {
            var shape = particles.shape;
            shape.radius = mainCamera.orthographicSize * 2;
            shape.scale = (mainCamera.fieldOfView / 60f) * (new Vector3(mainCamera.aspect, 1f, 1f));
        }

        public void Setup(float t)
        {
            mainCamera = Camera.main;
            transform.SetParent(mainCamera.transform, false);
            transform.localPosition = Vector3.forward;

            selfDestructor.Lifetime = t;
        }
    }
}