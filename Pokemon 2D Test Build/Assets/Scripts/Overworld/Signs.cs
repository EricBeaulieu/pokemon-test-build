using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signs : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

    public IEnumerator OnInteract(Vector2 vector2)
    {
        yield return GameManager.instance.GetDialogSystem.ShowDialogBox(dialog);
    }

}
