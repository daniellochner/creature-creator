using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class AbilitiesManager : MonoBehaviourSingleton<AbilitiesManager>
    {
        [SerializeField] public GridLayoutGroup abilitiesGrid;

        public GridLayoutGroup AbilitiesGrid => abilitiesGrid;
    }
}