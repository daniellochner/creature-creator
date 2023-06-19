using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    #region Fields
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform fill;
    [SerializeField] private float speed;
    [SerializeField] private UnityEvent onEnter;

    private bool hasEntered, hasCompleted;
    private float progress;
    private Material material;
    #endregion

    #region Properties
    public bool IsHeld { get; set; }
    #endregion

    #region Methods
    private void Awake()
    {
        material = new Material(meshRenderer.sharedMaterial);
        meshRenderer.sharedMaterial = material;
    }
    private void OnDisable()
    {
        hasCompleted = hasEntered = IsHeld = false;
        progress = 0f;
    }

    private void Update()
    {
        material.SetFloat("_MinTransparency", 0.25f + Mathf.PingPong(Time.time, 0.75f));
        progress = Mathf.Clamp01(progress + ((hasEntered || IsHeld) ? speed : -speed) * Time.deltaTime);

        fill.localScale = Vector3.Lerp(new Vector3(0f, 1f, 0f), Vector3.one, progress);
        if (progress >= 1 && !hasCompleted)
        {
            onEnter.Invoke();
            hasCompleted = true;
        }
        else
        if (progress <= 0)
        {
            hasCompleted = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Local"))
        {
            hasEntered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player/Local"))
        {
            hasEntered = false;
        }
    }
    #endregion
}