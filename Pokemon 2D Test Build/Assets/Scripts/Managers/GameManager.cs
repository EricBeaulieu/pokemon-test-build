using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Overworld, Battle, Party,Inventory, Dialog, Fade}

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public bool startNewSaveEveryStart;

    [SerializeField] PlayerController playerPrefab;
    PlayerController playerController;
    [SerializeField] GameSceneBaseSO startingScene;
    [SerializeField] Transform defaultSpawnLocation;
    TrainerController trainerController = null;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera overWorldCamera;
    [SerializeField] PartySystem partySystem;
    [SerializeField] FadeSystem fadeSystem;
    [SerializeField] DialogManager dialogManager;
    LevelManager _levelManager;
    [SerializeField] StartMenu startMenu;
    [SerializeField] PokeballItem standardPokeball;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] EvolutionEggUI evolutionSystem;
    List<Pokemon> pokemonEvolving = new List<Pokemon>();

    static GameState state = GameState.Overworld;

    public List<Entity> allActiveEntities = new List<Entity>();
    List<GameSceneBaseSO> currentScenesLoaded = new List<GameSceneBaseSO>();

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
        battleSystem.Initialization();
        partySystem.Initialization();
        dialogManager.Initialization();
        startMenu.Initialization();
        inventorySystem.Initialization();
        evolutionSystem.Initialization();

        currentScenesLoaded = GetAllOpenScenes(currentScenesLoaded);

        if(startNewSaveEveryStart == false)
        {
            LoadGame();
        }
        else
        {
            StartCoroutine(LoadScenethatPlayerSavedIn(startingScene.GetSceneName));
            SpawnInPlayer(defaultSpawnLocation.position);
        }
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Overworld:
                RunAllEntities();
                break;
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
            case GameState.Party:
                break;
            case GameState.Inventory:
                inventorySystem.HandleUpdate();
                break;
            case GameState.Dialog:
                dialogManager.HandleUpdate();
                break;
            case GameState.Fade:
                break;
            default:
                Debug.LogError("Broken");
                break;
        }
    }

    public void StartWildPokemonBattle()
    {
        SetGameState(GameState.Battle);
        battleSystem.gameObject.SetActive(true);
        //overWorldCamera.gameObject.SetActive(false);

        Pokemon currentWildPokemon = _levelManager.WildPokemon();

        battleSystem.SetupBattleArt(_levelManager.GetBattleEnvironmentArt);
        battleSystem.StartBattle(playerController, currentWildPokemon);
    }

    public void StartTrainerBattle(TrainerController currentTrainer)
    {
        SetGameState(GameState.Battle);
        battleSystem.gameObject.SetActive(true);
        //overWorldCamera.gameObject.SetActive(false);
        trainerController = currentTrainer;

        trainerController.pokemonParty.HealAllPokemonInParty();

        battleSystem.SetupBattleArt(_levelManager.GetBattleEnvironmentArt);
        battleSystem.StartBattle(playerController, trainerController);
    }

    public void EndBattle(bool wasWon)
    {
        trainerController = null;

        if (wasWon == false)
        {
            playerController.PlayerHasLostBattle();
        }
        
        battleSystem.gameObject.SetActive(false);
        //overWorldCamera.gameObject.SetActive(true);
        playerController.pokemonParty.SetPositionstoBeforeBattle();

        if(wasWon == true)
        {
            for (int i = 0; i < playerController.pokemonParty.CurrentPokemonList().Count; i++)
            {
                Pokemon pokemon = playerController.pokemonParty.CurrentPokemonList()[i];
                if (pokemon.pokemonBase.EvolveLevelBased != null)
                {
                    foreach (EvolveLevelBased evolution in pokemon.pokemonBase.EvolveLevelBased)
                    {
                        if (evolution.CanEvolve(pokemon, pokemon.GetCurrentItem) == true && pokemon.currentHitPoints > 0)
                        {
                            pokemonEvolving.Add(pokemon);
                        }
                    }
                }
            }
            if(pokemonEvolving.Count > 0)
            {
                StartCoroutine(PokemonEvolvingAtEndOfBattle());
                return;
            }

        }
        SetGameState(GameState.Overworld);
    }

    void RunAllEntities()
    {
        foreach (Entity entity in allActiveEntities)
        {
            entity.HandleUpdate();
        }
    }

    public void CapturedNewPokemon(Pokemon capturedPokemon,PokeballItem pokeball)
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
        playerController.OpenStartMenu += () =>
        {
            SetGameState(GameState.Dialog);
            startMenu.EnableStartMenu(true);
        };
        playerController.PortalEntered += PlayerEnteredPortal;
        allActiveEntities.Add(playerController);
        overWorldCamera.transform.parent = playerController.transform;
        overWorldCamera.transform.localPosition = Vector3.zero;
        defaultSpawnLocation.gameObject.SetActive(false);
        partySystem.SetPlayersParty(playerController.pokemonParty);
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

    public void SaveGame()
    {
        SavingSystem.SavePlayer(playerController,_levelManager.GameSceneBase);
        SavingSystem.SavePlayerInventorySystem(inventorySystem.SaveInventory());
        dialogManager.ShowMessage("The Game Has Been Saved");
    }

    public void LoadGame()
    {
        if(SavingSystem.SaveFileAvailable() == true)
        {
            StartCoroutine(LoadGameIEnumerator());
        }
        else
        {
            dialogManager.ShowMessage("Their is no save to load");
        }
    }

    IEnumerator LoadGameIEnumerator()
    {
        yield return Fade(FadeStyle.FullFade,true);
        yield return SavingSystem.LoadPlayerScene();
        SpawnInPlayer(SavingSystem.LoadPlayerPosition());
        playerController.pokemonParty.LoadPlayerParty(SavingSystem.LoadPlayerParty());
        inventorySystem.LoadInventory(SavingSystem.LoadPlayerInventory());
        yield return Fade(FadeStyle.FullFade, false);
    }

    IEnumerator Fade(FadeStyle fadeStyle,bool isFadeIn)
    {
        if (isFadeIn)
        {
            SetGameState(GameState.Fade);
            yield return fadeSystem.FadeIn(fadeStyle);
        }
        else
        {
            yield return fadeSystem.FadeOut(fadeStyle);
            SetGameState(GameState.Overworld);
        }
    }

    public BattleSystem GetBattleSystem
    {
        get { return battleSystem; }
    }

    public PartySystem GetPartySystem
    {
        get { return partySystem; }
    }

    public InventorySystem GetInventorySystem
    {
        get { return inventorySystem; }
    }

    public DialogManager GetDialogSystem
    {
        get { return dialogManager; }
    }

    public PlayerController GetPlayerController
    {
        get { return playerController; }
    }

    public PokeballItem StandardPokeball
    {
        get { return standardPokeball; }
    }

    public EvolutionEggUI GetEvolutionSystem
    {
        get { return evolutionSystem; }
    }

    public static void SetGameState(GameState newState)
    {
        state = newState;
    }

    IEnumerator PokemonEvolvingAtEndOfBattle()
    {
        Pokemon pokemon;
        for (int i = 0; i < pokemonEvolving.Count(); i++)
        {
            pokemon = pokemonEvolving[i];
            PokemonBase newEvolution = pokemonEvolving[i].pokemonBase.EvolveLevelBased.Find(x => x.CanEvolve(pokemon, pokemon.GetCurrentItem) == true).NewPokemonEvolution();
            yield return evolutionSystem.PokemonEvolving(pokemon, newEvolution);
        }
        pokemonEvolving.Clear();
        SetGameState(GameState.Overworld);
    }
}
