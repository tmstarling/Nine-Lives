using UnityEngine;

public interface IOpen
{
    void Open(GameObject opener);
    void Close();
    bool CanOpen(GameObject opener);
    bool IsOpen { get; }
}
