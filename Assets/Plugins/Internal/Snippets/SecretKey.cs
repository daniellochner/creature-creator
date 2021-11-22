using UnityEngine;

[CreateAssetMenu(menuName = "Secret Key")]
public class SecretKey : ScriptableObject
{
    [SerializeField] private string value;
    public string Value => value;
}