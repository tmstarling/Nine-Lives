using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LitterBoxWin1 : MonoBehaviour
{
    [Header("this script is like button functions, use ontouch to choose litterbox action")]
    [SerializeField] UnityEvent onPlayerTouch;
    [SerializeField] string nextSceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onPlayerTouch?.Invoke();
    }

    public void LoadScene() => LoadingManager.instance.StartSceneLoad(() => { }, nextSceneName);
    public void YouWin() => gamemanager.instance.youWin();
}
