using UnityEngine;

public class litterBoxWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           pickUpStats stats = other.GetComponent<pickUpStats>();
            if (stats != null && stats.pickUpsCount >= 3)
            {
                gamemanager.instance.youWin();
                
            }
        }
    }
}
