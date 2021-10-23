using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SemiInvulnerableType { NA,Air,Vanished, Underground , Underwater }

public class ChargingTurn : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.ChargingTurn; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new ChargingTurn(); }
    public MoveBase chargingMove { get; set; }
    SemiInvulnerableType invulnerableType;
    public bool hitCancelsAttack { get; private set; }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{GlobalTools.ReplaceUserWithPokemonName(chargingMove.GetSpecializedMoveMessage(ConditionID.ChargingTurn), attackingPokemon)}";
    }
    public override bool CanAttackThisTurn(MoveBase originalMove,BattleUnit attackingPokemon)
    {
        if (originalMove == SpecializedMoves.solarBeam || originalMove == SpecializedMoves.solarBlade)
        {
            if (BattleSystem.GetCurrentWeather == WeatherEffectID.Sunshine)
            {
                return true;
            }
        }

        if (attackingPokemon.pokemon.GetHoldItemEffects.ExecuteMoveWithChargingTurn() == true)
        {
            attackingPokemon.removeItem = true;
            return true;
        }
        
        attackingPokemon.pokemon.SetVolatileStatus(ConditionID.ChargingTurn, originalMove, attackingPokemon);
        return false;
    }
    public void SetInvulnerableType(SemiInvulnerableType type)
    {
        invulnerableType = type;
    }
    public bool CanBeHitWhileSemiInvulnerable(MoveBase originalMove)
    {
        if (invulnerableType == SemiInvulnerableType.NA)
        {
            return true;
        }
        else if (invulnerableType == SemiInvulnerableType.Air)
        {
            if (originalMove == SpecializedMoves.gust || originalMove == SpecializedMoves.hurricane || originalMove == SpecializedMoves.twister
                || originalMove == SpecializedMoves.smackDown || originalMove == SpecializedMoves.thousandArrows)
            {
                hitCancelsAttack = true;
                return true;
            }
        }
        else if (invulnerableType == SemiInvulnerableType.Underground)
        {
            if (originalMove == SpecializedMoves.earthquake || originalMove == SpecializedMoves.magnitude || originalMove == SpecializedMoves.fissure)
            {
                return true;
            }
        }
        else if (invulnerableType == SemiInvulnerableType.Underwater)
        {
            if (originalMove == SpecializedMoves.surf || originalMove == SpecializedMoves.whirlpool)
            {
                return true;
            }
        }
        return false;
    }
    public void HitDueToTakingAim()
    {
        if (invulnerableType == SemiInvulnerableType.Air)
        {
            hitCancelsAttack = true;
        }
    }

}
