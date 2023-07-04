using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CarsManager : NetworkBehaviour
    {
        #region Fields
        [SerializeField] private Vehicle[] cars;
        [SerializeField] private float factor;
        #endregion

        #region Methods
        private void Start()
        {
            if (IsServer && WorldManager.Instance.IsMultiplayer)
            {
                List<Vehicle> tmp = new List<Vehicle>(cars);
                tmp.Shuffle();

                for (int i = 0; i < (int)(tmp.Count * factor); i++)
                {
                    tmp[i].NetworkObject.Despawn();
                }
            }
        }
        #endregion
    }
}