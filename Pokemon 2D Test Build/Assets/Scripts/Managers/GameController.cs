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

    GameState _state = GameState.Overworld;

    void Start()
    {
        playerController.OnEncounter += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        battleSystem.OpenPokemonParty += OpenParty;
    }

    void Update()
    {
        switch (_state)
        {
            case GameState.Overworld:
                playerController.HandleUpdate();
                break;
            case GameState.Battle:
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
        battleSystem.gameObject.SetActive(false);
        overWorldCamera.gameObject.SetActive(true);
    }

    void OpenParty(bool inBattle)
    {
        _state = GameState.Party;
        partySystem.gameObject.SetActive(true);
        partySystem.SetPartyData(playerController.GetComponent<PokemonParty>().CurrentPokemonList());
        partySystem.SelectFirstBox();
    }

    void CloseParty(bool inBattle)
    {
        if(inBattle == true)
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
