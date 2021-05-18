using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaArmor : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.MagmaArmor; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new MagmaArmor(); }
    public override string Description()
    {
        return "The Pokémon is covered with hot magma, which prevents the Pokémon from becoming frozen.";
    }
    public override bool PreventCertainStatusCondition(ConditionID iD)
    {
        if (iD == ConditionID.Frozen)
        {
            return true;
        }
        return base.PreventCertainStatusCondition(iD);
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{pokemon.currentName} Magma Armor prevents freezing";
    }
}
