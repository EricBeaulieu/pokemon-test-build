using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WildPokemonEncounterTypes { Walking,Surfing,OldRod,GoodRod,SuperRod,RockSmash,CuttableTree }//headbutting trees, honey trees
public class LevelManager : MonoBehaviour
{
    [SerializeField] BattleFieldLayoutBaseSO currentAreaDetails;
    [SerializeField] bool showLabelUponEntry = true;
    [SerializeField] bool grassOnlyWildPokemon = true;
    [SerializeField] List<WildPokemon> walking;
    [SerializeField] List<WildPokemon> surfing;
    [SerializeField] List<WildPokemon> oldRod;
    [SerializeField] List<WildPokemon> goodRod;
    [SerializeField] List<WildPokemon> superRod;
    [SerializeField] List<WildPokemon> cuttableTree;
    [SerializeField] List<WildPokemon> rockSmash;
    [SerializeField] GameSceneBaseSO sceneReference;
    [SerializeField] GridManager currentGrid;
    public GridManager GetGrid { get { return currentGrid; } }
    List<Portal> allInLevelPortals = new List<Portal>();
    List<Entity> allEntitiesInScene = new List<Entity>();
    List<DestroyableObject> allDestroyableObjectsInScene = new List<DestroyableObject>();
    bool loaded;
    [SerializeField] Transform wildPokemonSpawnParent;

    void Awake()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;

        if(currentAreaDetails == null)
        {
            Debug.LogWarning("currentAreaDetails not set", gameObject);
        }

        StartingCheckerEncounterList(walking);
        StartingCheckerEncounterList(surfing);
        StartingCheckerEncounterList(oldRod);
        StartingCheckerEncounterList(goodRod);
        StartingCheckerEncounterList(superRod);
        StartingCheckerEncounterList(cuttableTree);
        StartingCheckerEncounterList(rockSmash);

        if (sceneReference == null)
        {
            Debug.LogError("Current Scene Reference is missing", gameObject);
        }

