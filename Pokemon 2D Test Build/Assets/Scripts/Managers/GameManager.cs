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
    [SerializeField] StartMenu startMenu;
    //used as a null reference and to return from here
    [SerializeField] PokeballItem standardPokeball;
    [SerializeField] SpriteAtlas spriteAtlas;

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
        playerController.OpenStartMenu += () => 
        {
            _state = GameState.Dialog;
            startMenu.EnableStartMenu(true);
        };

        battleSystem.OnBattleOver += EndBattle;
        battleSystem.OpenPokemonParty += OpenParty;
        battleSystem.OnPokemonCaptured += CapturedNewPokemon;

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

        startMenu.OpenPokemonParty += (() => OpenParty(false,false));
        startMenu.StartMenuClosed += () =>
        {
            _state = GameState.Overworld;
            playerController.ReturnFromStartMenu();
        };

        battleSystem.HandleAwake();
        startMenu.HandleAwake();

        if(partySystem.gameObject.activeInHierarchy == true)
        {
            partySystem.gameObject.SetActive(false);
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

        battleSystem.SetupBattleArt(levelManager.GetBattleEnvironmentArt);
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
        trainerParty.HealAllPokemonInParty();

        battleSystem.SetupBattleArt(levelManager.GetBattleEnvironmentArt);
        battleSystem.StartBattle(currentParty, trainerParty);
    }

    void EndBattle(bool wasWon)
    {
        trainerController = null;

        if (wasWon == false)
        {
            playerController.PlayerHasLostBattle();
        }

        _state = GameState.Overworld;
        _inBattle = false;
        battleSystem.gameObject.SetActive(false);
        overWorldCamera.gameObject.SetActive(true);
        playerController.GetComponent<PokemonParty>().SetPositionstoBeforeBattle();
    }

    void OpenParty(bool inBattle,bool wasShiftSwap)
    {
        _inBattle = inBattle;
        _state = GameState.Party;
        partySystem.gameObject.SetActive(true);
        partySystem.SetPartyData(playerController.GetComponent<PokemonParty>().CurrentPokemonList(),inBattle);
        partySystem.OpenPartySystem(inBattle,wasShiftSwap);
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
        AbilityDB.Initialization();
    }

    void CapturedNewPokemon(Pokemon capturedPokemon,PokeballItem pokeball)
    {
        //If pokemon was new then show it in the pokedex being added
        //show the pokemon info pop up and light up the sprite animating through the pokedex
        //animate the talking
        //Ask if the you would like to name the new pokemon

        //Add the new pokemon to either the party or PC
        bool addedToParty = playerController.GetComponent<PokemonParty>().AddCapturedPokemon(capturedPokemon,pokeball);
        if (addedToParty == true)
        {
            //return dialog stating that it was added to your party
        }
        else
        {
            // add to PC
        }
    }

    public PokeballItem StandardPokeball
    {
        get { return standardPokeball; }
    }

    public SpriteAtlas SpriteAtlas
    {
        get { return spriteAtlas; }
    }
}
