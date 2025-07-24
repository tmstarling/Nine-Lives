using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] pickUpStats stats;

    public void OnTriggerEnter(Collider other)
    {
        IPickup pickUp = other.GetComponent<IPickup>();
        if (pickUp != null)
        {
            pickUp.OnPickup(stats);
            Destroy(gameObject);
        }
    }
}

