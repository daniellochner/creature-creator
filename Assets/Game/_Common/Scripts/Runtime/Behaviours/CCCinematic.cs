using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CCCinematic : Cinematic
    {
        #region Methods
        protected virtual CreatureConstructor SpawnCreature(Transform parent, CreatureData data = null)
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

            return creature;
        }

        protected void SetMusic(bool isEnabled, float time)
        {
            string music = SettingsManager.Data.InGameMusic.ToString();
            if (!isEnabled || music == "None")
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