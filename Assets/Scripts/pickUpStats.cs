using UnityEngine;

[CreateAssetMenu]

public class pickUpStats : ScriptableObject
{

    public int pickUpsCount = 0;
    public GameObject model;
    [Range(10, 50)] public int bonusHealth;
    [Range(1, 5)] public int speedBoost;
    [Range(1, 5)] public int damageBoost;
}
