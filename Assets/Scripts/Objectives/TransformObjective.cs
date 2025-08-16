using UnityEngine;
using System;
using UnityEngine.Events;

public class TransformObjective : MonoBehaviour, IObjective
{
    bool complete;
    bool started;
    Action<string> _updateDescription;
    Action _complete;
    [SerializeField] UnityEvent onComplete;
    public virtual void Register(Action<string> updateDescription, Action complete)
    {
        _complete = complete;
        _updateDescription = updateDescription;
        started = true;
    }

    protected virtual void UpdateLoop(string description, Transform transform)
    {
        if (!started) return;
        if (complete) return;
        int count = transform.childCount;
        _updateDescription?.Invoke(string.Format(description, count));
        if (count == 0)
        {
            onComplete?.Invoke();
            _complete?.Invoke();
            complete = true;
        }
    }
}
