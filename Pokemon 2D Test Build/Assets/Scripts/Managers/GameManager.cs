using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Overworld, Battle, Party, Dialog, Fade}

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public bool startNewSaveEveryStart;

    [SerializeField] PlayerController playerPrefab;
    PlayerController playerController;
    [SerializeField] Transform defaultSpawnLocation;
    TrainerController trainerController = null;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera overWorldCamera;
    [SerializeField] PartySystem partySystem;
    [SerializeField] FadeSystem fadeSystem;
    DialogManager _dialogManager;
    LevelManager _levelManager;
    [SerializeField] StartMenu startMenu;
    //used as a null reference and to return from here
    [SerializeField] PokeballItem standardPokeball;

    [Header("Specialized Battle Moves")]
    [SerializeField] List<MoveBase> movesThatLeavesTargetWithOneHP;

    bool _inBattle = false;

    GameState _state = GameState.Overworld;

    public List<Entity> allActiveEntities = new List<Entity>();
    List<GameSceneBaseSO> currentScenesLoaded = new List<GameSceneBaseSO>();
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

        GlobalDataBase.InitializeAllDatabases();
    }

    void Start()
    {
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

        startMenu.OpenPokemonParty += (() => OpenParty(false));
        startMenu.SaveGame += SaveGame;
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
        
        currentScenesLoaded = GetAllOpenScenes(currentScenesLoaded);

        foreach (MoveBase move in movesThatLeavesTargetWithOneHP)
        {
            DamageModifiers.AddMovesThatLeavesTargetWithOneHP(move);
        }

        if(startNewSaveEveryStart == false)
        {
            LoadGame();
        }
        else
        {
            SpawnInPlayer(defaultSpawnLocation.position);
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
            case GameState.Fade:
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

    void OpenParty(bool wasShiftSwap)
    {
        _inBattle = (_state == GameState.Battle);
        _state = GameState.Party;
        partySystem.gameObject.SetActive(true);
        partySystem.SetPartyData(playerController.pokemonParty.CurrentPokemonList(), _inBattle);
        partySystem.OpenPartySystem(_inBattle, wasShiftSwap);
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

    public void NewAreaEntered(LevelManager newLevel)
    {
        if(_levelManager == null)
        {
            newLevel.Initilization();
            allActiveEntities.AddRange(newLevel.GetAllEntities());
        }
        _levelManager = newLevel;
        playerController.SetWildEncounter(newLevel.GetWildEncountersGrassSpecific);
        

        for (int i = currentScenesLoaded.Count-1; i >= 0; i--)
        {
            if (currentScenesLoaded[i] == newLevel.GameSceneBase || newLevel.GameSceneBase.AdjacentGameScenes.Contains(currentScenesLoaded[i]) == true)
            {
                continue;
            }
            allActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            SceneManager.UnloadSceneAsync(currentScenesLoaded[i].GetScenePath);
            currentScenesLoaded.RemoveAt(i);
        }

        foreach (GameSceneBaseSO newScene in newLevel.GameSceneBase.AdjacentGameScenes)
        {
            if (currentScenesLoaded.Contains(newScene) == true)
            {
                continue;
            }
            AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(newScene.GetSceneName,LoadSceneMode.Additive);
            currentScenesLoaded.Add(newScene);
            StartCoroutine(OnLevelLoaded(sceneToLoad,newScene));
        }
    }

    List<GameSceneBaseSO> GetAllOpenScenes(List<GameSceneBaseSO> current)
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        GameSceneBaseSO[] allGameSceneBaseSO;
        allGameSceneBaseSO = Resources.LoadAll<GameSceneBaseSO>("SceneData");

        for (int i = 0; i < loadedScenes.Length; i++)
        {
            GameSceneBaseSO matching = allGameSceneBaseSO.FirstOrDefault(x => x.GetSceneName == loadedScenes[i].name);
            if(matching != null)
            {
                if(current.Contains(matching) == false)
                {
                    current.Add(matching);
                    matching.GetLevelManager.Initilization();
                }
            }
        }

        return current;
    }

    void SpawnInPlayer(Vector3 spawnLocation)
    {
        if(playerController != null)
        {
            allActiveEntities.Remove(playerController);
            overWorldCamera.transform.parent = null;
            Destroy(playerController.gameObject);
        }

        playerController = Instantiate(playerPrefab,spawnLocation,Quaternion.identity);
        playerController.OnEncounter += StartWildPokemonBattle;
        playerController.OpenStartMenu += () =>
        {
            _state = GameState.Dialog;
            startMenu.EnableStartMenu(true);
        };
        playerController.PortalEntered += PlayerEnteredPortal;
        allActiveEntities.Add(playerController);
        overWorldCamera.transform.parent = playerController.transform;
        overWorldCamera.transform.localPosition = Vector3.zero;
        defaultSpawnLocation.gameObject.SetActive(false);
    }

    public IEnumerator LoadScenethatPlayerSavedIn(string sceneName)
    {
        for (int i = currentScenesLoaded.Count - 1; i >= 0; i--)
        {
            allActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            SceneManager.UnloadSceneAsync(currentScenesLoaded[i].GetScenePath);
            currentScenesLoaded.RemoveAt(i);
        }

        GameSceneBaseSO gameSceneBase = Resources.FindObjectsOfTypeAll<GameSceneBaseSO>().FirstOrDefault(x => x.GetSceneName == sceneName);
        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(gameSceneBase.GetSceneName, LoadSceneMode.Additive);
        currentScenesLoaded.Add(gameSceneBase);
        yield return OnLevelLoaded(sceneToLoad, gameSceneBase);
    }

    void PlayerEnteredPortal(Portal portal)
    {
        if(portal.canPlayerPassThrough == true)
        {
            StartCoroutine(PortalAnimation(portal));
        }
    }

    IEnumerator PortalAnimation(Portal portal)
    {
        yield return Fade(portal.FadeStyle, true);
        Portal exit = portal.AlternativeScene.GetLevelManager.GetAllPortalsInLevel().FirstOrDefault(x => x.MatchingIdentifier == portal.MatchingIdentifier);
        exit.PlayerPassedThroughPortal();
        playerController.transform.root.position = exit.SpawnPoint;
        playerController.SnapToGrid();
        yield return Fade(exit.FadeStyle, false);
    }

    IEnumerator OnLevelLoaded(AsyncOperation asyncScene, GameSceneBaseSO gameScene)
    {
        while (asyncScene.isDone == false)
        {
            yield return null;
        }
        gameScene.GetLevelManager.Initilization();
        allActiveEntities.AddRange(gameScene.GetLevelManager.GetAllEntities());
    }

    void SaveGame()
    {
        SavingSystem.SavePlayer(playerController,_levelManager.GameSceneBase);
        _dialogManager.ShowMessage("The Game Has Been Saved");
    }

    public void LoadGame()
    {
        if(SavingSystem.SaveFileAvailable() == true)
        {
            StartCoroutine(LoadGameIEnumerator());
        }
        else
        {
            _dialogManager.ShowMessage("Their is no save to load");
        }
    }

    IEnumerator LoadGameIEnumerator()
    {
        yield return Fade(FadeStyle.FullFade,true);
        yield return SavingSystem.LoadPlayerScene();
        SpawnInPlayer(SavingSystem.LoadPlayerPosition());
        playerController.pokemonParty.LoadPlayerParty(SavingSystem.LoadPlayerParty());
        yield return Fade(FadeStyle.FullFade, false);
    }

    IEnumerator Fade(FadeStyle fadeStyle,bool isFadeIn)
    {
        if (isFadeIn)
        {
            _state = GameState.Fade;
            yield return fadeSystem.FadeIn(fadeStyle);
        }
        else
        {
            yield return fadeSystem.FadeOut(fadeStyle);
            _state = GameState.Overworld;
        }
    }
}
