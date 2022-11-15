using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DoorPortal : Portal,IInteractable
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] BoxCollider2D _boxCollider;
    [SerializeField] bool isLocked;
    [SerializeField] Dialog doorIsLockedMessage = new Dialog(new List<string>()
    {
        "This door is locked."
    });

    void Start()
    {
        if(AlternativeScene == null)
        {
            Debug.Log($"Current Door does not have a alternative scene set, door will automatically be set to locked", gameObject);
            isLocked = true;
        }

        if (isLocked == true)
        {
            LockDoor();
        }

    }
    public IEnumerator OnInteract(Vector2 vector2)
    {
        if(isLocked)
        {
            DialogManager dialogManager = GameManager.instance.GetDialogSystem;
            if(doorIsLockedMessage.Lines.Count > 0)
            {
                yield return dialogManager.ShowDialogBox(doorIsLockedMessage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Door Called");
            canPlayerPassThrough = true;
            _spriteRenderer.enabled = true;
        }
    }

    void LockDoor()
    {
        _boxCollider.isTrigger = false;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    void UnlockDoor()
    {
        _boxCollider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Portal");
    }
}
