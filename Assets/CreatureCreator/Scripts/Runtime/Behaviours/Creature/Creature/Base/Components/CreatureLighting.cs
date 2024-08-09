using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CreatureLighting : NetworkBehaviour
    {
        public bool isLighting;
        public float fadeTime = 0.25f;

        private Coroutine setLightingCoroutine;
        private int count;


        public CreatureConstructor Constructor { get; private set; }

        private bool InDarkArea
        {
            get => count > 0;
        }


        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }
        private void Start()
        {
            var cols = Physics.OverlapSphere(transform.position, 1f);
            foreach (var col in cols)
            {
                OnTriggerEnter(col);
            }
        }

        public void Setup()
        {
            Constructor.OnAddBodyPartPrefab += OnAddBodyPartPrefab;

            if (DefaultLightingManager.Instance)
            {
                isLighting = DefaultLightingManager.Instance.isLighting;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DarkArea"))
            {
                count++;

                if (count == 1)
                {
                    FadeLighting(true, false);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("DarkArea"))
            {
                count--;

                if (count == 0)
                {
                    FadeLighting(false, false);
                }
            }
        }
        private void OnAddBodyPartPrefab(GameObject main, GameObject flipped)
        {
            if (main.TryGetComponent(out BodyPartConstructor bpc) && (bpc.LightSource != null) && SettingsManager.Data.OptimizeLighting)
            {
                bpc.LightSource.enabled = bpc.Flipped.LightSource.enabled = isLighting;
            }
        }

        public void FadeLighting(bool isLighting)
        {
            FadeLighting(isLighting, false);
        }
        public void FadeLighting(bool isLighting, bool isInstant)
        {
            if (SettingsManager.Data.OptimizeLighting)
            {
                if (isLighting && !InDarkArea)
                {
                    return;
                }

                this.StopStartCoroutine(FadeLightingRoutine(isLighting, isInstant), ref setLightingCoroutine);
            }
        }
        private IEnumerator FadeLightingRoutine(bool isLighting, bool isInstant)
        {
            if (isLighting)
            {
                foreach (var source in Constructor.LightSources)
                {
                    source.intensity = 0f;
                    source.enabled = true;
                }
                isLighting = true;
            }

            if (!isInstant)
            {
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float p)
                {
                    foreach (var source in Constructor.LightSources)
                    {
                        source.intensity = isLighting ? p : 1f - p;
                    }
                },
                fadeTime);
            }

            if (!isLighting)
            {
                foreach (var source in Constructor.LightSources)
                {
                    source.intensity = 0f;
                    source.enabled = false;
                }
                isLighting = false;
            }
        }
    }
}