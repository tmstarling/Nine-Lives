using UnityEngine;

public class litterBoxWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gamemanager.instance.PlayerEnteredLitterBox();

            gamemanager.instance.youWin();
        }
    }
}
