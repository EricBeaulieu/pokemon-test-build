using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemon;

    public Pokemon WildPokemon()
    {
        Pokemon temp = wildPokemon[Random.Range(0, wildPokemon.Count)];
        return temp;
    }

    public List<Entity> ReturnAllEntities()
    {
        //var gameObjects = (Entity)FindSceneObjectsOfType(typeof(Entity)) as Entity[]();
        Entity[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(Entity)) as Entity[];

        List<Entity> allEntitiesInScene = new List<Entity>();

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if(gameObjects[i].gameObject.activeInHierarchy == true)
            {
                allEntitiesInScene.Add(gameObjects[i]);
            }
        }

        return allEntitiesInScene;
    }
}
