using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentalHerb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.MentalHerb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new MentalHerb(); }
    public override bool RemovesMoveBindingEffectsAfterMoveUsed(Pokemon defendingPokemon)
    {
        for (int i = 0; i < defendingPokemon.moves.Count; i++)
        {
            if(defendingPokemon.moves[i].disabled == true)
            {
                RemoveItem = true;
                defendingPokemon.moves[i].disabled = false;
                return true;
            }
        }

        if(defendingPokemon.volatileStatus.Exists(x => x.Id == ConditionID.Infatuation))
        {
            RemoveItem = true;
            defendingPokemon.CureVolatileStatus(ConditionID.Infatuation);
            return true;
        }

        if (defendingPokemon.volatileStatus.Exists(x => x.Id == ConditionID.Encore))
        {
            RemoveItem = true;
            defendingPokemon.CureVolatileStatus(ConditionID.Encore);
            return true;
        }

        return base.RemovesMoveBindingEffectsAfterMoveUsed(defendingPokemon);
    }
}
