using UnityEngine;
public class Testing : MonoBehaviour
{
    [ContextMenu("TEST")]
    public void TEST()
    {
        foreach (PathCreation.PathCreator path in FindObjectsOfType<PathCreation.PathCreator>(true))
        {
            Vector3[] points = path.path.localPoints;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].y = 0;
            }

            path.path.localPoints = points;
        }
    }


}