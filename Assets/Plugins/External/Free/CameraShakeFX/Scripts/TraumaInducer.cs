 using UnityEngine;
using System.Collections;

/* Example script to apply trauma to the camera or any game object */
public class TraumaInducer : MonoBehaviour 
{
    [Tooltip("Seconds to wait before trigerring the explosion particles and the trauma effect")]
    public float Delay = 1;
    [Tooltip("Maximum stress the effect can inflict upon objects Range([0,1])")]
    public float MaximumStress = 0.6f;
    [Tooltip("Maximum distance in which objects are affected by this TraumaInducer")]
    public float Range = 45;

    private IEnumerator Start()
    {
        /* Wait for the specified delay */
        yield return new WaitForSeconds(Delay);
        /* Play all the particle system this object has */
        PlayParticles();

        /* Find all gameobjects in the scene and loop through them until we find all the nearvy stress receivers */
        var targets = UnityEngine.Object.FindObjectsOfType<GameObject>();
        for(int i = 0; i < targets.Length; ++i)
        {
            var receiver = targets[i].GetComponent<StressReceiver>();
            if(receiver == null) continue;
            float distance = Vector3.Distance(transform.position, targets[i].transform.position);
            /* Apply stress to the object, adjusted for the distance */
            if(distance > Range) continue;
            float distance01 = Mathf.Clamp01(distance / Range);
            float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
            receiver.InduceStress(stress);
        }
    }

    /* Search for all the particle system in the game objects children */
    private void PlayParticles()
    {
        var children = transform.GetComponentsInChildren<ParticleSystem>();
        for(var i  = 0; i < children.Length; ++i)
        {
            children[i].Play();
        }
        var current = GetComponent<ParticleSystem>();
        if(current != null) current.Play();
    }
}