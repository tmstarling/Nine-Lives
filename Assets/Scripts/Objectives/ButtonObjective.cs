using System;
using UnityEngine;

public class ButtonObjective : MonoBehaviour, IObjective
{
    [SerializeField] string button;
    [TextArea][SerializeField] string description;
    Action complete;
    bool started;
    bool _complete;
    public void Register(Action<string> updateDescription, Action complete)
    {
        updateDescription?.Invoke(description);
        this.complete = complete;
        started = true;
    }

    private void Update()
    {
        if (_complete)
            return;
        if (!started)
            return;
        if (Input.GetButtonDown(button))
        {
            _complete = true;
            complete?.Invoke();
        }
    }
}
