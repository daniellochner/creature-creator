using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

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
        [SerializeField] private RectTransform primaryColourButtonRT;
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
        #endregion

        #region Properties
        private string MoveKeys => $"{KeybindingsManager.Data.WalkForwards}{KeybindingsManager.Data.WalkLeft}{KeybindingsManager.Data.WalkBackwards}{KeybindingsManager.Data.WalkRight}";
        private string MoveToTargetButton => "right mouse button";
        #endregion

        #region Methods
        public void Begin()
        {
            StartCoroutine(TutorialRoutine());
        }

        private IEnumerator TutorialRoutine()
        {
            EditorManager.Instance.SetVisibility(false, 0f);

            yield return TutorialItemRoutine(
                UnlockBodyPartRoutine(), 
                "Unlock a body part", 
                $"Move to the highlighted body part on the ground using {MoveKeys} or {MoveToTargetButton}.", 
                20f);

            yield return TutorialItemRoutine(
                UnlockPatternRoutine(), 
                "Unlock a pattern", 
                $"Move to the highlighted pattern on the ground using {MoveKeys} or {MoveToTargetButton}.", 
                10f);

            yield return TutorialItemRoutine(
                ReturnToEditingPlatformRoutine(), 
                "Return to an editing platform", 
                $"Move to the highlighted editing platform using {MoveKeys} or {MoveToTargetButton}.", 
                20f);

            yield return TutorialItemRoutine(
                SwitchToBuildModeRoutine(), 
                "Switch to build mode", 
                "Click on the 'Build' button at the top-center of your screen.", 
                10f);

            yield return TutorialItemRoutine(
                AttachBodyPartRoutine(), 
                "Attach a body part", 
                "Drag-and-drop the highlighted body part onto your creature's body.", 
                10f);

            yield return TutorialItemRoutine(
                RevealToolsRoutine(),
                "Reveal tools",
                "Hover over and click on your creature's body to reveal the stretch tools.",
                10f);

            yield return TutorialItemRoutine(
                AddBonesRoutine(),
                "Add bones",
                "Drag away from your creature's body to extend your spine.",
                10f);

            yield return TutorialItemRoutine(
                AddWeightRoutine(),
                "Add weight",
                "Hover over your creature's body to reveal the bone tools. Then, scroll up/down using your mouse wheel to add/remove weight.",
                10f);

            yield return TutorialItemRoutine(
                SwitchToPaintModeRoutine(),
                "Switch to paint mode",
                "Click on the 'Paint' button at the top-center of your screen.",
                10f);

            yield return TutorialItemRoutine(
                ApplyPatternRoutine(),
                "Apply a pattern",
                "Click on the highlighted pattern.",
                10f);

            yield return TutorialItemRoutine(
                SetColourRoutine(),
                "Set colour",
                "Click on either the 'Primary' or 'Secondary' buttons at the bottom-right of your screen, and choose a colour using the colour picker tool.",
                30f);

            yield return TutorialItemRoutine(
                OpenOptionsMenuRoutine(),
                "View options menu",
                "Click on the arrow icon at the bottom-center of your screen.",
                10f);

            yield return TutorialItemRoutine(
                SaveCreatureRoutine(),
                "Save creature",
                "Enter a name for your creature and then click on the 'Save' button.",
                30f);

            yield return TutorialItemRoutine(
                SwitchToPlayModeRoutine(),
                "Switch to play mode",
                "Click on the 'Play' button at the top-center of your screen.",
                10f);

            yield return new WaitForSeconds(1f);

            InformationDialog.Inform("Tutorial Complete", "Great, you now know the basics! Go ahead and explore the world!");
        }    
        private IEnumerator TutorialItemRoutine(IEnumerator tutorialRoutine, string textHintTitle, string textHintMessage, float textHintTime)
        {
            Coroutine textHintCoroutine = StartCoroutine(TextHintRoutine(textHintTitle, textHintMessage, textHintTime));
            yield return tutorialRoutine;
            StopCoroutine(textHintCoroutine);
        }
        private IEnumerator TextHintRoutine(string title, string message, float time)
        {
            yield return new WaitForSeconds(time);
            InformationDialog.Inform(title, message);
        }

        private IEnumerator UnlockBodyPartRoutine()
        {
            WorldHint hint = Instantiate(worldHintPrefab, eye.transform.position, Quaternion.identity, Dynamic.Transform);
            yield return new WaitUntil(() => eye.IsUnlocked);
            Destroy(hint.gameObject);
        }
        private IEnumerator UnlockPatternRoutine()
        {
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
            hint.Setup(0, bodyPartUI.transform, Player.Instance.Constructor.Body, false, true);

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
            pos1.parent = pos2.parent = Player.Instance.Editor.BackTool;
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

                yield return new WaitUntil(() => (bone == null) || (Player.Instance.Constructor.GetWeight(i) > 0.25f));

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
            hint.Setup(0, primaryColourButtonRT, false);

            yield return new WaitUntil(() => ColourPickerDialog.Instance.IsOpen);
            Destroy(hint.gameObject);

            yield return new WaitUntil(() => !ColourPickerDialog.Instance.IsOpen);
        }
        private IEnumerator OpenOptionsMenuRoutine()
        {
            Transform pos1 = optionsHandleRT;
            Transform pos2 = new GameObject().transform;
            pos2.parent = optionsHandleRT;
            pos2.localPosition = Vector3.up * (optionsMenu.transform as RectTransform).sizeDelta.y;
            pos2.parent = Dynamic.OverlayCanvas;

            MouseHintDrag hint = Instantiate(mouseHintDragPrefab, EditorManager.Instance.transform);
            hint.Setup(0, pos1, pos2, false, false, 0.5f, 1f, 0.5f);

            yield return new WaitUntil(() => optionsMenu.CurrentState == SimpleSideMenu.State.Open);

            Destroy(hint.gameObject);
        }
        private IEnumerator SaveCreatureRoutine()
        {
            MouseHintClick hint = Instantiate(mouseHintClickPrefab, saveButtonRT);
            hint.Setup(0, saveButtonRT, false);

            yield return new WaitUntil(() => creaturesRT.childCount > 0);

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