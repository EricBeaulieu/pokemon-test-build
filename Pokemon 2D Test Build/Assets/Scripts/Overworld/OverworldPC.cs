using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OverworldPC : MonoBehaviour, IInteractable
{
    public IEnumerator OnInteract(Vector2 vector2)
    {
        GameManager.instance.GetPCSystem.OpenSystem();
        yield return null;
    }
}
