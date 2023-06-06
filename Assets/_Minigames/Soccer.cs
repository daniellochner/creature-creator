using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class Soccer : Minigame
    {
        [SerializeField] private Color red;
        [SerializeField] private Color blue;

        [SerializeField] private Ball[] balls;
        [SerializeField] private float spawnDelay;

        public void SpawnBalls()
        {
            if (IsServer)
            {
                for (int i = 0; i < balls.Length; i++)
                {
                    Ball ball = balls[i];
                    this.Invoke(delegate
                    {
                        ball.Teleport(ball.transform.parent.position);
                    }, 
                    spawnDelay * i);
                }
            }
        }



        protected override void OnApplyRestrictions()
        {
            base.OnApplyRestrictions();

            EditorManager.Instance.SetRestrictedBones(5);
            EditorManager.Instance.SetRestrictedColour(GetTeamColour());
        }


        public Color GetTeamColour()
        {
            return blue;
        }
    }
}