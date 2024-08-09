using System.Collections;

namespace DanielLochner.Assets.CreatureCreator
{
    public class DefaultLightingManager : MonoBehaviourSingleton<DefaultLightingManager>
    {
        public bool isLighting;

        private IEnumerator Start()
        {
            yield return new WaitUntilSetup(GameSetup.Instance);
            foreach (var creature in CreatureBase.Creatures)
            {
                creature.Lighting.isLighting = isLighting;
            }
        }
    }
}