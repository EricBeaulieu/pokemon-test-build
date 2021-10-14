using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentalHerb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.MentalHerb; } }
    public override bool RemovesMoveBindingEffectsAfterMoveUsed(BattleUnit holder)
    {
        for (int i = 0; i < holder.pokemon.moves.Count; i++)
        {
            if(holder.pokemon.moves[i].disabled == true)
            {
                holder.removeItem = true;
                holder.pokemon.moves[i].disabled = false;
                return true;
            }
        }

        if(holder.pokemon.HasCurrentVolatileStatus(ConditionID.Infatuation) == true)
        {
            holder.removeItem = true;
            holder.pokemon.CureVolatileStatus(ConditionID.Infatuation);
            return true;
        }

        if (holder.pokemon.HasCurrentVolatileStatus(ConditionID.Encore) == true)
        {
            holder.removeItem = true;
            holder.pokemon.CureVolatileStatus(ConditionID.Encore);
            return true;
        }

        return base.RemovesMoveBindingEffectsAfterMoveUsed(holder);
    }
}
