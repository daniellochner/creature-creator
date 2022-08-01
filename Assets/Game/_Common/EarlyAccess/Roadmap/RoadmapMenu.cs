using UnityEngine;
using UnityEngine.Video;

namespace DanielLochner.Assets.CreatureCreator
{
    public class RoadmapMenu : MenuSingleton<RoadmapMenu>
    {
        [SerializeField] private VideoPlayer[] videos;

        public override void Open(bool instant = false)
        {
            base.Open(instant);

            foreach (var video in videos)
            {
                video.Play();
            }
        }

        public override void Close(bool instant = false)
        {
            base.Close(instant);

            foreach (var video in videos)
            {
                video.Pause();
            }
        }
    }
}