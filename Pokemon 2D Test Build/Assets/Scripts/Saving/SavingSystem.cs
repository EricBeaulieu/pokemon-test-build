using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavingSystem
{
    static Stack<int> trainersToBeSaved = new Stack<int>();

    public static void SavePlayer(PlayerController player,GameSceneBaseSO currentScene)
    {
        int playerXPos = Mathf.FloorToInt(player.transform.position.x);
        int playerYPos = Mathf.FloorToInt(player.transform.position.y);
        PlayerPrefs.SetInt("PlayerXPos", playerXPos);
        PlayerPrefs.SetInt("PlayerYPos", playerYPos);

        string currentLevel = currentScene.GetSceneName;
        PlayerPrefs.SetString("CurrentLevel", currentLevel);

        List<Pokemon> currentParty = player.pokemonParty.CurrentPokemonList();

        for (int i = 0; i < currentParty.Count; i++)
        {
            
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

    public static bool GetTrainerSave(int trainerID)
    {
        return (PlayerPrefs.GetInt(trainerID.ToString()) == 1);
    }
}
