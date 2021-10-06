using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Forewarn : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Forewarn; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Forewarn(); }
    public override string Description()
    {
        return "When it enters a battle, the Pokémon can tell one of the moves an opposing Pokémon has.";
    }
    List<Move> tempMoveList = new List<Move>();
    string abilityMessage;
    public override bool ActivateAbilityUponEntry(Pokemon defendingPokemon, BattleUnit opposingTarget)
    {
        if(opposingTarget.pokemon.moves.Count <= 0)
        {
            return false;
        }

        tempMoveList.Clear();
        int highestMovePower = opposingTarget.pokemon.moves.Max(x => x.moveBase.MovePower);
        tempMoveList = opposingTarget.pokemon.moves.FindAll(x => x.moveBase.MovePower == highestMovePower);

        abilityMessage = $"It was alerted to {opposingTarget.pokemon.currentName}'s {tempMoveList[Random.Range(0, tempMoveList.Count - 1)].moveBase.MoveName}!";
        return true;
    }
    public override string OnAbilitityActivation(Pokemon pokemon)
    {
        return $"{abilityMessage}";
    }
}