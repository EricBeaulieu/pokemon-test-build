using UnityEngine;

public interface ISaveable
{
    object CaptureState(bool playerSave = false);
    void RestoreState(object state);
}
