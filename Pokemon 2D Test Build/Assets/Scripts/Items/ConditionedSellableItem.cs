using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionedSellableItem
{
    [SerializeField] ItemBase itemBase;
    [Range(0,8)]
    [SerializeField] int badgesRequired;

    public ItemBase ItemBase
    {
        get { return itemBase; }
    }

    public int BadgesRequired
    {
        get { return badgesRequired; }
    }
}
