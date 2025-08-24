using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    [SerializeField] int points;

    private void OnDestroy()  {
        if (gameObject.scene.isLoaded)
            CheckpointManager.instance.AddScore(points);
    }
}
