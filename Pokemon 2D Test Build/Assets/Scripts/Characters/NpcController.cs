using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : Entity, IInteractable
{
    [SerializeField] Dialog dialog;

    void IInteractable.OnInteract()
    {
        //StartCoroutine(DialogManager.instance.ShowDialogBox(dialog));
        StartCoroutine(MoveToPosition(new Vector2(1, 0)));
    }

    void Update()
    {
        _anim.SetBool("isMoving", _isMoving);
        _anim.SetBool("isRunning", isRunning);
    }

}
