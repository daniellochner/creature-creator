using UnityEngine;
public class Testing : MonoBehaviour
{

    public GameObject[] terrains;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            foreach (var smr in FindObjectsOfType<SkinnedMeshRenderer>(true))
            {
                smr.enabled = !smr.enabled;
            }
        }


        if (Input.GetKeyUp(KeyCode.O))
        {
            foreach (var t in terrains)
            {
                t.SetActive(!t.activeSelf);
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            foreach (var c in FindObjectsOfType< DanielLochner.Assets.CreatureCreator.CreatureNonPlayer>(true))
            {
                c.gameObject.SetActive(!c.gameObject.activeSelf);
            }
        }
    }


}