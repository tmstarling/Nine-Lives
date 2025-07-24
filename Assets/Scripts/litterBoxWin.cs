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
                gamemanager.instance.updateGameGoal(-1);
                Debug.Log("Goal updated! Player had enough pickups.");
            }
        }
    }
}
