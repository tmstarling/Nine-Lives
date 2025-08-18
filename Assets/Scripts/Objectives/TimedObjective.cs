using UnityEngine;
using System;

public class TimedObjective : MonoBehaviour, IObjective
{
    [Header("Tooltips included, completes when checkpoint changes.")]
    [Tooltip("Needs {0} for the timer string replacement")]
    [SerializeField] [TextArea] string description;
    [SerializeField] StringTimer timer;
    bool complete;
    bool started;
    Action _complete;
    int startCheckpointID;
    public virtual void Register(Action<string> updateDescription, Action complete)
    {
        timer.updateString = (timer) => { updateDescription?.Invoke(string.Format(description, timer)); };
        _complete = complete;
        timer.complete = KillPlayer;
        StartCoroutine(timer.RunTimer());
        started = true;
        startCheckpointID = CheckpointManager.instance.GetCheckpointID();
    }

    void KillPlayer() => gamemanager.instance.playerScript.TakeDamage(int.MaxValue);

    void Update()
    {
        if (!started) return;
        if (complete) return;
        if (CheckpointManager.instance.GetCheckpointID() > startCheckpointID)
        {
            _complete?.Invoke();
            complete = true;
            StopAllCoroutines();
        }
    }
}
