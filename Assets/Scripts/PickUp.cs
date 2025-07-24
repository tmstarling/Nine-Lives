using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] pickUpStats stats;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickUp = other.GetComponent<IPickup>();
        if (pickUp != null && pickUp.CanBePickedUp(other.gameObject))
        {
            pickUp.OnPickup(stats);
            Destroy(gameObject);
        }
    }
}

