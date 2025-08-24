using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// should be first
[DefaultExecutionOrder(-1000000000)]
public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    public static LoadingManager instance { get; private set; }
    [SerializeField]
    bool animating;
    Action afterLoad;
    string sceneName;

    private void Awake()
    {
        transform.parent = null;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool StartSceneLoad(Action afterLoad, string sceneName)
    {
        if (!animating)
        {
            animating = true;
            animator.SetTrigger("Open");
            this.afterLoad = afterLoad;
            this.sceneName = sceneName;
        }
        return !animating;
    }

    public async void ResetToLastCheckpoint()
    {
        await SceneManager.LoadSceneAsync(sceneName);
        animator.SetTrigger("Close");
        afterLoad?.Invoke();
        afterLoad = null;
        animating = false;
        sceneName = null;
        Time.timeScale = 1;
    }
}
