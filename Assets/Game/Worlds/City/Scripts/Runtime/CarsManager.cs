using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CarsManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Vehicle[] cars;
        [SerializeField] private float factor;
        #endregion

        #region Methods
        private void Start()
        {
            if (!WorldManager.Instance.IsMultiplayer) return;

            List<Vehicle> tmp = new List<Vehicle>(cars);
            tmp.Shuffle();

            for (int i = 0; i < (int)(tmp.Count * factor); i++)
            {
                tmp[i].NetworkObject.Despawn();
            }
        }
        #endregion
    }
}