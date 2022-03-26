// Wind Turbine
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets
{
    public class WindTurbine : MonoBehaviour
    {
        [SerializeField] private MinMax minMaxRate;
        private void Awake()
        {
            GetComponent<Animator>().SetFloat("RPS", Random.Range(minMaxRate.min, minMaxRate.max));
        }
    }
}