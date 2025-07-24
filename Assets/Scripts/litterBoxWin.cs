using UnityEngine;

public class litterBoxWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IPickup pickUp = other.GetComponent<IPickup>();
        if (pickUp != null)
        {
            pickUpStats stats = other.GetComponentInChildren<pickUpStats>();
            if (stats != null)
            {
             
                pickUp.OnPickup(stats);
                Destroy(gameObject);
            }
            
        }
    }
}

