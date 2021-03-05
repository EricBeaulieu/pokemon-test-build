using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyAction, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit _playerBattleUnit;
    [SerializeField] BattleHUD _playerBattleHud;
    [SerializeField] BattleUnit _enemyBattleUnit;
    [SerializeField] BattleHUD _enemyBattleHud;

    [SerializeField] BattleDialogBox _dialogBox;
    [SerializeField] ActionSelectionEventSelector _actionSelectionEventSelector;
    [SerializeField] AttackSelectionEventSelector _attackSelectionEventSelector;

    BattleState _state;

    void Awake()
    {
        _attackSelectionEventSelector.SetReferenceToBattleSystem(this);

    }

    void Start()
    {
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        _playerBattleUnit.Setup();
        _playerBattleHud.SetData(_playerBattleUnit.pokemon,false);
        _enemyBattleUnit.Setup();
        _enemyBattleHud.SetData(_enemyBattleUnit.pokemon, true);

        _dialogBox.BattleStartSetup();
        _attackSelectionEventSelector.SetReferenceToCurrentPokemon(_playerBattleUnit);
        _attackSelectionEventSelector.SetMovesList(_playerBattleUnit.pokemon.moves);

        yield return _dialogBox.TypeDialog($"A wild {_enemyBattleUnit.pokemon.currentName} has appeared!");

        PlayerActions();
    }

    void PlayerActions()
    {
        _state = BattleState.PlayerAction;

        StartCoroutine(_dialogBox.TypeDialog($"What will {_playerBattleUnit.pokemon.currentName} do?"));
        EnableActionSelector(true);

        _actionSelectionEventSelector.ReturnFightButton().onClick.AddListener(delegate { PlayerFight(); });
    }

    void PlayerFight()//Player Selected the Fight Button
    {
        _state = BattleState.PlayerMove;

        EnableActionSelector(false);
        EnableMoveSelector(true);
    }

    void EnableActionSelector(bool enabled)
    {
        _dialogBox.EnableActionSelector(enabled);
        if(enabled == true)
        {
            _actionSelectionEventSelector.SelectFirstBox();
        }
    }

    void EnableMoveSelector(bool enabled)
    {
        _dialogBox.EnableMoveSelector(enabled);
        if (enabled == true)
        {
            _attackSelectionEventSelector.SelectFirstBox();
        }
    }

    public void AttackSelected(Pokemon pokemon, MoveBase moveBase)
    {
        EnableMoveSelector(false);
        StartCoroutine(AttackSelectedCoroutine(pokemon, moveBase));
    }

    public IEnumerator AttackSelectedCoroutine(Pokemon pokemon,MoveBase moveBase)
    {
        yield return _dialogBox.TypeDialog($"{pokemon.currentName} used {moveBase.moveName}");

        _playerBattleUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        _enemyBattleUnit.PlayHitAnimation();
        int hpPriorToAttack = _enemyBattleUnit.pokemon.currentHitPoints;//for the animator in UpdateHP
        DamageDetails damageDetails = _enemyBattleUnit.pokemon.TakeDamage(moveBase, pokemon);

        yield return _enemyBattleHud.UpdateHP(hpPriorToAttack);
        yield return ShowDamageDetails(damageDetails, _enemyBattleUnit);

        if(damageDetails.hasFainted == true)
        {
            yield return _dialogBox.TypeDialog($"{_enemyBattleUnit.pokemon.currentName} has fainted");
            _enemyBattleUnit.PlayFaintAnimation();
            //apply experience here
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    public IEnumerator EnemyMove()
    {
        Move currentAttack = _enemyBattleUnit.pokemon.ReturnRandomMove();

        yield return _dialogBox.TypeDialog($"{_enemyBattleUnit.pokemon.currentName} used {currentAttack.moveBase.moveName}");

        _enemyBattleUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        _playerBattleUnit.PlayHitAnimation();
        int hpPriorToAttack = _playerBattleUnit.pokemon.currentHitPoints;
        DamageDetails damageDetails = _playerBattleUnit.pokemon.TakeDamage(currentAttack.moveBase, _enemyBattleUnit.pokemon);

        yield return _playerBattleHud.UpdateHP(hpPriorToAttack);
        yield return ShowDamageDetails(damageDetails, _playerBattleUnit);

        if (damageDetails.hasFainted == true)
        {
            yield return _dialogBox.TypeDialog($"{_playerBattleUnit.pokemon.currentName} has fainted");
            _playerBattleUnit.PlayFaintAnimation();
            //apply experience here
        }
        else
        {
            //End turn
            PlayerActions();
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
}
