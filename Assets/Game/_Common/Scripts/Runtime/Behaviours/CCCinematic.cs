using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCCinematic : Cinematic
    {
        #region Methods
        public override void Begin(bool fade)
        {
            base.Begin(fade);
            if (fade)
            {
                FadeMusic(false, 1f);
            }
        }
        public override void BeginFade()
        {
            base.BeginFade();

            EditorManager.Instance.SetVisibility(false, 0f);

            SetVisibility(true);
        }
        public override void End(bool fade)
        {
            base.End(fade);
            if (fade)
            {
                FadeMusic(true, 1f);
            }
        }
        public override void EndFade()
        {
            base.EndFade();

            EditorManager.Instance.SetVisibility(true, 0f);

            SetVisibility(false);
        }

        protected virtual void SpawnCreature(Transform parent, CreatureData data = null)
        {
            CreatureConstructor creature = Player.Instance.Cloner.Clone(data);
            creature.transform.SetZeroParent(parent);

            if (creature.Legs.Count == 0)
            {
                Mesh bodyMesh = new Mesh();
                creature.SkinnedMeshRenderer.BakeMesh(bodyMesh);
                float minY = Mathf.Infinity;
                foreach (Vector3 vertex in bodyMesh.vertices)
                {
                    if (vertex.y < minY)
                    {
                        minY = vertex.y;
                    }
                }
                creature.Body.localPosition = Vector3.up * -minY;
            }
        }

        protected void FadeMusic(bool enabled, float time)
        {
            string music = SettingsManager.Data.InGameMusic.ToString();
            if (!enabled || SettingsManager.Data.InGameMusic == Settings.InGameMusicType.None)
            {
                music = null;
            }
            MusicManager.Instance.FadeTo(music, time, 1f);
        }

        protected void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
            Player.Instance.Camera.enabled = !isVisible;
        }
        #endregion
    }
}