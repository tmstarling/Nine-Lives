using UnityEngine;

public interface IPickup
{
    void OnPickup(pickUpStats stats);
    bool CanBePickedUp(GameObject player);
}
