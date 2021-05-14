using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionID
{
    //Status
    NA, Poison, Burn, Sleep, Paralyzed, Frozen, ToxicPoison,
    //Volatile Status
    Confused, Bound, Cursed, CursedUser, Flinch,
    Infatuation,CantEscape, Embargo, Encore, HealBlock,
    Identified, LeechSeed, Nightmare,PerishSong
}

public class ConditionsDB
{
    public static void Initialization(List<ConditionBase> conditionBases)
    {
        foreach (ConditionBase condition in conditionBases)
        {
            Conditions.Add(condition.Id, condition);
        }
    }

    public static Dictionary<ConditionID, ConditionBase> Conditions = new Dictionary<ConditionID, ConditionBase>();
}