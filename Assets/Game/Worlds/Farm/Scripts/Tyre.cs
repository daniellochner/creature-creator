using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Tyre : MonoBehaviour
    {
        [SerializeField] private float tyreForce;
        [SerializeField] private AudioClip bounceSound;

        private void OnCollisionEnter(Collision collision)
        {
            Vector3 point = collision.GetContact(0).point;
            CreaturePlayerLocal player = collision.gameObject.GetComponent<CreaturePlayerLocal>();
            if (player != null)
            {
                Vector3 force = (point - transform.position).normalized * player.Velocity.LSpeedPercentage * tyreForce;
                player.GetComponent<Rigidbody>().AddForceAtPosition(force, point, ForceMode.Impulse);

                AudioSource.PlayClipAtPoint(bounceSound, point);
            }
        }
    }
}