using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class VisibilitySource : MonoBehaviourSingleton<VisibilitySource>
    {
        #region Properties
        public List<VisibilityTrigger> Triggers { get; set; } = new List<VisibilityTrigger>();
        #endregion

        #region Methods
        private void Start()
        {
            // Source
            GameObject source = new GameObject("Source");
            source.transform.parent = transform;
            source.transform.localPosition = Vector3.zero;
            source.layer = LayerMask.NameToLayer("Visibility");

            Rigidbody rb = source.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            SphereCollider col = source.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 0.5f;

            // Triggers
            foreach (VisibilityTrigger trigger in FindObjectsOfType<VisibilityTrigger>())
            {
                Triggers.Add(trigger);
                trigger.Setup();
            }
        }

        public void Validate()
        {
            foreach (VisibilityTrigger trigger in Triggers)
            {
                trigger.Validate();
            }
        }
        #endregion
    }
}