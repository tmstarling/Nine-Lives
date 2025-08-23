using UnityEngine;

public class NewDoor : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void Open()
    {
        _animator.SetTrigger("Open");
    }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }
}
