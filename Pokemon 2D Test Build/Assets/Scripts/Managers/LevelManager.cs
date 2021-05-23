using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] BattleFieldLayoutBaseSO currentAreaDetails;
    [SerializeField] List<Pokemon> wildPokemon;
    [SerializeField] GameSceneBaseSO sceneReference;

    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().path);
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

    }

    public Pokemon WildPokemon()
    {
        Pokemon temp = wildPokemon[Random.Range(0, wildPokemon.Count)];
        return temp;
    }

    public List<Entity> ReturnAllEntities()
    {
        List<Entity> allEntitiesInScene = new List<Entity>();

        //// get root objects in scene
        //List<GameObject> rootObjects = new List<GameObject>();
        //SceneManager.GetActiveScene().GetRootGameObjects(rootObjects);
        //Entity[] entity = GetComponentInChildren<Entity>();

        //// iterate root objects and do something
        //for (int i = 0; i < rootObjects.Count; ++i)
        //{
        //     = rootObjects[i].GetComponent<Entity>();
        //    if (entity != null)
        //    {
        //        allEntitiesInScene.Add(entity);
        //    }
        //}
        
        //Entity[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];

        //for (int i = 0; i < gameObjects.Length; i++)
        //{
        //    if(gameObjects[i].gameObject.activeInHierarchy == true)
        //    {
        //        allEntitiesInScene.Add(gameObjects[i]);
        //    }
        //}

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
}
