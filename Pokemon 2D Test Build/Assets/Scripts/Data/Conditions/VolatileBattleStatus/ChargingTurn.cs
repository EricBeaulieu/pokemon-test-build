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
        if (originalMove.MoveName == "Solar Beam" || originalMove.MoveName == "Solar Blade")
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
            if (originalMove.MoveName == "Gust" || originalMove.MoveName == "Hurricane" || originalMove.MoveName == "Twister" 
                //|| originalMove == SpecializedMoves.smackDown || originalMove == SpecializedMoves.thousandArrows 
                || originalMove.MoveName == "Thunder")
            {
                hitCancelsAttack = true;
                return true;
            }
        }
        else if (invulnerableType == SemiInvulnerableType.Underground)
        {
            if (originalMove.MoveName == "Earthquake" || originalMove.MoveName == "Earth Power" 
                || originalMove.MoveName == "Magnitude" || originalMove.MoveName == "Fissure")
            {
                originalMove.AdjustedMovePower(100);
                if(originalMove.MoveName == "Fissure")
                {
                    originalMove.AdjustedMoveAccuracy(50);
                }

                return true;
            }
        }
        else if (invulnerableType == SemiInvulnerableType.Underwater)
        {
            if (originalMove.MoveName == "Surf" || originalMove.MoveName == "Whirlpool")
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
