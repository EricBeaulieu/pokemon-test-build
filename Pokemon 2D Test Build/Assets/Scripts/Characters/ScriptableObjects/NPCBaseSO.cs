using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/NPC")]
public class NPCBaseSO : EntityBaseSO
{
    [SerializeField] Dialog dialog;

    public Dialog GetDialog
    {
        get { return dialog; }
    }
}
