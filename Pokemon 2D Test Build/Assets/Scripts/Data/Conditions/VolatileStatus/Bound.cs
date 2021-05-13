using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Bound; } }
    public override string StartMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} {boundMove.MoveEffects.SpecialStartMessage}";
    }
    public override void OnStart(Pokemon pokemon)
    {
        StatusTime = Random.Range(4, 6);
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        if(StatusTime <= 0)
        {
            pokemon.CureVolatileStatus(Id);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} was freed from {boundMove.MoveName}!");
            return;
        }

        int damage = Mathf.FloorToInt(pokemon.maxHitPoints / 8);

        if (damage <= 0)
        {
            damage = 1;
        }
        
        pokemon.UpdateHP(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} {boundMove.MoveEffects.SpecialEndTurnMessage}");
    }
    public MoveBase SetBoundMove { set { boundMove = value; } }
    MoveBase boundMove = null;
}
