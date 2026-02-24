using UnityEngine;

[CreateAssetMenu(menuName = "Pickups/Med Stats")]
public class medStats : ScriptableObject
{
    [Range(1, 100)]
    public int healAmount;

    public GameObject heldModelPrefab;
}
