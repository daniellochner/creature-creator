// Conveyor Belt
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private float offset = 0.991725f;
        [SerializeField] private Belt belt;
        [SerializeField] private Animator[] gears;

        private void Start()
        {
            float r = belt.transform.position.y - transform.position.y;
            float w = belt.Speed / r;
            float rps = w / (2f * Mathf.PI); // revolutions per second

            foreach (Animator gear in gears)
            {
                gear.SetFloat("RPS", rps * offset);
            }
        }
    }
}