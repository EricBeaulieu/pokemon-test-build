using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveSecondaryEffects : MoveEffects
{
    [SerializeField] int percentChance;
    [SerializeField] MoveTarget target;

    public int PercentChance
    {
        get { return percentChance; }
    }

    public MoveTarget Target
    {
        get { return target; }
    }
}