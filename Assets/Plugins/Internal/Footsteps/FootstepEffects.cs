using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace DanielLochner.Assets
{
    [CreateAssetMenu(menuName = "Footsteps/Effects")]
    public class FootstepEffects : ScriptableObject
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<StepType, Effects<AudioClip>> sounds;
        [SerializeField] private SerializableDictionaryBase<StepType, Effects<GameObject>> particles;
        #endregion

        #region Methods
        public AudioClip GetSound(StepType stepType)
        {
            if (sounds.ContainsKey(stepType))
            {
                AudioClip[] s = sounds[stepType].effects;
                return s[UnityEngine.Random.Range(0, s.Length)];
            }
            return null;
        }
        public GameObject GetParticle(StepType stepType)
        {
            if (particles.ContainsKey(stepType))
            {
                GameObject[] p = particles[stepType].effects;
                return p[UnityEngine.Random.Range(0, p.Length)];
            }
            return null;
        }
        #endregion

        #region Nested
        [Serializable]
        public class Effects<T>
        {
            public T[] effects;
        }

        public enum StepType
        {
            Walk,
            Run,
            JumpStart,
            JumpEnd
        }
        #endregion
    }
}