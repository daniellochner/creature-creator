using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class EarlyAccessManager : MonoBehaviour
    {
        public void UnlockAll()
        {
            StartCoroutine(UnlockAllRoutine());
        }
        private IEnumerator UnlockAllRoutine()
        {
            foreach (var obj in DatabaseManager.GetDatabase("Body Parts").Objects)
            {
                EditorManager.Instance.UnlockBodyPart(obj.Key, false);
                yield return null;
            }
            foreach (var obj in DatabaseManager.GetDatabase("Patterns").Objects)
            {
                EditorManager.Instance.UnlockPattern(obj.Key, false);
                yield return null;
            }
            NotificationsManager.Notify("You unlocked all the parts and patterns!");

            foreach (UnlockableItem item in FindObjectsOfType<UnlockableItem>())
            {
                if (item is UnlockableCollection) continue;
                Destroy(item.gameObject);
            }

            yield return new WaitForSeconds(2f);

            RoadmapMenu.Instance.Open();
        }
    }
}