// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BoxCreature : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Vector3 force;

        private Camera mainCamera;
        private CreatureConstructor creatureConstructor;
        #endregion

        #region Methods
        public void Spawn(CreatureData creatureData)
        {
            creatureConstructor = GetComponentInChildren<CreatureConstructor>();
            creatureConstructor.Construct(creatureData);

            this.InvokeOverTime(delegate (float progress)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(0, 1, progress);
            }, 0.5f);
        }
        public void ReplaceWithRagdoll()
        {
            if (CanvasUtility.IsPointerOverUI)
            {
                return;
            }

            GetComponent<Click>().enabled = false;
            creatureConstructor.gameObject.SetActive(false);

            CreatureConstructor ragdoll = creatureConstructor.GetComponent<CreatureRagdoll>().Generate(creatureConstructor.Data);
            foreach (Transform bone in ragdoll.Bones)
            {
                Press press = bone.gameObject.AddComponent<Press>();
                press.OnPress.AddListener(delegate
                {
                    if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(mainCamera, Input.mousePosition), out RaycastHit hitInfo))
                    {
                        Vector3 dir = (hitInfo.point - mainCamera.transform.position).normalized;
                        hitInfo.rigidbody.AddForce((dir * force.z) + (Vector3.up * force.y), ForceMode.Impulse);
                    }
                });
            }

            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (LimbConstructor limb in ragdoll.Limbs)
            {
                limb.gameObject.layer = limb.FlippedLimb.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
        }
        #endregion
    }
}