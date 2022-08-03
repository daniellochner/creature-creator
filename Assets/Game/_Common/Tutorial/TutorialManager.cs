using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
    {
        [SerializeField] private GameObject hintPrefab;
        [SerializeField] private UnlockableBodyPart eye;
        [SerializeField] private Platform platform;

        public void StartTutorial()
        {
            StartCoroutine(TutorialRoutine());
        }
        private IEnumerator TutorialRoutine()
        {
            GameObject hint1 = AddHint(eye.transform);
            yield return new WaitUntil(() => eye.IsUnlocked);
            Destroy(hint1);

            GameObject hint2 = AddHint(platform.transform);
            hint2.transform.localScale = Vector3.one * 3f;
            yield return new WaitUntil(() => EditorManager.Instance.IsEditing);
            Destroy(hint2);
        }

        private GameObject AddHint(Transform hintT)
        {
            GameObject hint = Instantiate(hintPrefab, hintT.position, hintT.rotation);
            hint.GetComponent<LookAtConstraint>().AddSource(new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1f });

            return hint;
        }
    }
}