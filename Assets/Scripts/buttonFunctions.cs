using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{ 
    public void resume()
    {
        gamemanager.instance.stateUnpaused();
    }

    public void restart()
    {
        CheckpointManager.instance.ResetToLastCheckpoint();
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
    
        
    
}




