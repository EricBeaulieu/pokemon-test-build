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
        _attackSelectionEventSelector.SetMovesList(_playerBattleUnit.pokemon.moves);

        yield return _dialogBox.TypeDialog($"A wild {_enemyBattleUnit.pokemon.currentName} has appeared!");
        yield return new WaitForSeconds(1f);

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

    void EnableActionSelector(bool enable)
    {
        _dialogBox.EnableActionSelector(true);
        if(enabled == true)
        {
            _actionSelectionEventSelector.SelectFirstBox();
        }
    }

    void EnableMoveSelector(bool enabled)
    {
        _dialogBox.EnableMoveSelector(true);
        if (enabled == true)
        {
            _attackSelectionEventSelector.SelectFirstBox();
        }
    }
}
