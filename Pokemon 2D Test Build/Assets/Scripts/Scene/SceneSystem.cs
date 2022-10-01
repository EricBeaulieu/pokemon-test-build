using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSystem
{
    public static LevelManager currentLevelManager { get; private set; }
    static List<GameSceneBaseSO> currentScenesLoaded;

    public static void Initialization()
    {
        currentScenesLoaded = GetAllOpenScenes();
    }

    public static IEnumerator NewAreaEntered(LevelManager newLevel)
    {
        if(currentLevelManager == newLevel)
        {
            yield break;
        }

        if (currentLevelManager == null)
        {
            newLevel.Initilization();
            AllActiveEntities.AddRange(newLevel.GetAllEntities());
        }
        currentLevelManager = newLevel;
        playerController.SetWildEncounter(newLevel.GetWildEncountersGrassSpecific);

        //Unload
        for (int i = currentScenesLoaded.Count - 1; i >= 0; i--)
        {
            if (currentScenesLoaded[i] == newLevel.GameSceneBase || newLevel.GameSceneBase.AdjacentGameScenes.Contains(currentScenesLoaded[i]) == true)
            {
                continue;
            }
            AllActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            yield return SceneManager.UnloadSceneAsync(currentScenesLoaded[i].name);
            currentScenesLoaded.RemoveAt(i);
        }

        //Load
        foreach (GameSceneBaseSO newScene in newLevel.GameSceneBase.AdjacentGameScenes)
        {
            if (currentScenesLoaded.Contains(newScene) == true)
            {
                continue;
            }
            AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(newScene.name, LoadSceneMode.Additive);
            currentScenesLoaded.Add(newScene);
            yield return OnLevelLoaded(sceneToLoad, newScene);
        }

        ArtificialGrid.SetupGrid();
    }

    static List<GameSceneBaseSO> GetAllOpenScenes()
    {
        List<GameSceneBaseSO> current = new List<GameSceneBaseSO>();
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
            GameSceneBaseSO matching = allGameSceneBaseSO.FirstOrDefault(x => x.name == loadedScenes[i].name);
            if (matching != null)
            {
                matching.GetLevelManager.Initilization();
                current.Add(matching);
            }
        }

        return current;
    }

    public static IEnumerator LoadScenethatPlayerSavedIn(GameSceneBaseSO scene)
    {

        for (int i = currentScenesLoaded.Count - 1; i >= 0; i--)
        {
            if (currentScenesLoaded.Contains(scene))
            {
                yield break;
            }

            if ((currentScenesLoaded.Any(x => scene.AdjacentGameScenes.Any(y => y == x))))
            {
                continue;
            }

            yield return AllActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            SceneManager.UnloadSceneAsync(currentScenesLoaded[i].GetScenePath);
            currentScenesLoaded.RemoveAt(i);
        }

        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
        currentScenesLoaded.Add(scene);
        yield return OnLevelLoaded(sceneToLoad, scene);
    }

    static IEnumerator OnLevelLoaded(AsyncOperation asyncScene, GameSceneBaseSO gameScene)
    {
        while (asyncScene.isDone == false)
        {
            yield return null;
        }
        gameScene.GetLevelManager.Initilization();
        AllActiveEntities.AddRange(gameScene.GetLevelManager.GetAllEntities());
    }

    public static BattleFieldLayoutBaseSO GetBattleEnvironmentArt
    {
        get { return currentLevelManager.GetBattleEnvironmentArt; }
    }

    static PlayerController playerController
    {
        get { return GameManager.instance.GetPlayerController; }
    }

    static List<Entity> AllActiveEntities
    {
        get { return GameManager.allActiveEntities; }
    }
}
