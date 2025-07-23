using UnityEngine;

public class LitterBoxWin1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.PlayerEnteredLitterBox();
        }
    }
}
