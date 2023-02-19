using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [SerializeField] private TerrainFootsteps sounds;


        private List<AudioSource> audioSources = new List<AudioSource>();





        public void Step(string terrain, StepType type, Vector3 position, float volume = 1f)
        {



            //if (sounds.ContainsKey(terrain))
            //{
            //    //AudioClip[] sounds = sounds[terrain];
            //}
        }


        //public void Step(Texture texture, StepType stepType, Vector3 position, float volume = 1f)
        //{
        //    if (textureSounds.ContainsKey(texture))
        //    {
        //        AudioClip[] sounds = textureSounds[texture];
        //        AudioSource.PlayClipAtPoint(sounds[UnityEngine.Random.Range(0, sounds.Length)], position, volume);
        //    }
        //    if (textureParticles.ContainsKey(texture))
        //    {
        //        GameObject[] effects = textureParticles[texture];
        //        Instantiate(effects[UnityEngine.Random.Range(0, effects.Length)], position, Quaternion.identity, Dynamic.Transform);
        //    }
        //}

        [Serializable]
        public class TerrainFootsteps : SerializableDictionaryBase<TerrainType, FootstepEffects> { }

        [Serializable]
        public class FootstepEffects : SerializableDictionaryBase<StepType, Effects<AudioClip>> { }

        [Serializable]
        public class Effects<T>
        {
            public T[] effects;
        }

        public enum TerrainType
        {
            Dirt,
            Grass,
            Sand,
            Rock,
            Water
        }

        public enum StepType
        {
            Walk,
            Run,
            JumpStart,
            JumpEnd
        }
    }
}