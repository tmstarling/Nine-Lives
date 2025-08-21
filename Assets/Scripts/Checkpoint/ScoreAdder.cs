using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    [SerializeField] int points;

    private void OnDestroy() => CheckpointManager.instance.AddScore(points);
}
