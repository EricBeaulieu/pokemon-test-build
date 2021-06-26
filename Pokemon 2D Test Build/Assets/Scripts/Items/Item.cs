using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    [SerializeField] ItemBase itemBase;
    [SerializeField] int count;

    public ItemBase ItemBase
    {
        get { return itemBase; }
        set
        {
            itemBase = value;
        }
    }

    public int Count
    {
        get { return count; }
        set
        {
            count = value;
        }
    }
}
