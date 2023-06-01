// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCloner))]
    public class CreaturePhotographer : MonoBehaviour
    {
        #region Fields
        [Header("Settings")]
        [SerializeField] private Vector3 cameraPosition;
        [SerializeField] private Vector3 cameraRotation;
        #endregion

        #region Properties
        public CreatureCloner CreatureCloner { get; private set; }

        public Action<Texture2D> OnTakePhoto { get; set; }
        public static bool IsTakingPhoto { get; private set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            CreatureCloner = GetComponent<CreatureCloner>();
        }

        public void TakePhoto(int resolution, Action<Texture2D> onPhotoTaken, CreatureData dataOverride = null)
        {
            StartCoroutine(TakePhotoRoutine(resolution, onPhotoTaken, dataOverride));
        }
        public IEnumerator TakePhotoRoutine(int resolution, Action<Texture2D> onPhotoTaken, CreatureData dataOverride = null)
        {
            yield return new WaitWhile(() => IsTakingPhoto); // Prevent rare case when creatures are photographed in the same frame.
            IsTakingPhoto = true;

            // Clone creature (to world origin).
            CreatureConstructor tmpCreature = CreatureCloner.Clone(creatureData: dataOverride, parent: Dynamic.Transform);
            tmpCreature.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Photography"));
            tmpCreature.SkinnedMeshRenderer.lightProbeUsage = LightProbeUsage.Off;
            foreach (BodyPartConstructor bpc in tmpCreature.BodyParts)
            {
                bpc.Renderer.lightProbeUsage = LightProbeUsage.Off;
                bpc.Flipped.Renderer.lightProbeUsage = LightProbeUsage.Off;
            }

            GameObject photoCamGO = new GameObject("Camera");
            photoCamGO.transform.SetParent(tmpCreature.Body);
            photoCamGO.transform.localPosition = cameraPosition;
            photoCamGO.transform.localRotation = Quaternion.Euler(cameraRotation);

            RenderTexture photoRenderTexture = new RenderTexture(resolution, resolution, 24);
            Camera photoCam = photoCamGO.AddComponent<Camera>();
            photoCam.nearClipPlane = 0.01f;
            photoCam.clearFlags = CameraClearFlags.SolidColor;
            photoCam.cullingMask = LayerMask.GetMask("Photography");
            photoCam.targetTexture = photoRenderTexture;
            photoCam.enabled = false;

            yield return new WaitForEndOfFrame(); // Invoke at end of frame to prevent "Assertion failed on expression: a.size == b.size".

            // Take photo.
            photoCam.Render();
            onPhotoTaken(photoCam.targetTexture.ToTexture2D(resolution));

            // Destroy temporary creature.
            DestroyImmediate(tmpCreature.gameObject); // Must be immediate to prevent creature from showing in next photo.

            IsTakingPhoto = false;
        }
        #endregion
    }
}