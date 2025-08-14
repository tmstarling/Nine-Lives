using UnityEngine;

// is simple because it works the same as the enemy objective, just destroy the object to update the count
public class PickupObjectivePickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }
}
