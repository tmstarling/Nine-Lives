using UnityEngine;

public interface IDamage
{
    void TakeDamage(int amount);
    void TakeDamage(int amount, Vector3 damageSourcePosition);
}
