using UnityEngine;

public interface IPickup
{
    void OnPickup(GameObject player);
    bool CanBePickedUp(GameObject player);
}
