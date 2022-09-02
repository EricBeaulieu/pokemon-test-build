using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] BattleFieldLayoutBaseSO currentAreaDetails;
    [SerializeField] bool showLabelUponEntry = true;
    [SerializeField] bool grassOnlyWildPokemon = true;
    [SerializeField] List<WildPokemon> standardWalking;
    [SerializeField] List<WildPokemon> standardSurfing;
    [SerializeField] GameSceneBaseSO sceneReference;
    [SerializeField] GridManager currentGrid;
    public GridManager GetGrid { get { return currentGrid; } }
    List<Portal> allInLevelPortals = new List<Portal>();
    List<Entity> allEntitiesInScene = new List<Entity>();
    List<CuttableTree> allTreesInScene = new List<CuttableTree>();
    bool loaded;
    [SerializeField] Transform wildPokemonSpawnParent;

    void Awake()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;

        if(currentAreaDetails == null)
        {
            Debug.LogWarning("currentAreaDetails not set", gameObject);
        }

        if(standardWalking.Count > 0)
        {
            for (int i = 0; i < standardWalking.Count; i++)
            {
                if (standardWalking[i].PokemonBase == null)
                {
                    Debug.LogWarning($"pokemon at position {i} has not been set", gameObject);
                }
                if(standardWalking[i].Level == 0)
                {
                    Debug.LogWarning($"pokemon at position {i} level has not been set", gameObject);
                }
            }
        }

        if(sceneReference == null)
        {
            Debug.LogError("Current Scene Reference is missing", gameObject);
        }

        if(currentGrid == null)
        {
            Debug.LogWarning("Current Grid Reference is missing", gameObject);
        }
        sceneReference.SetLevelManager(this);
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
        allTreesInScene = ReturnAllCuttableTrees(temp);

        loaded = true;
    }

    public Pokemon WildPokemon()
    {
        return new Pokemon(getWildPokemon());
    }

    WildPokemon getWildPokemon()
    {
        int olderValuesChecked = 0;
        int pokemonFound = Random.Range(0, 100);
        for (int i = 0; i < standardWalking.Count; i++)
        {
            if (pokemonFound <= (olderValuesChecked + standardWalking[i].WildEncounterChance))
            {
                return standardWalking[i];
            }
            olderValuesChecked += standardWalking[i].WildEncounterChance;
        }

        return standardWalking[standardWalking.Count];
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
            for (int i = 0; i < allTreesInScene.Count; i++)
            {
                allTreesInScene[i].RestoreTree();
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

    List<CuttableTree> ReturnAllCuttableTrees(GameObject[] gameObjects)
    {
        List<CuttableTree> trees = new List<CuttableTree>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            trees.AddRange(gameObjects[i].GetComponentsInChildren<CuttableTree>());
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
                if(saveable.GetComponent<CuttableTree>() == true)
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

    public int GetStandardWalkingCount()
    {
        int count = 0;
        for (int i = 0; i < standardWalking.Count; i++)
        {
            count += standardWalking[i].WildEncounterChance;
        }
        return count;
    }

    public int GetStandardSurfingCount()
    {
        int count = 0;
        for (int i = 0; i < standardSurfing.Count; i++)
        {
            count += standardSurfing[i].WildEncounterChance;
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

        WildPokemonController temp = GameManager.instance.GetWildPokemonPrefab(getWildPokemon());
        Instantiate(temp, spawnLocation, Quaternion.identity,wildPokemonSpawnParent);
    }
}
