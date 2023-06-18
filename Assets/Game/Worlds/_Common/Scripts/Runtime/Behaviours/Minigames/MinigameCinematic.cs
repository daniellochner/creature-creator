using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class MinigameCinematic : CCCinematic
    {
        public override void Show()
        {
            base.Show();
            EditorManager.Instance.SetVisibility(false, 0f);
        }
        public override void Hide()
        {
            base.Hide();
            EditorManager.Instance.SetVisibility(true, 0f);
        }
    }
}