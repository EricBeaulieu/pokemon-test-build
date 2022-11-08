using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { Overworld, Battle, Party,Inventory, Dialog, Fade,PC,Shop}

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public bool startNewSaveEveryStart;
    public bool startWithPresetBoxes;
    public bool turnOffWildPokemon;
    public bool turnOffTrainerBattles;

    public bool debugGrid = false;

    [SerializeField] PlayerController playerPrefab;
    static PlayerController playerController;
    [SerializeField] GridPositionDisplayerHelper gridPrefab;
    [SerializeField] GameSceneBaseSO startingScene;
    [SerializeField] Transform defaultSpawnLocation;
    TrainerController trainerController = null;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera overWorldCamera;
    [SerializeField] PartySystem partySystem;
    [SerializeField] FadeSystem fadeSystem;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] StartMenu startMenu;
    [SerializeField] PokeballItem standardPokeball;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] EvolutionEggUI evolutionSystem;
    [SerializeField] LearnNewMoveUI learnNewMoveUI;
    List<Pokemon> pokemonEvolving = new List<Pokemon>();
    [SerializeField] PCSystem pCSystem;
    [SerializeField] ShopSystem shopSystem;
    [SerializeField] AudioSource audioMusic;
    [SerializeField] AudioSource audioSFX;
    [SerializeField] List<AudioClip> trainerBattleMusic;
    [SerializeField] List<AudioClip> wildBattleMusic;
    [SerializeField] WildPokemonController wildPokemonPrefab;
    [SerializeField] SurfableEntity surfableEntityPrefab;
    static SurfableEntity playersurfableEntityController;

    public static LayerMask solidObjectLayermask { get; private set; }
    public static LayerMask interactableLayermask { get; private set; }
    public static LayerMask grassLayermask { get; private set; }
    public static LayerMask playerLayerMask { get; private set; }
    public static LayerMask portalLayerMask { get; private set; }
    public static LayerMask southLedgeLayerMask { get; private set; }
    public static LayerMask eastLedgeLayerMask { get; private set; }
    public static LayerMask westLedgeLayerMask { get; private set; }
    public static LayerMask waterLayerMask { get; private set; }
    public static LayerMask triggerLayerMask { get; private set; }

    [SerializeField] Test tester;
    bool testPokemonBeenSet = false;

    static GameState state = GameState.Overworld;

    public static List<Entity> allActiveEntities = new List<Entity>();
    List<GridPositionDisplayerHelper> aliveGrids = new List<GridPositionDisplayerHelper>();

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

        solidObjectLayermask = LayerMask.GetMask("SolidObjects");
        interactableLayermask = LayerMask.GetMask("Interactable");
        grassLayermask = LayerMask.GetMask("Grass");
        playerLayerMask = LayerMask.GetMask("Player");
        portalLayerMask = LayerMask.GetMask("Portal");
        southLedgeLayerMask = LayerMask.GetMask("SouthLedge");
        eastLedgeLayerMask = LayerMask.GetMask("EastLedge");
        westLedgeLayerMask = LayerMask.GetMask("WestLedge");
        waterLayerMask = LayerMask.GetMask("Water");
        triggerLayerMask = LayerMask.GetMask("Trigger");

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
        learnNewMoveUI.Initialization();
        SceneSystem.Initialization();
        pCSystem.Initialization();
        shopSystem.Initialization();
        AudioManager.Initialization(audioMusic,audioSFX,trainerBattleMusic,wildBattleMusic);
        SpecializedMoves.Initialization();

        if (startNewSaveEveryStart == false)
        {
            LoadGame();
        }
        else
        {
            StartCoroutine(SceneSystem.LoadScenethatPlayerSavedIn(startingScene));
            SpawnInPlayer();
        }

        if(startWithPresetBoxes == true)
        {
            pCSystem.PresetBoxesWithPresetPokemon();
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
            case GameState.PC:
                break;
            case GameState.Shop:
                shopSystem.HandleUpdate();
                break;
            default:
                Debug.LogError("Broken");
                break;
        }
    }

    public void StartWildPokemonBattle(WildPokemonEncounterTypes encounterType, WildPokemon specifiedPokemon = null)
    {
        Pokemon currentWildPokemon;
        if (specifiedPokemon != null)
        {
            currentWildPokemon = new Pokemon(specifiedPokemon);
        }
        else
        {
            currentWildPokemon = SceneSystem.currentLevelManager.WildPokemon(encounterType);
        }

        if(currentWildPokemon == null)
        {
            Debug.Log($"Current Level Manager does not have any pokemon set up to battle", SceneSystem.currentLevelManager.gameObject);
            return;
        }

        SetGameState(GameState.Battle);
        battleSystem.gameObject.SetActive(true);


        battleSystem.SetupBattleArt(SceneSystem.GetBattleEnvironmentArt);
        battleSystem.StartBattle(playerController, currentWildPokemon,encounterType);
    }

    public void StartWildPokemonBattle(WildPokemonController wildPokemon)
    {
        SetGameState(GameState.Battle);
        battleSystem.gameObject.SetActive(true);

        battleSystem.SetupBattleArt(SceneSystem.GetBattleEnvironmentArt);
        battleSystem.StartBattle(playerController, wildPokemon);
    }

    public void StartTrainerBattle(TrainerController currentTrainer)
    {
        SetGameState(GameState.Battle);
        battleSystem.gameObject.SetActive(true);
        trainerController = currentTrainer;

        trainerController.pokemonParty.HealAllPokemonInParty();

        battleSystem.SetupBattleArt(SceneSystem.GetBattleEnvironmentArt);
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
        playerController.pokemonParty.SetPositionstoBeforeBattle();

        if (wasWon == true)
        {
            for (int i = 0; i < playerController.pokemonParty.CurrentPokemonList().Count; i++)
            {
                Pokemon pokemon = playerController.pokemonParty.CurrentPokemonList()[i];
                if (pokemon.evolvePokemonAfterBattle == true)
                {
                    pokemonEvolving.Add(pokemon);
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

    public string CapturedNewPokemon(Pokemon capturedPokemon,PokeballItem pokeball)
    {
        string dialog;
        //If pokemon was new then show it in the pokedex being added
        //show the pokemon info pop up and light up the sprite animating through the pokedex
        //animate the talking
        //Ask if the you would like to name the new pokemon

        //Add the new pokemon to either the party or PC
        if (playerController.pokemonParty.PartyIsFull() == false)
        {
            capturedPokemon.Obtained(playerController, pokeball);
            playerController.pokemonParty.AddCapturedPokemon(capturedPokemon, pokeball);
            dialog = $"{capturedPokemon.currentName} has been added to your party";
        }
        else
        {
            if(pCSystem.IsPCFull() == false)
            {
                capturedPokemon.Obtained(playerController, pokeball);
                pCSystem.DepositPokemonInFirstAvailablePosition(capturedPokemon);
                dialog = $"{capturedPokemon.currentName} has been sent to a Box";
            }
            else
            {
                dialog = $"you caught the {capturedPokemon.currentName} but your pc box is full so you release it.";
            }
        }
        return dialog;
    }

    void SpawnInPlayer()
    {
        if(playerController != null)
        {
            allActiveEntities.Remove(playerController);
            overWorldCamera.transform.parent = null;
            Destroy(playerController.gameObject);
        }

        object previousSave = SavingSystem.ReturnSpecificSave(SavingSystem.PlayerID);
        if (previousSave != null)
        {
            Vector2 savedPos = new Vector2(((PlayerSaveData)previousSave).playerPosX, ((PlayerSaveData)previousSave).playerPosY);
            playerController = Instantiate(playerPrefab, savedPos, Quaternion.identity);
            playerController.RestoreState(previousSave);
        }
        else
        {
            playerController = Instantiate(playerPrefab, defaultSpawnLocation.position, Quaternion.identity);
        }
        
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
        if(exit == null)
        {
            Debug.Log($"Portal Exit not found {portal.MatchingIdentifier}");
        }
        exit.PlayerPassedThroughPortal();
        playerController.transform.root.position = exit.SpawnPoint;
        if(portal.FaceDirectionAfterMoving == true)
        {
            playerController.FaceTowardsDirection(portal.DirectionUponExiting);
        }
        playerController.SnapToGrid();
        yield return Fade(exit.FadeStyle, false);
    }

    public IEnumerator PlayerUsedHMAnimation(Pokemon pokemonUsingHM)
    {
        fadeSystem.SetPokemonUsingHm(pokemonUsingHM);
        yield return Fade(FadeStyle.PokemonHM,true);
        yield return fadeSystem.PokemonAnimationPassBy();
        yield return Fade(FadeStyle.PokemonHM, false);
        playerController.AnimationComplete();
    }

    public void SaveGame()
    {
        SavingSystem.SavePlayer(playerController, SceneSystem.currentLevelManager.GameSceneBase, inventorySystem.CurrentInventory(),pCSystem.SaveBoxData());
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

        SpawnInPlayer();
        inventorySystem.LoadInventory(SavingSystem.LoadPlayerInventory());
        pCSystem.LoadBoxData(SavingSystem.LoadPCInfo().ToArray());
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

    public LearnNewMoveUI GetLearnNewMoveSystem
    {
        get { return learnNewMoveUI; }
    }

    public PCSystem GetPCSystem
    {
        get { return pCSystem; }
    }

    public ShopSystem GetShopSystem
    {
        get { return shopSystem; }
    }

    public static void SetGameState(GameState newState)
    {
        Debug.Log(newState);
        state = newState;
    }

    IEnumerator PokemonEvolvingAtEndOfBattle()
    {
        Pokemon pokemon;
        for (int i = 0; i < pokemonEvolving.Count(); i++)
        {
            pokemon = pokemonEvolving[i];
            PokemonBase newEvolution = pokemonEvolving[i].pokemonBase.Evolutions.Find(x => x.CanEvolve(pokemon) == true).NewPokemonEvolution(pokemon);
            yield return evolutionSystem.PokemonEvolving(pokemon, newEvolution);
        }
        pokemonEvolving.Clear();
        SetGameState(GameState.Overworld);
    }

    public Pokemon[] GetTestPokemon()
    {
        if(testPokemonBeenSet == false)
        {
            tester.SetAllTestPokemon();
            testPokemonBeenSet = true;
        }
        return tester.pcPokemonTest;
    }

    public void CreateGridCell(int x, int y, Vector2 worldPos, bool walkable)
    {
        GridPositionDisplayerHelper gridPositionDisplayerHelper = ReturnFirstInactiveGameobject();
        if (gridPositionDisplayerHelper == null)
        {
            gridPositionDisplayerHelper = Instantiate(gridPrefab, new Vector3(worldPos.x + 0.5f, worldPos.y + 0.5f, 0), Quaternion.identity);
            aliveGrids.Add(gridPositionDisplayerHelper);
        }
        else
        {
            gridPositionDisplayerHelper.transform.position = new Vector3(worldPos.x + 0.5f, worldPos.y + 0.5f, 0);
            gridPositionDisplayerHelper.gameObject.SetActive(true);
        }
        gridPositionDisplayerHelper.gameObject.name = $"{x},{y}";
        Debug.Log($"{x},{y}", gridPositionDisplayerHelper.gameObject);
        gridPositionDisplayerHelper.XPos = x;
        gridPositionDisplayerHelper.YPos = y;
        gridPositionDisplayerHelper.IsWalkable = walkable;
    }

    public void ClearGrid()
    {
        for (int i = 0; i < aliveGrids.Count; i++)
        {
            aliveGrids[i].gameObject.SetActive(false);
        }
    }

    GridPositionDisplayerHelper ReturnFirstInactiveGameobject()
    {
        for (int i = 0; i < aliveGrids.Count; i++)
        {
            if (aliveGrids[i].gameObject.activeInHierarchy == false)
            {
                return aliveGrids[i];
            }
        }
        return null;
    }

    public WildPokemonController GetWildPokemonPrefab(WildPokemon wildPokemon)
    {
        wildPokemonPrefab.SetWildPokemon(wildPokemon);
        return wildPokemonPrefab;
    }

    public SurfableEntity GetPlayerSurfableEntityController
    {
        get 
        { 
            if(playersurfableEntityController == null)
            {
                playersurfableEntityController = Instantiate(surfableEntityPrefab, Vector3.zero, Quaternion.identity);
                playersurfableEntityController.gameObject.SetActive(false);
            }
            return playersurfableEntityController; 
        }
    }
}