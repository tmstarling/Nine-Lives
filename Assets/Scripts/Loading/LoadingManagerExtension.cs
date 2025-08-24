using UnityEngine;

public class LoadingManagerExtension : MonoBehaviour
{
    [SerializeField] LoadingManager manager;

    public void Load() => manager.ResetToLastCheckpoint();
}
