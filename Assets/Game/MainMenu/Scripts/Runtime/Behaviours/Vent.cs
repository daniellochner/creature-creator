using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Vent : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private MinMax minMaxTime;
        [SerializeField] private GameObject steamPrefab;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minMaxTime.min, minMaxTime.max));
                Instantiate(steamPrefab, spawnPoint, false);
            }
        }
    }
}