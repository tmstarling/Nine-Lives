using System;
using UnityEngine;

public class WASDObjective : MonoBehaviour, IObjective
{
    [Tooltip("WASD status needs {0}")]
    [TextArea][SerializeField] string description;
    Action<string> updateDescription;
    Action complete;
    bool started;
    bool _complete;
    bool wpressed;
    bool apressed;
    bool spressed;
    bool dpressed;
    public void Register(Action<string> updateDescription, Action complete)
    {
        this.updateDescription = updateDescription;
        this.complete = complete;
        started = true;
    }

    private void Update()
    {
        if (_complete)
            return;
        if (!started)
            return;
        string w;
        if (Input.GetKeyDown(KeyCode.W))
        {
            wpressed = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            apressed = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            spressed = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            dpressed = true;
        }
        if (wpressed)
            w = "<b>W</b>";
        else
            w = "W";
        string a;
        if (apressed)
            a = "<b>A</b>";
        else
            a = "A";
        string s;
        if (spressed)
            s = "<b>S</b>";
        else
            s = "S";
        string d;
        if (dpressed)
            d = "<b>D</b>";
        else
            d = "D";
        updateDescription?.Invoke(string.Format(description, string.Format("{0}{1}{2}{3}", w, a, s, d)));
        if (apressed && dpressed && wpressed && spressed)
        {
            _complete = true;
            complete?.Invoke();
        }
    }
}
