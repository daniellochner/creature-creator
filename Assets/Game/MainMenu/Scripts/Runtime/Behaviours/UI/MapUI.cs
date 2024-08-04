using TMPro;
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
        [SerializeField] private GameObject timedPanel;
        [SerializeField] private TextMeshProUGUI timeText;

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

            timeText.text = "";
        }
        public void OnModeChanged(int option)
        {
            UpdatePadlock(mapOS, modeOS);

            Mode mode = (Mode)option;
            timedPanel.SetActive(mode == Mode.Timed);
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
            if (map == Map.ComingSoon)
            {
                unlocked = false;
            }
            else
            {
                Mode mode = (Mode)modeOS.Selected;
                if (mode == Mode.Adventure)
                {
                    if (!ProgressManager.Instance.IsMapUnlocked(map))
                    {
                        unlocked = false;
                    }
                }
            }

            lockedIcon.SetActive(!unlocked);
        }
        #endregion
    }
}