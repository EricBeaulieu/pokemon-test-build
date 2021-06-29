using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldItemBase
{
    public abstract HoldItemID Id { get; }
    public abstract HoldItemBase ReturnDerivedClassAsNew();
    public void RemoveItem() { }
    public virtual float AlterDamageTaken(MoveBase move) { return 1f; }
    public virtual void OnTurnEnd(Pokemon defendingPokemon) { }
    public virtual StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType) { return null; }
    public virtual MoveBase AlterUserMoveDetails(MoveBase move) { return move; }
    public virtual MoveBase AlterOpposingMoveDetails(MoveBase move) { return move; }
    public virtual bool DoublesPrizeMoneyRecieved() { return false; }
    public virtual bool PreventTheUseOfCertainMoves(MoveBase move) { return false; }
    public virtual string PreventTheUseOfCertainMoveMessage() { return ""; }
    public virtual bool BindingDamageIncreased() { return false; }
    public virtual float AlterStat(StatAttribute statAffected) { return 1f; }
}
