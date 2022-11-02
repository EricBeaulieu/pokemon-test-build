using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Game Scene")]
public class GameSceneBaseSO : ScriptableObject
{
    [Header("Information")]
    [SerializeField] string sceneName;
    [SerializeField] string shortDescription;
    [SerializeField] List<GameSceneBaseSO> adjacentGameScenes;
    static char[] delimiterChars = { '.' };

    [Header("Sounds")]
    [SerializeField] AudioClip music;
    [Range(0.0f, 1.0f)]
    [SerializeField] float musicVolume = 1.0f;
    
    bool isLoaded;
    LevelManager attachedLevelManager;

    public string GetSceneName
    {
        get { return sceneName; }
    }

    public string GetScenePath
    {
        get { return GetAssetPath(this); }
    }

    static string GetAssetPath(GameSceneBaseSO gSBSO)
    {
        string currentPath = UnityEditor.AssetDatabase.GetAssetPath(gSBSO);
        currentPath = currentPath.Replace("Assets/Resources/SceneData/", "Assets/Scenes/");
        currentPath = currentPath.Split(delimiterChars)[0];
        return currentPath;
    }

    public AudioClip GetLevelMusic
    {
        get { return music; }
    }

    public float GetLevelMusicVolume
    {
        get { return musicVolume; }
    }

    public bool SceneLoaded
    {
        get { return isLoaded; }
        set { isLoaded = value; }
    }

    public List<GameSceneBaseSO> AdjacentGameScenes
    {
        get { return adjacentGameScenes; }
    }

    public void SetLevelManager(LevelManager levelManager)
    {
        attachedLevelManager = levelManager;
    }

    public LevelManager GetLevelManager
    {
        get { return attachedLevelManager; }
    }
}
