using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureCamera : MonoBehaviour
    {
        [SerializeField] private GameObject cameraPrefab;

        public Transform Root { get; private set; }
        public CameraOrbit CameraOrbit { get; private set; }
        public Camera Camera { get; private set; }

        public void Setup()
        {
            GameObject camera = Instantiate(cameraPrefab);

            Root = camera.transform;
            CameraOrbit = camera.GetComponent<CameraOrbit>();
            Camera = camera.GetComponentInChildren<Camera>();

            camera.GetComponent<Follower>().SetFollow(transform, true);
        }
    }
}