        if(currentGrid == null)
        {
            Debug.LogWarning("Current Grid Reference is missing", gameObject);
        }
        sceneReference.SetLevelManager(this);
    }

    void StartingCheckerEncounterList(List<WildPokemon> currentList)
    {
        if (currentList.Count > 0)
        {
            for (int i = 0; i < currentList.Count; i++)
            {
                if (currentList[i].PokemonBase == null)
                {
                    Debug.LogWarning($"pokemon at position {i} has not been set", gameObject);
                }
                if (currentList[i].Level == 0)
                {
                    Debug.LogWarning($"pokemon at position {i} level has not been set", gameObject);
                }
                if (currentList[i].WildEncounterChance == 0)
                {
                    Debug.LogWarning($"pokemon at position {i} encounter chance has not been set", gameObject);
                }
            }
        }
    }

    public void Initilization()
    {
        if(loaded == true)
        {
            return;
        }

        GameObject[] temp = SceneManager.GetSceneByName(sceneReference.name).GetRootGameObjects();

        ReloadSavedSettings();
        allEntitiesInScene = ReturnAllEntities(temp);
        allInLevelPortals = ReturnAllPortals(temp);
        allDestroyableObjectsInScene = ReturnAllCuttableTrees(temp);

        loaded = true;
    }

    public Pokemon WildPokemon(WildPokemonEncounterTypes encounterType)
    {
        switch (encounterType)
        {
            case WildPokemonEncounterTypes.Walking:
                if (walking.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(walking));
            case WildPokemonEncounterTypes.Surfing:
                if (surfing.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(surfing));
            case WildPokemonEncounterTypes.OldRod:
                if (oldRod.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(oldRod));
            case WildPokemonEncounterTypes.GoodRod:
                if (goodRod.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(goodRod));
            case WildPokemonEncounterTypes.SuperRod:
                if (superRod.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(superRod));
            case WildPokemonEncounterTypes.RockSmash:
                if (rockSmash.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(rockSmash));
            case WildPokemonEncounterTypes.CuttableTree:
                if (cuttableTree.Count <= 0)
                    return null;
                return new Pokemon(getWildPokemon(cuttableTree));
            default:
                return null;
        }
    }

    WildPokemon getWildPokemon(List<WildPokemon> wildPokemon)
    {
        int olderValuesChecked = 0;
        int pokemonFound = Random.Range(0, 100);
        for (int i = 0; i < wildPokemon.Count; i++)
        {
            if (pokemonFound <= (olderValuesChecked + wildPokemon[i].WildEncounterChance))
            {
                return wildPokemon[i];
            }
            olderValuesChecked += wildPokemon[i].WildEncounterChance;
        }

        return wildPokemon[wildPokemon.Count];
    }

    public List<Entity> GetAllEntities()
    {
        return allEntitiesInScene;
    }

    public BattleFieldLayoutBaseSO GetBattleEnvironmentArt
    {
        get { return currentAreaDetails; }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log($"entered", gameObject);
            StartCoroutine(SceneSystem.NewAreaEntered(this));
            if(sceneReference.GetLevelMusic != null)
            {
                Debug.Log("Playing music");
                AudioManager.PlayMusic(sceneReference.GetLevelMusic,sceneReference.GetLevelMusicVolume);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < allDestroyableObjectsInScene.Count; i++)
            {
                allDestroyableObjectsInScene[i].RestoreBreakableObject();
            }
        }
    }

    public GameSceneBaseSO GameSceneBase
    {
        get { return sceneReference; }
    }

    public List<Portal> GetAllPortalsInLevel()
    {
        return allInLevelPortals;
    }

    List<Entity> ReturnAllEntities(GameObject[] gameObjects)
    {
        List<Entity> entity = new List<Entity>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if(gameObjects[i].activeInHierarchy == false)
            {
                continue;
            }
            entity.AddRange(gameObjects[i].GetComponentsInChildren<NpcController>());
            entity.AddRange(gameObjects[i].GetComponentsInChildren<TrainerController>());
        }

        return entity;
    }

    List<Portal> ReturnAllPortals(GameObject[] gameObjects)
    {
        List<Portal> portals = new List<Portal>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            portals.AddRange(gameObjects[i].GetComponentsInChildren<Portal>());
        }

        return portals;
    }

    List<DestroyableObject> ReturnAllCuttableTrees(GameObject[] gameObjects)
    {
        List<DestroyableObject> trees = new List<DestroyableObject>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            trees.AddRange(gameObjects[i].GetComponentsInChildren<DestroyableObject>());
        }

        return trees;
    }

    public bool GetWildEncountersGrassSpecific
    {
        get { return grassOnlyWildPokemon; }
    }

    public void ReloadSavedSettings()
    {
        if(SavingSystem.savedLevel == sceneReference)
        {
            SavingSystem.savedLevel = null;

            foreach (SaveableEntity saveable in SaveableEntities())
            {
                object previousSave = SavingSystem.ReturnSpecificSave(saveable.GetID);
                if (previousSave != null)
                {
                    saveable.RestoreState(previousSave);
                }
            }
        }
        else
        {
            foreach (SaveableEntity saveable in SaveableEntities())
            {
                if(saveable.GetComponent<DestroyableObject>() == true)
                {
                    continue;
                }
                object previousSave = SavingSystem.ReturnSpecificSave(saveable.GetID);
                if (previousSave != null)
                {
                    saveable.RestoreState(previousSave);
                }
            }
        }
    }

    public List<SaveableEntity> SaveableEntities()
    {
        List<SaveableEntity> saveableEntities = new List<SaveableEntity>();
        GameObject[] temp = SceneManager.GetSceneByName(sceneReference.name).GetRootGameObjects();

        for (int i = 0; i < temp.Length; i++)
        {
            if(temp[i].activeInHierarchy == true)
            {
                saveableEntities.AddRange(temp[i].GetComponentsInChildren<SaveableEntity>());
            }
        }

        return saveableEntities;
    }

    public string GetCurrentListCount()
    {
        string s;

        s = $"Current Level Manager Walking Encounter Total: {GetWildPokemonListCount(walking)}";
        s += $"\nCurrent Level Manager Surfing Encounter Total: {GetWildPokemonListCount(surfing)}";
        s += $"\nCurrent Level Manager Old Rod Encounter Total: {GetWildPokemonListCount(oldRod)}";
        s += $"\nCurrent Level Manager Good Rod Encounter Total: {GetWildPokemonListCount(goodRod)}";
        s += $"\nCurrent Level Manager Super Rod Encounter Total: {GetWildPokemonListCount(superRod)}";
        s += $"\nCurrent Level Manager Cuttable Tree Encounter Total: {GetWildPokemonListCount(cuttableTree)}";
        s += $"\nCurrent Level Manager Rock Smash Encounter Total: {GetWildPokemonListCount(rockSmash)}";
        return s;
    }

    public int GetWildPokemonListCount(List<WildPokemon> currentList)
    {
        int count = 0;
        for (int i = 0; i < currentList.Count; i++)
        {
            count += currentList[i].WildEncounterChance;
        }
        return count;
    }

    public void SpawnInPokemon()
    {
        if(wildPokemonSpawnParent == null)
        {
            return;
        }

        Vector2 spawnLocation = currentGrid.SpawnLocation();
        Collider2D collider = Physics2D.OverlapCircle(spawnLocation, 0.25f, GameManager.solidObjectLayermask | 
            GameManager.interactableLayermask | GameManager.playerLayerMask | GameManager.southLedgeLayerMask | 
            GameManager.eastLedgeLayerMask | GameManager.westLedgeLayerMask | GameManager.waterLayerMask);

        if(collider != null)
        {
            Debug.Log("Cannot go into location " + collider.gameObject.transform.position);
            return;
        }

        WildPokemonController temp = GameManager.instance.GetWildPokemonPrefab(getWildPokemon(walking));
        Instantiate(temp, spawnLocation, Quaternion.identity,wildPokemonSpawnParent);
    }
}
