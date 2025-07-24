using UnityEngine;

public class DestructableDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletDestroy"))
        {
           

            Destroy(other.gameObject);
        }
    }
}
