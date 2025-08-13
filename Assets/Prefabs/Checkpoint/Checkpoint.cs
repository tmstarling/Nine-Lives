using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour, IObjective
{
    [SerializeField] string objectiveDescription;
    [SerializeField] UnityEvent completeEvent;
    bool completed;
    [SerializeField] int checkpointID;
    [SerializeField] Renderer[] renderers;
    Action complete;
    private void OnTriggerEnter(Collider other)
    {
        if (completed)
            return;
        if (!other.gameObject.CompareTag("Player"))
            return;
        completed = true;
        completeEvent?.Invoke();
        CheckpointManager.instance.UpdateCheckpoint(checkpointID, transform.position);
        StartCoroutine(FlashCoroutine());
        complete?.Invoke();
    }

    IEnumerator FlashCoroutine()
    {
        foreach (Renderer r in renderers)
            r.material.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        foreach (Renderer r in renderers)
            r.material.color = Color.white;
    }

    public void Register(Action<string> updateDescription, Action complete)
    {
        this.complete = complete;
        updateDescription?.Invoke(objectiveDescription);
    }
}
