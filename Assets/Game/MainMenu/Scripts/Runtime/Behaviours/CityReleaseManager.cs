using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CityReleaseManager : MonoBehaviour
    {
        #region Properties
        public static bool IsCityReleased { get; private set; } = false;
        #endregion

        #region Methods
        public void OnComplete()
        {
            IsCityReleased = true;
        }
        #endregion
    }
}