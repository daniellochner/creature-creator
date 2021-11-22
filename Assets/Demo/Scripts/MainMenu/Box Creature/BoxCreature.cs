// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class BoxCreature : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Vector3 force;
        [SerializeField] private GameObject creatureName;

        private Camera mainCamera;
        private CreatureConstructor creatureConstructor;
        #endregion

        #region Methods
        private void Start()
        {
            creatureName.GetComponent<Canvas>().worldCamera = mainCamera = Camera.main;
            creatureName.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource()
            {
                sourceTransform = mainCamera.transform, weight = 1
            });
        }

        public void Setup(CreatureData creatureData)
        {
            creatureConstructor = GetComponentInChildren<CreatureConstructor>();
            creatureConstructor.Construct(creatureData);

            creatureName.GetComponent<TextMeshProUGUI>().text = creatureData.Name;
        }
        public void ReplaceWithRagdoll()
        {
            if (CanvasUtility.IsPointerOverUI)
            {
                return;
            }

            GetComponent<Click>().enabled = false;
            Destroy(creatureConstructor.gameObject);

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