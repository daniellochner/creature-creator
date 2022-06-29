using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets
{
    public class PlayerNamer : NetworkBehaviour
    {
        [SerializeField] private Transform playerNameT;
        [SerializeField] private TextMeshProUGUI nameText;

        private Transform cam;

        private void Awake()
        {
            cam = Camera.main.transform;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            // set the name
        }

        private void LateUpdate()
        {
            playerNameT.LookAt(cam);
        }
    }
}