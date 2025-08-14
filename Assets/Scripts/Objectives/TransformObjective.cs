using UnityEngine;
using System;

public class TransformObjective : MonoBehaviour, IObjective
{
    bool complete;
    bool started;
    Action<string> _updateDescription;
    Action _complete;
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
            _complete?.Invoke();
            complete = true;
        }
    }
}
