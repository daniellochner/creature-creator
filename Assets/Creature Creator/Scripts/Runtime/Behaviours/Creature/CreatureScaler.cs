using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureScaler : MonoBehaviour, IConstructible
    {
        [SerializeField] private float scale = 1f;

        public CreatureConstructor Constructor { get; set; }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        public void Setup()
        {
            Constructor.OnPreConstructCreature += () => Scale(1f);
            Constructor.OnConstructCreature += () => Scale(scale);
        }

        public void Scale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
    }
}