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
        private string MoveToTargetButton => $"{KeyCode.Mouse1}";

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
                $"(1/14) {LocalizationUtility.Localize("tutorial_1_title")}",
                LocalizationUtility.Localize("tutorial_1_message", MoveKeys, MoveToTargetButton),
                20f);

            yield return TutorialItemRoutine(
                UnlockPatternRoutine(),
                $"(2/14) {LocalizationUtility.Localize("tutorial_2_title")}",
                LocalizationUtility.Localize("tutorial_2_message", MoveKeys, MoveToTargetButton),
                10f);

            yield return TutorialItemRoutine(
                ReturnToEditingPlatformRoutine(),
                $"(3/14) {LocalizationUtility.Localize("tutorial_3_title")}",
                LocalizationUtility.Localize("tutorial_3_message", MoveKeys, MoveToTargetButton),
                20f);

            yield return TutorialItemRoutine(
                SwitchToBuildModeRoutine(),
                $"(4/14) {LocalizationUtility.Localize("tutorial_4_title")}",
                LocalizationUtility.Localize("tutorial_4_message"),
                10f);

            yield return TutorialItemRoutine(
                AttachBodyPartRoutine(),
                $"(5/14) {LocalizationUtility.Localize("tutorial_5_title")}",
                LocalizationUtility.Localize("tutorial_5_message"),
                15f);

            yield return TutorialItemRoutine(
                RevealToolsRoutine(),
                $"(6/14) {LocalizationUtility.Localize("tutorial_6_title")}",
                LocalizationUtility.Localize("tutorial_6_message"),
                10f);

            yield return TutorialItemRoutine(
                AddBonesRoutine(),
                $"(7/14) {LocalizationUtility.Localize("tutorial_7_title")}",
                LocalizationUtility.Localize("tutorial_7_message"),
                10f);

            yield return TutorialItemRoutine(
                AddWeightRoutine(),
                $"(8/14) {LocalizationUtility.Localize("tutorial_8_title")}",
                LocalizationUtility.Localize("tutorial_8_message"),
                20f);

            yield return TutorialItemRoutine(
                SwitchToPaintModeRoutine(),
                $"(9/14) {LocalizationUtility.Localize("tutorial_9_title")}",
                LocalizationUtility.Localize("tutorial_9_message"),
                10f);

            yield return TutorialItemRoutine(
                ApplyPatternRoutine(),
                $"(10/14) {LocalizationUtility.Localize("tutorial_10_title")}",
                LocalizationUtility.Localize("tutorial_10_message"),
                10f);

            yield return TutorialItemRoutine(
                SetColourRoutine(),
                $"(11/14) {LocalizationUtility.Localize("tutorial_11_title")}",
                LocalizationUtility.Localize("tutorial_11_message"),
                30f);

            yield return TutorialItemRoutine(
                ViewOptionsMenuRoutine(),
                $"(12/14) {LocalizationUtility.Localize("tutorial_12_title")}",
                LocalizationUtility.Localize("tutorial_12_message"),
                15f);

            yield return TutorialItemRoutine(
                SaveCreatureRoutine(),
                $"(13/14) {LocalizationUtility.Localize("tutorial_13_title")}",
                LocalizationUtility.Localize("tutorial_13_message"),
                30f);

            yield return TutorialItemRoutine(
                SwitchToPlayModeRoutine(),
                $"(14/14) {LocalizationUtility.Localize("tutorial_14_title")}",
                LocalizationUtility.Localize("tutorial_14_message"),
                10f);

            yield return new WaitForSeconds(1f);

            InformationDialog.Inform(LocalizationUtility.Localize("tutorial_complete_title"), LocalizationUtility.Localize("tutorial_complete_message"));
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
            int remindTime = 300; // remind every 5 minutes if they still haven't completed the tutorial
            yield return new WaitForSeconds(remindTime);
            while (!IsComplete)
            {
                InformationDialog.Inform(LocalizationUtility.Localize("tutorial-in-progress_title"), LocalizationUtility.Localize("tutorial-in-progress_message"));
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
            BodyPartUI bodyPartUI = System.Array.Find(bodyPartsRT.GetComponentsInChildren<BodyPartUI>(), x => x.BodyPart is Eye);
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

                yield return new WaitUntil(() => (bone == null) || (Player.Instance.Constructor.GetWeight(i) > 10f));

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
            RectTransform pos1 = optionsHandleRT;
            RectTransform pos2 = new GameObject("TMP", typeof(RectTransform)).transform as RectTransform;
            pos2.parent = EditorManager.Instance.transform;
            pos2.anchorMin = pos2.anchorMax = new Vector2(0.5f, 0f);
            pos2.anchoredPosition = Vector2.up * ((optionsMenu.transform as RectTransform).rect.height + (45f / 2f));

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