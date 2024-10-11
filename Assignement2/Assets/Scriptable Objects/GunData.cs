using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    public string gunName;
    public Sprite gunIcon; // Assign the gun icon in the inspector
    public GameObject gunPrefab;
}
