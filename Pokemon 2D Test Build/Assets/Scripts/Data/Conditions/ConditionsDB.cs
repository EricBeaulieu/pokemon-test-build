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
    Identified, LeechSeed, Nightmare,PerishSong,Taunt,
    Torment,
    //Volatile Battle Status
    AquaRing, Bracing, ChargingTurn, CenterOfAttention, Rooting,
    MagicCoat, MagneticLevitation, Minimize, Protection, Recharging,//name must recharge!
    SemiInvulnerableTurn, Substitute, TakingAim, Thrashing, Transformed,
    Grounded,Ingrained, Telekinesis
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

    static Dictionary<ConditionID, ConditionBase> Conditions = new Dictionary<ConditionID, ConditionBase>();

    public static ConditionBase GetConditionBase(ConditionID iD)
    {
        return Conditions[iD].ReturnDerivedClassAsNew();
    } 
}