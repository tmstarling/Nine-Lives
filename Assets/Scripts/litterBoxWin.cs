using UnityEngine;

public class litterBoxWin : MonoBehaviour
{
   public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

           pickUpStats stats = other.GetComponent<pickUpStats>();
            if (stats != null && stats.pickUpsCount >= 3)
            {
                gamemanager.instance.youWin();

            }

            gamemanager.instance.PlayerEnteredLitterBox();
        }
    }
}
