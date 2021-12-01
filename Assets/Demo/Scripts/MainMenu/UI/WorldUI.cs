using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DanielLochner.Assets.CreatureCreator
{
    public class WorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playersText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI mapVersionText;
        [SerializeField] private GameObject padlockIcon;
        [SerializeField] private Button joinButton;

        public Button JoinButton => joinButton;

        public void Setup(string lobbyId, int currentPlayers, int maxPlayers, string name, string map, string version, bool isPasswordProtected)
        {
            playersText.text = $"{currentPlayers}/{maxPlayers}";
            nameText.text = name;
            mapVersionText.text = $"{map} ({version})";
            padlockIcon.SetActive(isPasswordProtected);
        }
    }
}