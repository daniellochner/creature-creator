using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MapUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Menu mapMenu;
        [SerializeField] private Image screenshotImg;
        [SerializeField] private Sprite[] screenshots;
        [SerializeField] private GameObject lockedIcon;

        private OptionSelector mapOS, modeOS;
        #endregion

        #region Methods
        public void View(Transform anchor)
        {
            mapMenu.transform.position = anchor.position;
            mapMenu.Open();
        }
        public void Hide()
        {
            mapMenu.Close();
        }

        public void OnMapChanged(int option)
        {
            screenshotImg.sprite = screenshots[option];
            UpdatePadlock(mapOS, modeOS);
        }
        public void OnModeChanged(int option)
        {
            UpdatePadlock(mapOS, modeOS);
        }

        public void UpdatePadlock(OptionSelector mapOS, OptionSelector modeOS)
        {
            if (!mapOS)
            {
                return;
            }
            this.mapOS = mapOS;

            if (!modeOS)
            {
                return;
            }
            this.modeOS = modeOS;

            bool unlocked = true;
            Map map = (Map)mapOS.Selected;
            Mode mode = (Mode)modeOS.Selected;
            if (mode == Mode.Adventure)
            {
                string mapId = $"map_unlocked_{map}".ToLower();
                if (PlayerPrefs.GetInt(mapId) == 0)
                {
                    unlocked = false;
                }
            }

            if (map == Map.City)
            {
                unlocked = false;
            }

            lockedIcon.SetActive(!unlocked);
        }
        #endregion
    }
}