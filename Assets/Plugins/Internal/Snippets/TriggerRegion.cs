using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DanielLochner.Assets
{
    [RequireComponent(typeof(Collider))]
    public class TriggerRegion : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<string> triggerable = new List<string>();
        [SerializeField] public List<string> ignored = new List<string>();
        [Space]
        [SerializeField] private UnityEvent<Collider> onEnter = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> onExit = new UnityEvent<Collider>();
        [Header("Debug")]
        [SerializeField, ReadOnly] private List<Collider> entered = new List<Collider>();

        private Collider region;
        #endregion

        #region Properties
        public Collider Region => region;

        public UnityEvent<Collider> OnEnter => onEnter;
        public UnityEvent<Collider> OnExit => onExit;

        public Func<Collider, bool> OnCanEnter { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            region = GetComponent<Collider>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (CanEnter(other) && !entered.Contains(other) && !ignored.Contains(other.name))
            {
                onEnter.Invoke(other);
                entered.Add(other);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (entered.Contains(other) && !ignored.Contains(other.name))
            {
                onExit.Invoke(other);
                entered.Remove(other);
            }
        }

        public void Clear()
        {
            entered.Clear();
        }
        public void Validate()
        {
            List<Collider> tmp = new List<Collider>(entered);
            Clear();
            Collider[] colliders = PhysicsUtility.Overlap(region);

            foreach (Collider collider in colliders)
            {
                if (tmp.Contains(collider))
                {
                    tmp.Remove(collider);
                }
                else
                {
                    OnTriggerEnter(collider);
                }
            }
            foreach (Collider collider in tmp)
            {
                OnTriggerExit(collider);
            }
        }
        public bool CanEnter(Collider other)
        {
            return (triggerable.Count == 0 || triggerable.Contains(other.tag)) && (OnCanEnter == null || (OnCanEnter != null && OnCanEnter(other)));
        }
        #endregion
    }
}