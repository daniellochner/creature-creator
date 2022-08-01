using DanielLochner.Assets;
using UnityEngine;

public class FileBrowserMenu : Menu
{
    // TODO: Figure out better method of fixing this issue!

    public override void Open(bool instant = false)
    {
        base.Open(instant);
        SetInteractable(true);
    }
    public override void Close(bool instant = false)
    {
        base.Close(instant);
        SetInteractable(false);
    }
    private void SetInteractable(bool isInteractable)
    {
        foreach (CanvasGroup cg in GetComponentsInChildren<CanvasGroup>(true))
        {
            if (cg.transform == transform) continue;
            cg.interactable = cg.blocksRaycasts = isInteractable;
        }
    }
}