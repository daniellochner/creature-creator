using UnityEditor;
using System.Collections;

public class IronSourceEditorCoroutines
{
    readonly IEnumerator mRoutine;

    public static IronSourceEditorCoroutines StartEditorCoroutine( IEnumerator routine)
    {
        IronSourceEditorCoroutines coroutine = new IronSourceEditorCoroutines(routine);
        coroutine.start();
        return coroutine;
    }

    IronSourceEditorCoroutines(IEnumerator routine)
    {
        mRoutine = routine;
    }

    void start()
    {
        EditorApplication.update += update;
    }

    void update()
    {
        if(!mRoutine.MoveNext())
        {
            StopEditorCoroutine();
        }
    }

    public void StopEditorCoroutine()
    {
        EditorApplication.update -= update;
    }
}
