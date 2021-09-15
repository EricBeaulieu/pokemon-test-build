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

    static Stack<int> trainersToBeSaved = new Stack<int>();

    //keys
    const string PlayerName = "PlayerName";
    const string PlayerXPos = "PlayerXPos";
    const string PlayerYPos = "PlayerYPos";
    const string PlayerParty = "PlayerParty";

    const string PlayerInventory = "PlayerInventory";
    const string PlayerLevelSaved = "CurrentLevel";

    public const string split = "*";

    public static bool SaveFileAvailable()
    {
        return PlayerPrefs.HasKey(PlayerName);
    }

    public static void SavePlayer(PlayerController player,GameSceneBaseSO currentScene)
    {
        int playerXPos = Mathf.FloorToInt(player.transform.position.x);
        int playerYPos = Mathf.FloorToInt(player.transform.position.y);
        PlayerPrefs.SetInt(PlayerXPos, playerXPos);
        PlayerPrefs.SetInt(PlayerYPos, playerYPos);
        PlayerPrefs.SetString(PlayerName, player.TrainerName);

        string currentLevel = GetAssetPath(currentScene);
        PlayerPrefs.SetString(PlayerLevelSaved, currentLevel);

        List<Pokemon> currentParty = player.pokemonParty.CurrentPokemonList();
        int partysize = currentParty.Count;

        for (int i = 0; i < PokemonParty.MAX_PARTY_POKEMON_SIZE; i++)
        {
            string location = $"{PlayerParty}{i}";
            if(i < partysize)
            {
                PokemonSavingSystem.SavePokemon(currentParty[i], location);
            }
            else
            {
                PokemonSavingSystem.SavePokemon(null, location);
            }
        }

        while(trainersToBeSaved.Count > 0)
        {
            PlayerPrefs.SetInt(trainersToBeSaved.Pop().ToString(), 1);
        }

        PlayerPrefs.Save();
        
        saveInfo = LoadFile();
        OverrideSave(saveInfo, currentScene);
        SaveFile(saveInfo);
    }

    public static IEnumerator LoadPlayerScene()
    {
        trainersToBeSaved.Clear();

        savedLevel = Resources.Load<GameSceneBaseSO>(PlayerPrefs.GetString(PlayerLevelSaved));
        yield return SceneSystem.LoadScenethatPlayerSavedIn(savedLevel);

        saveInfo = LoadFile();
    }

    public static Vector2 LoadPlayerPosition()
    {
        return new Vector2(PlayerPrefs.GetInt("PlayerXPos"), PlayerPrefs.GetInt("PlayerYPos"));
    }

    public static void AddDefeatedTrainerToStack(int trainerID)
    {
        trainersToBeSaved.Push(trainerID);
    }

    public static bool GetTrainerSave(int trainerID)
    {
        if (trainersToBeSaved.Contains(trainerID))
        {
            return true;
        }

        return (PlayerPrefs.GetInt(trainerID.ToString()) == 1);
    }

    public static List<Pokemon> LoadPlayerParty()
    {
        List<Pokemon> savedParty = new List<Pokemon>();

        for (int i = 0; i < PokemonParty.MAX_PARTY_POKEMON_SIZE; i++)
        {
            string location = $"{PlayerParty}{i}";
            Pokemon currentPokemon = PokemonSavingSystem.LoadPokemon(location);

            if(currentPokemon != null)
            {
                savedParty.Add(currentPokemon);
            }
        }
        return savedParty;
    }

    public static string GetAssetPath(Object obj)
    {
        string currentPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        currentPath = currentPath.Replace("Assets/Resources/", string.Empty);
        return currentPath.Remove(currentPath.Length - 6);
    }

    public static void SavePlayerInventorySystem(List<Item> currentInventory)
    {
        InventorySavingSystem.SaveInventory(currentInventory, PlayerInventory);
    }

    public static List<Item> LoadPlayerInventory()
    {
        return InventorySavingSystem.LoadInventorySystem(PlayerInventory);
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

    static void OverrideSave(Dictionary<string,object> state, GameSceneBaseSO playerSavedScene)
    {
        state = infoTobeSaved.Concat(state.Where(kvp => !infoTobeSaved.ContainsKey(kvp.Key))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (SaveableEntity saveable in playerSavedScene.GetLevelManager.SaveableEntities())
        {
            state[saveable.GetID] = saveable.CaptureState(true);
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
