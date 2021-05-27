using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] BattleFieldLayoutBaseSO currentAreaDetails;
    [SerializeField] bool grassOnlyWildPokemon = true;
    [SerializeField] List<Pokemon> wildPokemon;
    [SerializeField] GameSceneBaseSO sceneReference;
    public List<Portal> allInLevelPortals = new List<Portal>();
    public List<Entity> allEntitiesInScene = new List<Entity>();
    bool loaded;

    void Awake()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;

        if(currentAreaDetails == null)
        {
            Debug.LogWarning("currentAreaDetails not set", gameObject);
        }

        if(wildPokemon.Count <= 0)
        {
            Debug.LogWarning("no wild pokemon have been set", gameObject);
        }
        else
        {
            for (int i = 0; i < wildPokemon.Count; i++)
            {
                if (wildPokemon[i].pokemonBase == null)
                {
                    Debug.LogWarning($"pokemon at position {i} has not been set", gameObject);
                }
                if(wildPokemon[i].currentLevel == 0)
                {
                    Debug.LogWarning($"pokemon at position {i} level has not been set", gameObject);
                }
            }
        }
        sceneReference.SetLevelManager(this);
    }

    public void Initilization()
    {
        if(loaded == true)
        {
            return;
        }

        GameObject[] temp = SceneManager.GetSceneByName(sceneReference.GetSceneName).GetRootGameObjects();

        allEntitiesInScene = ReturnAllEntities(temp);
        allInLevelPortals = ReturnAllPortals(temp);

        loaded = true;
    }

    public Pokemon WildPokemon()
    {
        Pokemon temp = wildPokemon[Random.Range(0, wildPokemon.Count)];
        return temp;
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
            GameManager.instance.NewAreaEntered(this);
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

    public bool GetWildEncountersGrassSpecific
    {
        get { return grassOnlyWildPokemon; }
    }
}
