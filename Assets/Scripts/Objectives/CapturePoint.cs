using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [Header("Must be a transform child of the objective.")] CaptureObjective objective;
    public StringTimer timer;
    [HideInInspector] public bool finished;
    [SerializeField] Renderer _renderer;
    private void Awake()
    {
        objective = GetComponentInParent<CaptureObjective>();
        timer.complete = Finished;
        timer.updateString = objective.SetDescription;
        _renderer.material.color = Color.red;
    }
    void Finished() => finished = true;
    private void OnTriggerEnter(Collider other)
    {
        if (finished) return;
        if (!other.CompareTag("Player")) return;
        StartCoroutine(timer.RunTimer());
        _renderer.material.color = Color.white;
    }
}