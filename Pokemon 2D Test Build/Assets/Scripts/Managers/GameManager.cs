using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Overworld, Battle, Party, Dialog}

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    [SerializeField] PlayerController playerController;
    TrainerController trainerController = null;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera overWorldCamera;
    [SerializeField] PartySystem partySystem;
    DialogManager _dialogManager;
    [SerializeField] LevelManager levelManager;

    bool _inBattle = false;

    GameState _state = GameState.Overworld;

    List<Entity> allActiveEntities;

    public static GameManager instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeAllDatabases();

        playerController.OnEncounter += StartWildPokemonBattle;
        playerController.OnEncounter += (() => _inBattle = true);
        battleSystem.OnBattleOver += EndBattle;
        battleSystem.OpenPokemonParty += OpenParty;

        //Clean this up later, just get it working for now and then clean up the code later
        partySystem.battleSystemReference = battleSystem;
        partySystem.onCloseParty += CloseParty;

        _dialogManager = DialogManager.instance;
        _dialogManager.OnShowDialog += () => { _state = GameState.Dialog; };
        _dialogManager.OnCloseDialog += () => 
        {
            if(_state == GameState.Dialog)
            {
                if (_inBattle == true)
                {
                    _state = GameState.Battle;
                }
                else
                {
                    _state = GameState.Overworld;
                }
            }
        };

        if(battleSystem.gameObject.activeInHierarchy == true)
        {
            battleSystem.gameObject.SetActive(false);
        }
        allActiveEntities = new List<Entity>();
        allActiveEntities = levelManager.ReturnAllEntities();
    }

    void Update()
    {
        switch (_state)
        {
            case GameState.Overworld:
                RunAllEntities();
                break;
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
            case GameState.Party:
                break;
            case GameState.Dialog:
                _dialogManager.HandleUpdate();
                break;
            default:
                Debug.LogError("Broken");
                break;
        }
    }

    void StartWildPokemonBattle()
    {
        _state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        overWorldCamera.gameObject.SetActive(false);

        PokemonParty currentParty = playerController.GetComponent<PokemonParty>();
        Pokemon currentWildPokemon = FindObjectOfType<LevelManager>().GetComponent<LevelManager>().WildPokemon();

        battleSystem.StartBattle(currentParty,currentWildPokemon);
    }

    public void StartTrainerBattle(TrainerController currentTrainer)
    {
        _state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        overWorldCamera.gameObject.SetActive(false);
        _inBattle = true;
        trainerController = currentTrainer;

        PokemonParty currentParty = playerController.GetComponent<PokemonParty>();
        PokemonParty trainerParty = currentTrainer.GetComponent<PokemonParty>();

        battleSystem.StartBattle(currentParty, trainerParty);
    }

    void EndBattle(bool wasWon)
    {
        if(trainerController != null)
        {
            trainerController.HasLostBattleToPlayer();
            trainerController = null;
        }

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

    void RunAllEntities()
    {
        foreach (Entity entity in allActiveEntities)
        {
            entity.HandleUpdate();
        }
    }

    void InitializeAllDatabases()
    {
        ConditionsDB.Initialization();
        EntryHazardsDB.Initialization();
        WeatherEffectDB.Initialization();
    }
}
