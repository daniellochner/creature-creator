using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CollideFX : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject particleFX;
        [SerializeField] private AudioClip[] soundFX;
        [SerializeField] private MinMax minMaxPitch;
        [SerializeField] private float cooldown;
        [SerializeField] private StressReceiver stressReceiver;
        [SerializeField, DontDrawIf("stressReceiver", null)] private float stress;

        private AudioSource audioSource;
        private float timeOfLast = Mathf.NegativeInfinity;
        #endregion

        #region Methods
        private void Awake()
        {
            if (soundFX.Length > 0)
            {
                audioSource = gameObject.GetOrAddComponent<AudioSource>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Collide(transform.position);
        }
        private void OnCollisionEnter(Collision collision)
        {
            Collide(collision.contacts[0].point);
        }

        private void Collide(Vector3 point)
        {
            if (Time.time - timeOfLast > cooldown)
            {
                if (soundFX.Length > 0)
                {
                    audioSource.pitch = Random.Range(minMaxPitch.min, minMaxPitch.max);
                    audioSource.PlayOneShot(soundFX[Random.Range(0, soundFX.Length)]);
                }
                if (particleFX != null)
                {
                    Instantiate(particleFX, point, Quaternion.identity);
                }
                if (stressReceiver != null)
                {
                    stressReceiver.InduceStress(stress);
                }

                timeOfLast = Time.time;
            }
        }
        #endregion
    }
}