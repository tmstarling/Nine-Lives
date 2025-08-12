using System;
using UnityEngine;

public class KillObjective : MonoBehaviour, IObjective
{
    [Tooltip("Must have {0} for string replacement for enemy count")]
    [TextArea]
    [SerializeField]
    string description;
    [Tooltip("defaults to this transform on awake. enemies that need to be killed must be a child of this transform.")]
    [SerializeField]
    Transform _enemiesTransform;
    bool complete;
    bool started;
    Action<string> _updateDescription;
    Action _complete;
    private void Awake()
    {
        if (_enemiesTransform == null)
            _enemiesTransform = transform;
    }

    public void Register(Action<string> updateDescription, Action complete)
    {
        _complete = complete;
        _updateDescription = updateDescription;
        started = true;
    }

    int GetCount() => _enemiesTransform.childCount;

    void Update()
    {
        if (!started) return;
        if (complete) return;
        int count = GetCount();
        _updateDescription?.Invoke(string.Format(description, count));
        if (count == 0)
        {
            _complete?.Invoke();
            complete = true;
        }
    }
}
