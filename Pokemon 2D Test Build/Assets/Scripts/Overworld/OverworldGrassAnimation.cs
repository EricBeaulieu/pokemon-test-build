using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldGrassAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Entity>() == true)
        {
            animator.SetTrigger("Entered");
        }
    }
}
