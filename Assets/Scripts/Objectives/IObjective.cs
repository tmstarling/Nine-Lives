using System;

public interface IObjective
{
    public void Register(Action<string> updateDescription, Action complete);
}
