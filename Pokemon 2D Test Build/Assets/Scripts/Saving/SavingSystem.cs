using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavingSystem
{
    static Stack<int> trainersToBeSaved = new Stack<int>();

    //keys
    const string PlayerName = "PlayerName";
    const string PlayerXPos = "PlayerXPos";
    const string PlayerYPos = "PlayerYPos";
    const string PlayerParty = "PlayerParty";

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

        string currentLevel = currentScene.GetSceneName;
        PlayerPrefs.SetString("CurrentLevel", currentLevel);

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
    }

    public static IEnumerator LoadPlayerScene()
    {
        trainersToBeSaved.Clear();

        string currentLevel = PlayerPrefs.GetString("CurrentLevel");
        yield return GameManager.instance.LoadScenethatPlayerSavedIn(currentLevel);
    }

    public static Vector2 LoadPlayerPosition()
    {
        return new Vector2(PlayerPrefs.GetInt("PlayerXPos"), PlayerPrefs.GetInt("PlayerYPos"));
    }

    public static void AddDefeatedTrainerToStack(int trainerID)
    {
        trainersToBeSaved.Push(trainerID);
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

    public static bool GetTrainerSave(int trainerID)
    {
        return (PlayerPrefs.GetInt(trainerID.ToString()) == 1);
    }
}
