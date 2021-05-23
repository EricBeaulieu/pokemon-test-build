using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    LevelManager _levelManager;
    [SerializeField] StartMenu startMenu;
    //used as a null reference and to return from here
    [SerializeField] PokeballItem standardPokeball;
    [SerializeField] SpriteAtlas spriteAtlas;

    [Header("Specialized Battle Moves")]
    [SerializeField] List<MoveBase> movesThatLeavesTargetWithOneHP;

    bool _inBattle = false;

    GameState _state = GameState.Overworld;

    List<Entity> allActiveEntities;
    List<GameSceneBaseSO> currentScenesLoaded = new List<GameSceneBaseSO>();
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    [SerializeField] GameSceneBaseSO startingScene;


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

        InitializeAllDatabases();
    }

    void Start()
    {
        playerController.OnEncounter += StartWildPokemonBattle;
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
        allActiveEntities.Add(playerController);
        currentScenesLoaded.Add(startingScene);

        foreach (MoveBase move in movesThatLeavesTargetWithOneHP)
        {
            DamageModifiers.AddMovesThatLeavesTargetWithOneHP(move);
        }
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
        _inBattle = true;

        PokemonParty currentParty = playerController.GetComponent<PokemonParty>();
        Pokemon currentWildPokemon = _levelManager.WildPokemon();

        battleSystem.SetupBattleArt(_levelManager.GetBattleEnvironmentArt);
        battleSystem.StartBattle(currentParty,currentWildPokemon);
    }

    public void StartTrainerBattle(TrainerController currentTrainer)
    {
        _state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        overWorldCamera.gameObject.SetActive(false);
        _inBattle = true;
        trainerController = currentTrainer;

        PokemonParty currentParty = playerController.pokemonParty;
        PokemonParty trainerParty = currentTrainer.pokemonParty;
        trainerParty.HealAllPokemonInParty();

        battleSystem.SetupBattleArt(_levelManager.GetBattleEnvironmentArt);
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
        ConditionsDB.Initialization(GetAllConditions().ToList());
        EntryHazardsDB.Initialization(GetAllEntryHazards().ToList());
        WeatherEffectDB.Initialization(GetAllWeatherEffects().ToList());
        AbilityDB.Initialization(GetAllAbilities().ToList());
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

    IEnumerable<ConditionBase> GetAllConditions()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ConditionBase)))
            .Select(type => Activator.CreateInstance(type) as ConditionBase);
    }

    IEnumerable<EntryHazardBase> GetAllEntryHazards()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(EntryHazardBase)))
            .Select(type => Activator.CreateInstance(type) as EntryHazardBase);
    }

    IEnumerable<WeatherEffectBase> GetAllWeatherEffects()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(WeatherEffectBase)))
            .Select(type => Activator.CreateInstance(type) as WeatherEffectBase);
    }

    IEnumerable<AbilityBase> GetAllAbilities()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(AbilityBase)))
            .Select(type => Activator.CreateInstance(type) as AbilityBase);
    }

    public void NewAreaEntered(LevelManager newLevel)
    {
        _levelManager = newLevel;
        //allActiveEntities = newLevel.ReturnAllEntities();

        foreach (GameSceneBaseSO currentScenes in currentScenesLoaded)
        {
            if(currentScenes == newLevel.GameSceneBase || newLevel.GameSceneBase.AdjacentGameScenes.Contains(currentScenes) == true)
            {
                continue;
            }
            SceneManager.UnloadSceneAsync(currentScenes.GetScenePath);
            currentScenesLoaded.Remove(currentScenes);
        }

        foreach (GameSceneBaseSO newScene in newLevel.GameSceneBase.AdjacentGameScenes)
        {
            if (currentScenesLoaded.Contains(newScene) == true)
            {
                continue;
            }
            SceneManager.LoadSceneAsync(newScene.GetSceneName,LoadSceneMode.Additive);
            currentScenesLoaded.Add(newScene);
        }
    }
}
