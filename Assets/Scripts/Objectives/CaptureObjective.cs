using UnityEngine;
using System;
public class CaptureObjective : MonoBehaviour, IObjective
{
    [Header("Objective points are children of this transform.")][Header("Tooltips included. All child points must be captured to progress.")][Tooltip("Needs {0} for the timer string replacement")][SerializeField][TextArea] 
    string descriptionCapturingTime;
    [Tooltip("Needs {0} for the number of captures left string replacement")][SerializeField][TextArea] 
    string descriptionCapturesLeft;
    [SerializeField]
    [TextArea]
    string descriptionEnemiesInvading;
    bool complete;
    bool started;
    Action _complete;
    CapturePoint[] points;
    [HideInInspector] public Action<string> _updateDescription;
    private void Awake() => points = GetComponentsInChildren<CapturePoint>();
    public virtual void Register(Action<string> updateDescription, Action complete)
    {
        _complete = complete;
        _updateDescription = updateDescription;
        started = true;
        SetToCapturesLeft();
    }
    public void SetDescription(string timer) => _updateDescription?.Invoke(string.Format(descriptionCapturingTime, timer));
    int CapturesLeft()
    {
        int finsihed = 0;
        foreach (var point in points) if (!point.finished) finsihed++;
        return finsihed;
    }
    public void SetToCapturesLeft() => _updateDescription?.Invoke(string.Format(descriptionCapturesLeft, CapturesLeft()));

    public void SetToEnemiesInvading() => _updateDescription?.Invoke(descriptionEnemiesInvading);
    void Update()
    {
        if (!started) return;
        if (complete) return;
        if (CapturesLeft() == 0)
        {
            _complete?.Invoke();
            complete = true;
        }
    }
}