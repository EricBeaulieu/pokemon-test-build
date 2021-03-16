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
        TurnAttackDetails turnAttack = new TurnAttackDetails()
        {
            currentMove = moveBase,
            attackingPokemon = currentPokemon,
            targetPokmeon = _enemyBattleUnit
        };

        currentTurnDetails.Add(turnAttack);
        //StartCoroutine(PlayerAttackSelectedCoroutine(currentPokemon, moveBase));
        StartCoroutine(EnemyMove());
    }

    //public IEnumerator PlayerAttackSelectedCoroutine(BattleUnit currentPokemon,MoveBase moveBase)
    //{
    //    yield return RunMove(currentPokemon, _enemyBattleUnit, moveBase);
    //    //StartCoroutine(EnemyMove());
    //}

    public IEnumerator EnemyMove()
    {

        //Create a check function to see if they have enough PP
        Move currentAttack = _enemyBattleUnit.pokemon.ReturnRandomMove();

        TurnAttackDetails turnAttack = new TurnAttackDetails()
        {
            currentMove = currentAttack.moveBase,
            attackingPokemon = _enemyBattleUnit,
            targetPokmeon = _playerBattleUnit
        };
        currentTurnDetails.Add(turnAttack);
        //yield return RunMove(_enemyBattleUnit, _playerBattleUnit, currentAttack.moveBase);

        yield return ChoostFirstTurn(currentTurnDetails);

        //If current pokemon has fainted then it goes to the party system and waits on the selector
        if (_playerBattleUnit.pokemon.currentHitPoints > 0)
        {
            PlayerActions();
        }
    }

    IEnumerator ChoostFirstTurn(List<TurnAttackDetails> attacksChosen)
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
                    else
                    {
                        if(currentAttack.attackingPokemon.pokemon.speed > attack.attackingPokemon.pokemon.speed)
                        {
                            continue;
                        }
                        else if (currentAttack.attackingPokemon.pokemon.speed == attack.attackingPokemon.pokemon.speed)
                        {
                            bool coinFlip = (Random.value > 0.5f);
                            if(coinFlip == true)
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
                }
            }
            yield return RunMove(currentAttack.attackingPokemon, currentAttack.targetPokmeon, currentAttack.currentMove);
            attacksChosen.Remove(currentAttack);
        }

        //Called after all attacks have been done

        yield return ApplyEffectsOnEndTurn(_playerBattleUnit);
        yield return ApplyEffectsOnEndTurn(_enemyBattleUnit);
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

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        targetUnit.PlayHitAnimation();

        if(moveBase.MoveType == MoveType.Status)
        {
            yield return RunMoveEffects(moveBase, sourceUnit.pokemon, targetUnit.pokemon);
        }
        else
        {
            int hpPriorToAttack = targetUnit.pokemon.currentHitPoints;//for the animator in UpdateHP
            DamageDetails damageDetails = targetUnit.pokemon.TakeDamage(moveBase, sourceUnit.pokemon);

            yield return targetUnit.HUD.UpdateHP(hpPriorToAttack);
            yield return ShowDamageDetails(damageDetails, targetUnit);
        }


        if (targetUnit.pokemon.currentHitPoints <= 0)
        {
            yield return PokemonHasFainted(targetUnit);
        }
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

    public void SwitchPokemon(Pokemon newPokemon)
    {
        StartCoroutine(SwitchPokemonIEnumerator(newPokemon));
        EnableActionSelector(false);
    }

    IEnumerator SwitchPokemonIEnumerator(Pokemon newPokemon)
    {
        bool currentPokemonFainted = true;
        if (_playerBattleUnit.pokemon.currentHitPoints > 0)
        {
            currentPokemonFainted = false;
            yield return _dialogBox.TypeDialog($"Come Back {_playerBattleUnit.pokemon.currentName}!");
            _playerBattleUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        _playerBattleUnit.Setup(newPokemon);

        _attackSelectionEventSelector.SetMovesList(_playerBattleUnit, _playerBattleUnit.pokemon.moves, this);

        yield return _dialogBox.TypeDialog($"Go {newPokemon.currentName}!");

        if(currentPokemonFainted == true)
        {
            PlayerActions();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    public void ReturnFromPokemonPartySystem()
    {
        _actionSelectionEventSelector.SelectPokemonButton();
    }

    IEnumerator RunMoveEffects(MoveBase currentMove, Pokemon source, Pokemon target)
    {
        //Status Boosting
        var effects = currentMove.MoveEffects;
        if (effects.Boosts != null)
        {
            if (currentMove.Target == MoveTarget.Foe)
            {
                target.ApplyStatModifier(effects.Boosts);
            }
            else if (currentMove.Target == MoveTarget.Self)
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
