using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    BattleUnit _playerBattleUnit;
    [SerializeField]
    BattleHUD _playerBattleHud;
    [SerializeField]
    BattleUnit _enemyBattleUnit;
    [SerializeField]
    BattleHUD _enemyBattleHud;

    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()
    {
        _playerBattleUnit.Setup();
        _playerBattleHud.SetData(_playerBattleUnit.pokemon,false);
        _enemyBattleUnit.Setup();
        _enemyBattleHud.SetData(_enemyBattleUnit.pokemon, true);
    }
}
