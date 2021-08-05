using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveSecondaryEffects : MoveEffects
{
    [SerializeField] int percentChance;
    [SerializeField] MoveTarget target;

    public MoveSecondaryEffects(ConditionID conditionID, int chance,MoveTarget moveTarget = MoveTarget.Foe)
    {
        percentChance = chance;
        if(conditionID <= ConditionID.ToxicPoison)
        {
            status = conditionID;
        }
        else
        {
            volatileStatus = conditionID;
        }
    }

    public int PercentChance
    {
        get { return percentChance; }
        set { percentChance = value; }
    }

    public MoveTarget Target
    {
        get { return target; }
    }
}