using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] AudioSource hoverSFX;
    [SerializeField] AudioSource clickSFX;

    public void resume()
    {
        gamemanager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpaused();
    }

    public void quit()
    {
    #if !UNITY_EDITOR
        Application.Quit();
    #else
    
        
       UnityEditor.EditorApplication.isPlaying = false;    

    #endif

    }

    public void hover()
    {
        hoverSFX.Play();
    }
    public void click()
    {
        clickSFX.Play();
    }



}




