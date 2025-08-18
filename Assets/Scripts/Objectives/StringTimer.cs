using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class StringTimer
{
    public int timerSeconds;
    public int timerMinutes;
    public int timerHours;
    public Action<string> updateString;
    public Action complete;
    string FormatFromSeconds(int remaining)
    {
        if (remaining >= 1 * 60 * 60)
        {
            string seconds = (remaining % 60).ToString();
            string minutes = (remaining / 60 % 60).ToString();
            string hours =  (remaining / 60 / 60).ToString();
            if (seconds.Length == 1)
                seconds = "0" + seconds;
            if (minutes.Length == 1)
                minutes = "0" + minutes;
            return string.Format("{2}:{1}:{0}", seconds, minutes, hours);
        }
        else if (remaining >= 60)
        {
            string seconds = (remaining % 60).ToString();
            string minutes = (remaining / 60 % 60).ToString();
            if (seconds.Length == 1)
                seconds = "0" + seconds;
            return string.Format("{1}:{0}", seconds, minutes);
        }
        return (remaining % 60).ToString();
    }

    public IEnumerator RunTimer()
    {
        for (int i = timerSeconds + timerMinutes * 60 + timerHours * 60 * 60; i > 0; i--)
        {
            updateString?.Invoke(FormatFromSeconds(i));
            yield return new WaitForSeconds(1.0f);
        }
        complete?.Invoke();
    }
}
