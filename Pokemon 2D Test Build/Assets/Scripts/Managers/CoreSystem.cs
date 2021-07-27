using UnityEngine;

public abstract class CoreSystem : MonoBehaviour
{
    [SerializeField] protected DialogBox dialogBox;

    public abstract void Initialization();
    public abstract void HandleUpdate();

    public virtual void OpenSystem(bool specifiedBool = false) { }
    protected virtual void CloseSystem() { }
}
