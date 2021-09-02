using System.Collections;
using UnityEngine;

public interface IInteractable
{
    IEnumerator OnInteract(Vector2 vector2);
}