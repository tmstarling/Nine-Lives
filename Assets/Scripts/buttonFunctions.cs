using UnityEngine;

public class buttonFunctions : MonoBehaviour
{ 
    public void resume()
    {
        gamemanager.instance.stateUnpaused();
    }

    public void restart()
    {
        if (CheckpointManager.instance == null) 
        {
            Debug.LogError("CheckpointManager.instance is null");
        }
        else
        {
            CheckpointManager.instance.ResetToLastCheckpoint();
            gamemanager.instance.stateUnpaused();
        }
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




