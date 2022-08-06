using UnityEngine;

namespace DanielLochner.Assets
{
    public class PitchRandomizer : MonoBehaviour
    {
        [SerializeField] MinMax minMaxPitch;
        private void Awake()
        {
            GetComponent<AudioSource>().pitch = minMaxPitch.Random;
        }
    }
}