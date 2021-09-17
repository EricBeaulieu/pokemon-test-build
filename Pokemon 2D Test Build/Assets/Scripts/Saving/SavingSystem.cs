using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
    public readonly static string savePath = $"{Application.persistentDataPath}/save.txt";
    static Dictionary<string, object> saveInfo = new Dictionary<string, object>();
    static Dictionary<string, object> infoTobeSaved = new Dictionary<string, object>();
    
    public static GameSceneBaseSO savedLevel { get; set; }

    //keys
    public const string PlayerID = "Player";
    const string PlayerInventory = "PlayerInventory";
    const string PlayerLevelSaved = "CurrentLevel";

    public const string split = "*";

    public static bool SaveFileAvailable()
    {
        return File.Exists(savePath);
    }

    public static void SavePlayer(PlayerController player,GameSceneBaseSO currentScene, List<Item> currentInventory)
    {
        saveInfo = LoadFile();
        OverrideSave(saveInfo, player, currentScene,currentInventory);
        SaveFile(saveInfo);
    }

    public static IEnumerator LoadPlayerScene()
    {
        saveInfo = LoadFile();

        object previousLevelSaved = SavingSystem.ReturnSpecificSave(PlayerLevelSaved);

        savedLevel = Resources.Load<GameSceneBaseSO>((string)previousLevelSaved);
        yield return SceneSystem.LoadScenethatPlayerSavedIn(savedLevel);
    }

    public static string GetAssetPath(Object obj)
    {
        string currentPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        currentPath = currentPath.Replace("Assets/Resources/", string.Empty);
        return currentPath.Remove(currentPath.Length - 6);
    }

    public static List<Item> LoadPlayerInventory()
    {
        object previousInventorySaved = SavingSystem.ReturnSpecificSave(PlayerInventory);

        return ((List<ItemSaveData>)previousInventorySaved).Select(x => new Item(x)).ToList();
    }

    public static void SaveFile(object state)
    {
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var formater = new BinaryFormatter();
            formater.Serialize(stream, state);
        }
    }

    public static Dictionary<string,object> LoadFile()
    {
        if(File.Exists(savePath) == false)
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            var formater = new BinaryFormatter();
            return (Dictionary<string, object>)formater.Deserialize(stream);
        }
    }

    static void OverrideSave(Dictionary<string,object> state,PlayerController player, GameSceneBaseSO playerSavedScene, List<Item> playerInventory)
    {
        state = infoTobeSaved.Concat(state.Where(kvp => !infoTobeSaved.ContainsKey(kvp.Key))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (SaveableEntity saveable in playerSavedScene.GetLevelManager.SaveableEntities())
        {
            state[saveable.GetID] = saveable.CaptureState(true);
        }

        //PlayerData
        if (state.ContainsKey(PlayerID))
        {
            state[PlayerID] = player.CaptureState();
        }
        else
        {
            state.Add(PlayerID, player.CaptureState());
        }

        //PlayerLevel
        if (state.ContainsKey(PlayerLevelSaved))
        {
            state[PlayerLevelSaved] = GetAssetPath(playerSavedScene);
        }
        else
        {
            state.Add(PlayerLevelSaved, GetAssetPath(playerSavedScene));
        }

        //PlayerInventory
        if (state.ContainsKey(PlayerInventory))
        {
            state[PlayerInventory] = playerInventory.Select(x => x.GetSaveData()).ToList();
        }
        else
        {
            state.Add(PlayerInventory, playerInventory.Select(x => x.GetSaveData()).ToList());
        }

        saveInfo = state;
    }

    public static object ReturnSpecificSave(string iD)
    {
        if (infoTobeSaved.TryGetValue(iD, out object infoTobeSavedValue))
        {
            return infoTobeSavedValue;
        }

        if (saveInfo.TryGetValue(iD, out object saveInfoValue))
        {
            return saveInfoValue;
        }

        return null;
    }

    public static void AddInfoTobeSaved(SaveableEntity saveable)
    {
        infoTobeSaved[saveable.GetID] = saveable.CaptureState();
    }
}
