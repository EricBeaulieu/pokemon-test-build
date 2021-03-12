using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    BattleState _state;

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
        _state = BattleState.ActionSelection;

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
        _state = BattleState.MoveSelection;
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
        _state = BattleState.PerformMove;
        EnableAttackMoveSelector(false);
        StartCoroutine(PlayerAttackSelectedCoroutine(currentPokemon, moveBase));
    }

    public IEnumerator PlayerAttackSelectedCoroutine(BattleUnit currentPokemon,MoveBase moveBase)
    {
        yield return RunMove(currentPokemon, _enemyBattleUnit, moveBase);

        //If the Battle State was not changed by run move then perform the action
        if(_state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
    }

    public IEnumerator EnemyMove()
    {
        _state = BattleState.PerformMove;
        //Create a check function to see if they have enough PP
        Move currentAttack = _enemyBattleUnit.pokemon.ReturnRandomMove();

        yield return RunMove(_enemyBattleUnit, _playerBattleUnit, currentAttack.moveBase);

        if (_state == BattleState.PerformMove)
        {
            PlayerActions();
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit,BattleUnit targetUnit,MoveBase moveBase)
    {
        yield return _dialogBox.TypeDialog($"{sourceUnit.pokemon.currentName} used {moveBase.MoveName}");

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        targetUnit.PlayHitAnimation();

        if(moveBase.MoveType == MoveType.Status)
        {
            var effects = moveBase.MoveEffects;
            if (effects.Boosts != null)
            {
                if (moveBase.Target == MoveTarget.Foe)
                {
                    targetUnit.pokemon.ApplyStatModifier(effects.Boosts);
                }
                else if(moveBase.Target == MoveTarget.Self)
                {
                    sourceUnit.pokemon.ApplyStatModifier(effects.Boosts);
                }
            }

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
            yield return _dialogBox.TypeDialog($"{targetUnit.pokemon.currentName} has fainted");
            targetUnit.PlayFaintAnimation();
            //apply experience here

            yield return new WaitForSeconds(2f);
            CheckForBattleOver(targetUnit);
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
                _state = BattleState.Busy;
                //PlayerActions();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }

    void BattleOver(bool hasWon)
    {
        _state = BattleState.BattleOver;
        OnBattleOver(hasWon);
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

        if (_playerBattleUnit.pokemon.currentHitPoints > 0)
        {
            yield return _dialogBox.TypeDialog($"Come Back {_playerBattleUnit.pokemon.currentName}!");
            _playerBattleUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        _playerBattleUnit.Setup(newPokemon);

        _attackSelectionEventSelector.SetMovesList(_playerBattleUnit, _playerBattleUnit.pokemon.moves, this);

        yield return _dialogBox.TypeDialog($"Go {newPokemon.currentName}!");

        if(_state == BattleState.Busy)
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
}
