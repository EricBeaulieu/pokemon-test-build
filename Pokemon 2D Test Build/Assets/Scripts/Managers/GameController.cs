using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Overworld, Battle, Party}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera overWorldCamera;
    [SerializeField] PartySystem partySystem;

    bool _inBattle = false;

    GameState _state = GameState.Overworld;

    void Start()
    {
        playerController.OnEncounter += StartBattle;
        playerController.OnEncounter += (() => _inBattle = true);
        battleSystem.OnBattleOver += EndBattle;
        battleSystem.OpenPokemonParty += OpenParty;

        //Clean this up later, just get it working for now and then clean up the code later
        partySystem.battleSystemReference = battleSystem;
        partySystem.onCloseParty += CloseParty;
        ConditionsDB.Initialization();
    }

    void Update()
    {
        switch (_state)
        {
            case GameState.Overworld:
                playerController.HandleUpdate();
                break;
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
            case GameState.Party:
                break;
            default:
                Debug.LogError("Broken");
                break;
        }
    }

    void StartBattle()
    {
        _state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        overWorldCamera.gameObject.SetActive(false);

        PokemonParty currentParty = playerController.GetComponent<PokemonParty>();
        Pokemon currentWildPokemon = FindObjectOfType<LevelManager>().GetComponent<LevelManager>().WildPokemon();

        battleSystem.StartBattle(currentParty,currentWildPokemon);
    }

    void EndBattle(bool wasWon)
    {
        _state = GameState.Overworld;
        _inBattle = false;
        battleSystem.gameObject.SetActive(false);
        overWorldCamera.gameObject.SetActive(true);
    }

    void OpenParty(bool inBattle)
    {
        _state = GameState.Party;
        partySystem.gameObject.SetActive(true);
        partySystem.SetPartyData(playerController.GetComponent<PokemonParty>().CurrentPokemonList(),inBattle);
        partySystem.OpenPartySystem();
    }

    void CloseParty()
    {
        if(_inBattle == true)
        {
            _state = GameState.Battle;
        }
        else
        {
            _state = GameState.Overworld;
        }
        partySystem.gameObject.SetActive(false);
    }
}
