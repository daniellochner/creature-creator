using System.Collections;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
    {
        #region Fields
        [SerializeField] private WorldHint worldHintPrefab;
        [SerializeField] private MouseHintClick mouseHintClickPrefab;
        [SerializeField] private MouseHintDrag mouseHintDragPrefab;
        [SerializeField] private MouseHintScroll mouseHintScrollPrefab;
        [Space]
        [SerializeField] private RectTransform buildMenuRT;
        [SerializeField] private RectTransform buildButtonRT;
        [SerializeField] private RectTransform bodyPartsRT;
        [SerializeField] private RectTransform paintMenuRT;
        [SerializeField] private RectTransform paintButtonRT;
        [SerializeField] private RectTransform patternsRT;
        [SerializeField] private RectTransform secondaryColourButtonRT;
        [SerializeField] private RectTransform playMenuRT;
        [SerializeField] private RectTransform playButtonRT;
        [SerializeField] private RectTransform optionsHandleRT;
        [SerializeField] private RectTransform saveButtonRT;
        [SerializeField] private RectTransform creaturesRT;
        [SerializeField] private SimpleSideMenu optionsMenu;
        [Space]
        [SerializeField] private UnlockableBodyPart eye;
        [SerializeField] private UnlockablePattern pattern;
        [SerializeField] private Platform platform;

        private Coroutine tutorialCoroutine;
        private string currentTutorialItem;
        #endregion

        #region Properties
        private string MoveKeys => $"{KeybindingsManager.Data.WalkForwards}{KeybindingsManager.Data.WalkLeft}{KeybindingsManager.Data.WalkBackwards}{KeybindingsManager.Data.WalkRight}";
        private string MoveToTargetButton => "right mouse button";

        public bool IsComplete { get; private set; }
        #endregion

        #region Methods
        public void Begin()
        {
            tutorialCoroutine = StartCoroutine(TutorialRoutine());
        }

        private IEnumerator TutorialRoutine()
        {
            IsComplete = false;
            StartCoroutine(RemindTutorialRoutine());
            EditorManager.Instance.SetVisibility(false, 0f);

            yield return TutorialItemRoutine(
                UnlockBodyPartRoutine(), 
                "(1/14) Unlock A Body Part", 
                $"Move to the highlighted body part on the ground using {MoveKeys} or {MoveToTargetButton}.", 
                20f);

            yield return TutorialItemRoutine(
                UnlockPatternRoutine(),
                "(2/14) Unlock A Pattern", 
                $"Move to the highlighted pattern on the ground using {MoveKeys} or {MoveToTargetButton}.", 
                10f);

            yield return TutorialItemRoutine(
                ReturnToEditingPlatformRoutine(),
                "(3/14) Return To An Editing Platform", 
                $"Move to the highlighted editing platform using {MoveKeys} or {MoveToTargetButton}.", 
                20f);

            yield return TutorialItemRoutine(
                SwitchToBuildModeRoutine(),
                "(4/14) Switch To Build Mode", 
                "Click on the 'Build' button at the top-center of your screen.", 
                10f);

            yield return TutorialItemRoutine(
                AttachBodyPartRoutine(),
                "(5/14) Attach A Body Part", 
                "Drag-and-drop a body part onto your creature's body.", 
                15f);

            yield return TutorialItemRoutine(
                RevealToolsRoutine(),
                "(6/14) Reveal Build Tools",
                "Hover over and click on your creature's body to reveal the stretch tools.",
                10f);

            yield return TutorialItemRoutine(
                AddBonesRoutine(),
                "(7/14) Add Bones",
                "Drag the stretch tools to extend your creature's spine.",
                10f);

            yield return TutorialItemRoutine(
                AddWeightRoutine(),
                "(8/14) Add Weight",
                "Hover over your creature's body to reveal the bone tools. Then, scroll up/down using your mouse wheel to add/remove weight.",
                20f);

            yield return TutorialItemRoutine(
                SwitchToPaintModeRoutine(),
                "(9/14) Switch To Paint Mode",
                "Click on the 'Paint' button at the top-center of your screen.",
                10f);

            yield return TutorialItemRoutine(
                ApplyPatternRoutine(),
                "(10/14) Apply A Pattern",
                "Click on a pattern to apply it to your creature.",
                10f);

            yield return TutorialItemRoutine(
                SetColourRoutine(),
                "(11/14) Set A Colour",
                "Click on the 'Primary' or 'Secondary' button at the bottom-right of your screen, and choose a colour using the colour picker tool.",
                30f);

            yield return TutorialItemRoutine(
                ViewOptionsMenuRoutine(),
                "(12/14) View Options Menu",
                "Drag the menu handle, at the bottom-center of your screen, upwards. You can also click on it once to toggle the menu's state.",
                15f);

            yield return TutorialItemRoutine(
                SaveCreatureRoutine(),
                "(13/14) Save Creature",
                "Enter a name for your creature and then click on the 'Save' button.",
                30f);

            yield return TutorialItemRoutine(
                SwitchToPlayModeRoutine(),
                "(14/14) Switch To Play Mode",
                "Click on the 'Play' button at the top-center of your screen.",
                10f);

            yield return new WaitForSeconds(1f);

            InformationDialog.Inform("Tutorial Complete!", "Great, you now know the basics! Go ahead and explore the world!");
            IsComplete = true;
        }
        private IEnumerator TutorialItemRoutine(IEnumerator tutorialRoutine, string textHintTitle, string textHintMessage, float textHintTime)
        {
            currentTutorialItem = textHintTitle;

            Coroutine textHintCoroutine = StartCoroutine(TextHintRoutine(textHintTitle, textHintMessage, textHintTime));
            yield return tutorialRoutine;
            StopCoroutine(textHintCoroutine);
        }
        private IEnumerator TextHintRoutine(string title, string message, float time)
        {
            yield return new WaitForSeconds(time);
            InformationDialog.Inform(title, message);
        }
        private IEnumerator RemindTutorialRoutine()
        {
            int remindTime = 600; // remind every 10 minutes if they still haven't completed the tutorial
            yield return new WaitForSeconds(remindTime);
            while (!IsComplete)
            {
                InformationDialog.Inform("Tutorial In Progress", "Looks like you're still busy with the tutorial! Head back to an editing platform to complete it!");
                yield return new WaitForSeconds(remindTime);
            }
        }

        private IEnumerator UnlockBodyPartRoutine()
        {
            if (eye == null)
            {
                yield break;
            }

            WorldHint hint = Instantiate(worldHintPrefab, eye.transform.position, Quaternion.identity, Dynamic.Transform);
            yield return new WaitUntil(() => eye.IsUnlocked);
            Destroy(hint.gameObject);
        }
        private IEnumerator UnlockPatternRoutine()
        {
            if (pattern == null)
            {
                yield break;
            }

            WorldHint hint = Instantiate(worldHintPrefab, pattern.transform.position, Quaternion.identity, Dynamic.Transform);
            yield return new WaitUntil(() => pattern.IsUnlocked);
            Destroy(hint.gameObject);
        }
        private IEnumerator ReturnToEditingPlatformRoutine()
        {
            WorldHint hint = Instantiate(worldHintPrefab, platform.transform.position, Quaternion.identity, Dynamic.Transform);
            hint.transform.localScale = Vector3.one * 2.5f;
            yield return new WaitUntil(() => EditorManager.Instance.IsEditing);
            Destroy(hint.gameObject);
            EditorManager.Instance.SetVisibility(true);
        }
        private IEnumerator SwitchToBuildModeRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, buildButtonRT);
            hint.Setup(0, buildButtonRT, false);

            yield return new WaitUntil(() => EditorManager.Instance.IsBuilding);
            Destroy(hint.gameObject);
        }
        private IEnumerator AttachBodyPartRoutine()
        {
            BodyPartUI bodyPartUI = bodyPartsRT.GetComponentInChildren<BodyPartUI>();
            MouseHintDrag hint = Instantiate(mouseHintDragPrefab, buildMenuRT);
            hint.Setup(0, bodyPartUI.transform, Player.Instance.Constructor.Body, false, true, 1f, 1f, 1f);

            yield return new WaitUntil(() => Player.Instance.Constructor.Data.AttachedBodyParts.Count > 0);
            Destroy(hint.gameObject);
        }
        private IEnumerator RevealToolsRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, buildMenuRT);
            hint.Setup(0, Player.Instance.Constructor.Body, true);

            yield return new WaitUntil(() => Player.Instance.Editor.IsSelected);
            Destroy(hint.gameObject);
        }
        private IEnumerator AddBonesRoutine()
        {
            Transform pos1 = new GameObject().transform;
            Transform pos2 = new GameObject().transform;
            pos1.parent = pos2.parent = Player.Instance.Editor.BTool;
            pos1.localPosition = 0.5f * Vector3.forward;
            pos2.localPosition = 1.0f * Vector3.forward;

            MouseHintDrag hint = Instantiate(mouseHintDragPrefab, buildMenuRT);
            hint.Setup(0, pos1, pos2, true, true, 0.5f, 1f, 0.5f);

            yield return new WaitUntil(() => Player.Instance.Constructor.Bones.Count > 2);

            Destroy(pos1.gameObject);
            Destroy(pos2.gameObject);
            Destroy(hint.gameObject);
        }
        private IEnumerator AddWeightRoutine()
        {
            for (int i = 0; i < Player.Instance.Constructor.Bones.Count; i++)
            {
                MouseHintScroll hint = Instantiate(mouseHintScrollPrefab, buildMenuRT);
                Transform bone = Player.Instance.Constructor.Bones[i];
                hint.Setup(1, bone, true);

                yield return new WaitUntil(() => (bone == null) || (Player.Instance.Constructor.GetWeight(i) > 25f));

                Destroy(hint.gameObject);
            }
        }
        private IEnumerator SwitchToPaintModeRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, paintButtonRT);
            hint.Setup(0, paintButtonRT, false);

            yield return new WaitUntil(() => EditorManager.Instance.IsPainting);
            Destroy(hint.gameObject);
        }
        private IEnumerator ApplyPatternRoutine()
        {
            PatternUI patternUI = patternsRT.GetComponentInChildren<PatternUI>();
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, paintMenuRT.transform);
            hint.Setup(0, patternUI.transform, false);

            yield return new WaitUntil(() => !string.IsNullOrEmpty(Player.Instance.Constructor.Data.PatternID));

            Destroy(hint.gameObject);
        }
        private IEnumerator SetColourRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, paintMenuRT.transform);
            hint.Setup(0, secondaryColourButtonRT, false);

            yield return new WaitUntil(() => ColourPickerDialog.Instance.IsOpen);
            Destroy(hint.gameObject);

            yield return new WaitUntil(() => !ColourPickerDialog.Instance.IsOpen);
        }
        private IEnumerator ViewOptionsMenuRoutine()
        {
            Transform pos1 = optionsHandleRT;
            Transform pos2 = new GameObject().transform;
            pos2.parent = EditorManager.Instance.transform;
            pos2.position = pos1.position + (Vector3.up * (optionsMenu.transform as RectTransform).rect.height);

            MouseHintDrag hint = Instantiate(mouseHintDragPrefab, EditorManager.Instance.transform);
            hint.Setup(0, pos1, pos2, false, false, 0.5f, 1f, 0.5f);

            yield return new WaitUntil(() => optionsMenu.CurrentState == SimpleSideMenu.State.Open);

            Destroy(pos2.gameObject);
            Destroy(hint.gameObject);
        }
        private IEnumerator SaveCreatureRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, EditorManager.Instance.transform);
            hint.Setup(0, saveButtonRT, false);

            int prevCount = creaturesRT.childCount;
            yield return new WaitUntil(() => creaturesRT.childCount > prevCount);

            Destroy(hint.gameObject);
        }
        private IEnumerator SwitchToPlayModeRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, playButtonRT);
            hint.Setup(0, playButtonRT, false);

            yield return new WaitUntil(() => EditorManager.Instance.IsPlaying);
            Destroy(hint.gameObject);
        }
        #endregion
    }
}