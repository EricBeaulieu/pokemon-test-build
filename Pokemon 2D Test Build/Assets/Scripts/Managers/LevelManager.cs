using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArtDetails
{
    public Sprite background { get; private set; }
    public Sprite playerPosition { get; private set; }
    public Sprite enemyPosition { get; private set; }

    public LevelArtDetails(Sprite background,Sprite playerPosition,Sprite enemyPosition)
    {
        this.background = background;
        this.playerPosition = playerPosition;
        this.enemyPosition = enemyPosition;
    }
}

public enum levelArea { GrassyField}

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemon;

    LevelArtDetails _currentAreaDetails;

    void Start()
    {
        _currentAreaDetails = LevelManagerArt.instance.GetArtDetails(levelArea.GrassyField);
    }

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

    public LevelArtDetails GetBattleEnvironmentArt
    {
        get { return _currentAreaDetails; }
    }
}
