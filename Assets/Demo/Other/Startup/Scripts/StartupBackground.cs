using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class StartupBackground : MonoBehaviour
    {
        [SerializeField] private Material gridMat;
        [SerializeField] private float scale;
        [SerializeField] private float speed;

        private void Start()
        {
            gridMat.mainTextureScale = (1f / scale) * new Vector2(Screen.width, Screen.height);
        }
        private void Update()
        {
            gridMat.mainTextureOffset -= speed * Time.deltaTime * Vector2.one;
        }
    }
}