using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, Busy, BattleOver}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit _playerBattleUnit;
    [SerializeField] BattleUnit _enemyBattleUnit;

    public event Action<bool> OnBattleOver;
    public event Action<bool> OpenPokemonParty;

    [SerializeField] BattleDialogBox _dialogBox;
    [SerializeField] ActionSelectionEventSelector _actionSelectionEventSelector;
    [SerializeField] AttackSelectionEventSelector _attackSelectionEventSelector;

    List<TurnAttackDetails> currentTurnDetails;

    PokemonParty _playerParty;
    Pokemon _wildPokemon;

    public void HandleUpdate()
    {
        //If B button is pressed go back a menu
        if(Input.GetButtonDown("Fire2"))
        {
            if(_attackSelectionEventSelector.isActiveAndEnabled == true)
            {
                EnableActionSelector(true);
                EnableAttackMoveSelector(false);
            }
        }
    }

    public void StartBattle(PokemonParty playerParty,Pokemon wildPokemon)
    {
        _playerParty = playerParty;
        _wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
        currentTurnDetails = new List<TurnAttackDetails>();
    }

    /// <summary>
    /// Goes through animations and sets up both the current pokemon and the enemy pokemon, 
    /// all available attacks along with their PP and names
    /// </summary>
    IEnumerator SetupBattle()
    {
        _playerBattleUnit.Setup(_playerParty.GetFirstHealthyPokemon());
        _enemyBattleUnit.Setup(_wildPokemon);

        _dialogBox.BattleStartSetup();
        _attackSelectionEventSelector.SetMovesList(_playerBattleUnit,_playerBattleUnit.pokemon.moves,this);

        yield return _dialogBox.TypeDialog($"A wild {_enemyBattleUnit.pokemon.currentName} has appeared!");

        PlayerActions();
    }

    #region Player Actions

    /// <summary>
    /// sets up the players action box, with the cursor/event system selected on the first box
    /// Sets up the listeners for all the current selections
    /// </summary>
    void PlayerActions()
    {
        _dialogBox.SetDialogText($"What will {_playerBattleUnit.pokemon.currentName} do?");
        EnableActionSelector(true);

        _actionSelectionEventSelector.ReturnFightButton().onClick.AddListener(delegate { PlayerActionFight(); });
        _actionSelectionEventSelector.ReturnPokemonButton().onClick.AddListener(delegate { PlayerActionPokemon(); });
    }

    /// <summary>
    /// Player Selected the Fight Button
    /// </summary>
    void PlayerActionFight()
    {
        EnableActionSelector(false);
        EnableAttackMoveSelector(true);
    }

    /// <summary>
    /// Player Selected The Pokemon Button
    /// </summary>
    void PlayerActionPokemon()
    {
        OpenPokemonParty(true);
    }

    /// <summary>
    /// Turns on the Action Box, with all available buttons (Fight,Bag,Pokemon and Run) and selects the first option
    /// </summary>
    /// <param name="enabled"></param>
    void EnableActionSelector(bool enabled)
    {
        _dialogBox.EnableActionSelector(enabled);
        if (enabled == true)
        {
            _actionSelectionEventSelector.SelectFirstBox();
        }
    }

    #endregion


    /// <summary>
    /// Turns on/off the current Dialog box and the updater for the PP system as well as the type of move it is
    /// </summary>
    /// <param name="enabled"></param>
    void EnableAttackMoveSelector(bool enabled)
    {
        _dialogBox.EnableMoveSelector(enabled);
        if (enabled == true)
        {
            _attackSelectionEventSelector.SelectFirstBox();
        }
    }

    /// <summary>
    /// Called from the attack button listener passing the correct information to process through what pokemon did what attack
    /// </summary>
    /// <param name="currentPokemon">Current Battle Unit Pokemon</param>
    /// <param name="moveBase">Current Move Being Used</param>
    public void AttackSelected(BattleUnit currentPokemon, MoveBase moveBase)
    {
        EnableAttackMoveSelector(false);
        TurnAttackDetails turnAttack = new TurnAttackDetails(moveBase, currentPokemon, _enemyBattleUnit);

        currentTurnDetails.Add(turnAttack);
        StartCoroutine(EnemyMove());
    }

    public IEnumerator EnemyMove()
    {
        //Create a check function to see if they have enough PP
        Move currentAttack = _enemyBattleUnit.pokemon.ReturnRandomMove();

        TurnAttackDetails turnAttack = new TurnAttackDetails(currentAttack.moveBase, _enemyBattleUnit, _playerBattleUnit);
        currentTurnDetails.Add(turnAttack);
        //yield return RunMove(_enemyBattleUnit, _playerBattleUnit, currentAttack.moveBase);

        yield return RunThroughTurns(currentTurnDetails);
    }

    IEnumerator RunThroughTurns(List<TurnAttackDetails> attacksChosen)
    {
        TurnAttackDetails currentAttack;

        while(attacksChosen.Count > 0)
        {
            currentAttack = attacksChosen[0];

            if (attacksChosen.Count > 1)
            {
                foreach(TurnAttackDetails attack in attacksChosen)
                {
                    if(currentAttack == attack)
                    {
                        continue;
                    }

                    if(currentAttack.currentMove.Priority > attack.currentMove.Priority)
                    {
                        continue;
                    }

                    if(currentAttack.currentMove.Priority == attack.currentMove.Priority)
                    {
                        if (currentAttack.attackingPokemon.pokemon.speed > attack.attackingPokemon.pokemon.speed)
                        {
                            continue;
                        }
                        else if (currentAttack.attackingPokemon.pokemon.speed == attack.attackingPokemon.pokemon.speed)
                        {
                            bool coinFlip = (Random.value > 0.5f);
                            if (coinFlip == true)
                            {
                                currentAttack = attack;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else //currentAttack.attackingPokemon.pokemon.speed < attack.attackingPokemon.pokemon.speed
                        {
                            currentAttack = attack;
                        }
                    }
                    else //currentAttack.currentMove.Priority < attack.currentMove.Priority
                    {
                        currentAttack = attack;
                    }
                }
            }
            if (currentAttack.attackingPokemon.pokemon.currentHitPoints > 0)
            {
                yield return RunMove(currentAttack.attackingPokemon, currentAttack.targetPokmeon, currentAttack.currentMove);
            }

            attacksChosen.Remove(currentAttack);
        }

        //Called after all attacks have been done

        yield return ApplyEffectsOnEndTurn(_playerBattleUnit);
        yield return ApplyEffectsOnEndTurn(_enemyBattleUnit);

        //Apply weather effects

        //If current pokemon has fainted then it goes to the party system and waits on the selector
        if (_playerBattleUnit.pokemon.currentHitPoints > 0)
        {
            PlayerActions();
        }
    }

    IEnumerator ApplyEffectsOnEndTurn(BattleUnit sourceUnit)
    {
        int currentHP = sourceUnit.pokemon.currentHitPoints;

        sourceUnit.pokemon.OnEndTurn();
        yield return ShowStatusChanges(sourceUnit.pokemon);
        yield return sourceUnit.HUD.UpdateHP(currentHP);

        if (sourceUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(sourceUnit);
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit,MoveBase moveBase)
    {
        //This is here incase the pokemon hits itself in confusion for the smooth animation
        int previousHP = sourceUnit.pokemon.currentHitPoints;

        bool canUseMove = sourceUnit.pokemon.OnBeforeMove();
        if(canUseMove == false)
        {
            yield return ShowStatusChanges(sourceUnit.pokemon);
            //If it hit itself in its confusion update the HUD
            yield return sourceUnit.HUD.UpdateHP(previousHP);
            yield break;
        }
        //This is here incase the pokemon has a status effect that ended such as being frozen
        yield return ShowStatusChanges(sourceUnit.pokemon);

        yield return _dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName} used {moveBase.MoveName}");


        if(CheckIfMoveHits(moveBase,sourceUnit.pokemon,targetUnit.pokemon) == true)
        {
            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            if(moveBase.Target == MoveTarget.Foe)
            {
                targetUnit.PlayHitAnimation();
            }

            if (moveBase.MoveType == MoveType.Status)
            {
                yield return RunMoveEffects(moveBase.MoveEffects, sourceUnit.pokemon, targetUnit.pokemon,moveBase.Target);
            }
            else
            {
                int hpPriorToAttack = targetUnit.pokemon.currentHitPoints;//for the animator in UpdateHP
                DamageDetails damageDetails = targetUnit.pokemon.TakeDamage(moveBase, sourceUnit.pokemon);

                yield return targetUnit.HUD.UpdateHP(hpPriorToAttack);
                yield return ShowDamageDetails(damageDetails, targetUnit);
            }

            if(moveBase.SecondaryEffects != null && moveBase.SecondaryEffects.Count > 0 && targetUnit.pokemon.currentHitPoints > 0)
            {
                foreach (var secondaryEffect in moveBase.SecondaryEffects)
                {
                    int rnd = Random.Range(1, 101);
                    if(rnd <= secondaryEffect.PercentChance)
                    {
                        yield return RunMoveEffects(secondaryEffect, sourceUnit.pokemon, targetUnit.pokemon, secondaryEffect.Target);
                    }
                }
            }
        }
        else
        {
            yield return _dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName}'s attack missed!");
        }

        if (targetUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(targetUnit);
        }
    }

    bool CheckIfMoveHits(MoveBase move,Pokemon source,Pokemon target)
    {
        if(move.AlwaysHits == true)
        {
            return true;
        }

        float moveAccuracy = move.MoveAccuracy;

        moveAccuracy *= source.accuracy;
        moveAccuracy /= target.evasion;

        return Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator PokemonHasFainted(BattleUnit targetBattleUnit)
    {
        if (targetBattleUnit.pokemon.currentHitPoints <= 0)
        {
            yield return _dialogBox.TypeDialog($"{targetBattleUnit.pokemon.currentName} has fainted");
            targetBattleUnit.PlayFaintAnimation();
            //apply experience here

            yield return new WaitForSeconds(2f);
            CheckForBattleOver(targetBattleUnit);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.isPlayerPokemon)
        {
            Pokemon nextPokemon = _playerParty.GetFirstHealthyPokemon();
            if (nextPokemon != null)
            {
                OpenPokemonParty(true);
            }
            else
            {
                OnBattleOver(false);
            }
        }
        else
        {
            OnBattleOver(true);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails,BattleUnit battleUnit)
    {
        if(damageDetails.criticalHit > 1f)
        {
            yield return _dialogBox.TypeDialog("A Critical Hit!");
        }

        if(damageDetails.typeEffectiveness == 0)
        {
            yield return _dialogBox.TypeDialog($"It doesnt effect {battleUnit.pokemon.currentName}");
        }
        else if(damageDetails.typeEffectiveness <= 0.5f)
        {
            yield return _dialogBox.TypeDialog($"It's not very effective");
        }
        else if(damageDetails.typeEffectiveness > 1f)
        {
            yield return _dialogBox.TypeDialog($"It's super effective!");
        }
    }

    public Pokemon GetCurrentPokemonInBattle
    {
        get { return _playerBattleUnit.pokemon; }
    }

    public void PlayerSwitchPokemon(Pokemon newPokemon)
    {
        StartCoroutine(SwitchPokemonIEnumerator(_playerBattleUnit,newPokemon));
        EnableActionSelector(false);
    }

    IEnumerator SwitchPokemonIEnumerator(BattleUnit battleUnit,Pokemon newPokemon)
    {
        bool currentPokemonFainted = true;
        if (battleUnit.pokemon.currentHitPoints > 0)
        {
            currentPokemonFainted = false;
            yield return _dialogBox.TypeDialog($"Come Back {battleUnit.pokemon.currentName}!");
            battleUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        battleUnit.Setup(newPokemon);

        if(battleUnit.isPlayerPokemon == true)
        {
            _attackSelectionEventSelector.SetMovesList(battleUnit, battleUnit.pokemon.moves, this);
        }

        yield return _dialogBox.TypeDialog($"Go {battleUnit.pokemon.currentName}!");

        if (battleUnit.isPlayerPokemon)
        {
            if (currentPokemonFainted == true)
            {
                PlayerActions();
            }
            else
            {
                StartCoroutine(EnemyMove());
            }
        }
        else
        {
            yield return RunThroughTurns(currentTurnDetails);
        }
    }

    public void ReturnFromPokemonPartySystem()
    {
        _actionSelectionEventSelector.SelectPokemonButton();
    }

    IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target,MoveTarget moveTarget)
    {
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Foe)
            {
                target.ApplyStatModifier(effects.Boosts);
            }
            else if (moveTarget == MoveTarget.Self)
            {
                source.ApplyStatModifier(effects.Boosts);
            }
        }

        //Status Condition
        if(effects.Status != ConditionID.NA)
        {
            target.SetStatus(effects.Status);
        }

        //Volatile Status Condition
        if (effects.Volatiletatus != ConditionID.NA)
        {
            target.SetVolatileStatus(effects.Volatiletatus);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while(pokemon.statusChanges.Count > 0)
        {
            string message = pokemon.statusChanges.Dequeue();
            yield return _dialogBox.TypeDialog(message);
        }
    }
}
