using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace DanielLochner.Assets
{
    public static class AudioSourceUtility
    {
        public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, AudioMixerGroup group = null)
        {
            if (clip == null) return;
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = position;
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            if (group != null)
                audioSource.outputAudioMixerGroup = group;
            audioSource.clip = clip;
            audioSource.spatialBlend = 1f;
            audioSource.volume = volume;
            audioSource.Play();
            Object.Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
        }

        public static IEnumerator FadeRoutine(this AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }
    }
}