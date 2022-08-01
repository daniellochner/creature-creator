using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EarlyAccessManager : MonoBehaviour
    {
        public void UnlockAll()
        {
            foreach (var obj in DatabaseManager.GetDatabase("Body Parts").Objects)
            {
                EditorManager.Instance.UnlockBodyPart(obj.Key, false);
            }
            foreach (var obj in DatabaseManager.GetDatabase("Patterns").Objects)
            {
                EditorManager.Instance.UnlockPattern(obj.Key, false);
            }
            NotificationsManager.Notify("You unlocked all the parts and patterns!");

            foreach (UnlockableItem item in FindObjectsOfType<UnlockableItem>())
            {
                if (item is UnlockableCollection) continue;
                Destroy(item.gameObject);
            }

            this.Invoke(() => RoadmapMenu.Instance.Open(), 2f);
        }
    }
}