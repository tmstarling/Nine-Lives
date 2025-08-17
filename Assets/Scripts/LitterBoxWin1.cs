using UnityEngine;
using UnityEngine.SceneManagement;

public class LitterBoxWin1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("ObjectiveTesting");
        }
    }
}
