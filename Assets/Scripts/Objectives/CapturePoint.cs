using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [Header("Must be a transform child of the objective.")] 
    CaptureObjective objective;
    public StringTimer timer;
    [HideInInspector] public bool finished;
    [SerializeField] Renderer _renderer;
    bool playerColliding;
    bool startedCapture = false;
    List<GameObject> enemies = new List<GameObject>();
    private void Awake()
    {
        objective = GetComponentInParent<CaptureObjective>();
        if (objective == null)
            return;
        timer.complete = Finished;
        timer.updateString = objective.SetDescription;
        _renderer.material.color = Color.red;
    }

    void Finished()
    {
        _renderer.material.color = Color.green;
        finished = true;
        objective.SetToCapturesLeft();
    }

    void StopCapture()
    {
        startedCapture = false;
        _renderer.material.color = Color.red;
        StopAllCoroutines();
    }

    private void StartCapture()
    {
        if (startedCapture)
            return;
        _renderer.material.color = Color.white;
        StartCoroutine(timer.RunTimer());
        startedCapture = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (objective == null)
            return;
        if (finished) return;
        if (other.CompareTag("Player"))
        {
            StartCapture();
            playerColliding = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;
        if (other.isTrigger) return;
        if (finished) return;
        if (!enemies.Contains(other.gameObject))
        {
            enemies.Add(other.gameObject);
            StopCapture();
            objective.SetToEnemiesInvading();
        }
    }

    private void Update()
    {
        if (finished) return;
        for (int i = enemies.Count - 1; i >= 0; i--)
            if (enemies[i] == null)
                enemies.RemoveAt(i);
        if (enemies.Count == 0 && playerColliding)
            StartCapture();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;
        if (objective == null)
            return;
        if (finished) return;
        if (other.CompareTag("Player"))
        {
            StopCapture();
            objective.SetToCapturesLeft();
            playerColliding = false;
        }
        if (other.CompareTag("Enemy"))
            if (enemies.Contains(other.gameObject))
                enemies.Remove(other.gameObject);
    }
}