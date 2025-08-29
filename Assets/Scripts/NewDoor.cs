using UnityEngine;

public class NewDoor : MonoBehaviour
{
    [SerializeField] Animator _animator;

    void Start()
    {
        if (_animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger("Open");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.Play("Open", 0, 1f);
            _animator.speed = -1f;
        }
    }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }

    public void Open()
    {
        _animator.SetTrigger("Open");
    }
}
