using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSystem
{
    public static LevelManager currentLevelManager { get; private set; }
    static List<GameSceneBaseSO> currentScenesLoaded = new List<GameSceneBaseSO>();

    public static void Initialization()
    {
        currentScenesLoaded = GetAllOpenScenes(currentScenesLoaded);
    }

    public static IEnumerator NewAreaEntered(LevelManager newLevel)
    {
        if (currentLevelManager == null)
        {
            newLevel.Initilization();
            allActiveEntities.AddRange(newLevel.GetAllEntities());
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
            allActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            SceneManager.UnloadSceneAsync(currentScenesLoaded[i].GetScenePath);
            currentScenesLoaded.RemoveAt(i);
        }

        //Load
        foreach (GameSceneBaseSO newScene in newLevel.GameSceneBase.AdjacentGameScenes)
        {
            if (currentScenesLoaded.Contains(newScene) == true)
            {
                continue;
            }
            AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(newScene.GetSceneName, LoadSceneMode.Additive);
            currentScenesLoaded.Add(newScene);
            yield return OnLevelLoaded(sceneToLoad, newScene);
        }
    }

    static List<GameSceneBaseSO> GetAllOpenScenes(List<GameSceneBaseSO> current)
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
            if (matching != null)
            {
                if (current.Contains(matching) == false)
                {
                    SceneManager.UnloadSceneAsync(matching.GetScenePath);
                }
            }
        }

        return current;
    }

    public static IEnumerator LoadScenethatPlayerSavedIn(GameSceneBaseSO scene)
    {
        for (int i = currentScenesLoaded.Count - 1; i >= 0; i--)
        {
            allActiveEntities.RemoveAll(x => currentScenesLoaded[i].GetLevelManager.GetAllEntities().Contains(x) == true);
            SceneManager.UnloadSceneAsync(currentScenesLoaded[i].GetScenePath);
            currentScenesLoaded.RemoveAt(i);
        }

        GameSceneBaseSO gameSceneBase = Resources.FindObjectsOfTypeAll<GameSceneBaseSO>().FirstOrDefault(x => x == scene);
        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(gameSceneBase.GetSceneName, LoadSceneMode.Additive);
        currentScenesLoaded.Add(gameSceneBase);
        yield return OnLevelLoaded(sceneToLoad, gameSceneBase);
    }

    static IEnumerator OnLevelLoaded(AsyncOperation asyncScene, GameSceneBaseSO gameScene)
    {
        while (asyncScene.isDone == false)
        {
            yield return null;
        }
        gameScene.GetLevelManager.Initilization();
        allActiveEntities.AddRange(gameScene.GetLevelManager.GetAllEntities());
    }

    public static BattleFieldLayoutBaseSO GetBattleEnvironmentArt
    {
        get { return currentLevelManager.GetBattleEnvironmentArt; }
    }

    static PlayerController playerController
    {
        get { return GameManager.instance.GetPlayerController; }
    }

    static List<Entity> allActiveEntities
    {
        get { return GameManager.allActiveEntities; }
    }
}