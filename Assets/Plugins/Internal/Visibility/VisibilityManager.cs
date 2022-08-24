using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets
{
    public class VisibilityManager : MonoBehaviourSingleton<VisibilityManager>
    {
        #region Fields
        [SerializeField] private List<VisibilityObject> objects;
        #endregion

        #region Properties
        public List<VisibilityObject> Objects => objects;
        #endregion
    }
}