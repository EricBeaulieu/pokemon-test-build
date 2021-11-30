using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class OverworldPokeballContainingPokemon : MonoBehaviour,IInteractable, ISaveable
{
    [SerializeField] SaveableEntity saveableEntity;
    [SerializeField] Pokemon giftPokemon;
    [SerializeField] PokeballItem specifiedPokeball;

    [Tooltip("<player> converts to the players name and <pokemon> converts to the pokemons original name")]
    [SerializeField] string specializedMessage;

    bool giftReceived;

    void Start()
    {
        transform.position = GlobalTools.SnapToGrid(transform.position);
    }

    public IEnumerator OnInteract(Vector2 vector2)
    {
        if(giftPokemon.pokemonBase == null)
        {
            Debug.LogError($"Pokemon set to null on current Overworld Pokeball", gameObject);
            yield break;
        }

        DialogManager dialogManager = GameManager.instance.GetDialogSystem;
        PlayerController player = GameManager.instance.GetPlayerController;

        string message;
        dialogManager.ActivateDialog(true);
        if (player.pokemonParty.PartyIsFull() == true)
        {
            message = $"Your party is full. Please make room in your party to receive your gift.";
            yield return dialogManager.TypeDialog(message,true);
            dialogManager.ActivateDialog(false);
            yield break;
        }

        giftPokemon = new Pokemon(giftPokemon.pokemonBase, giftPokemon.currentLevel, giftPokemon.individualValues, giftPokemon.effortValues,
                    giftPokemon.gender, giftPokemon.isShiny, giftPokemon.Nature, giftPokemon.currentName, giftPokemon.presetMoves,
                    giftPokemon.startingAbilityID, giftPokemon.GetCurrentItem);

        if (string.IsNullOrEmpty(specializedMessage) == true)
        {
            message = $"{player.TrainerName} received {giftPokemon.pokemonBase.GetPokedexName()}!";
        }
        else
        {
            message = GlobalTools.ReplacePokemonWithPokemonName(specializedMessage, giftPokemon.pokemonBase);
            message = GlobalTools.ReplacePlayerWithTrainerName(message, player);
        }

        player.pokemonParty.AddGiftPokemon(giftPokemon,specifiedPokeball);

        giftReceived = true;
        gameObject.SetActive(false);
        yield return dialogManager.TypeDialog($"{message}", true);
        dialogManager.ActivateDialog(false);
        SavingSystem.AddInfoTobeSaved(saveableEntity);
    }

    public object CaptureState(bool playerSave = false)
    {
        return giftReceived;
    }

    public void RestoreState(object state)
    {
        giftReceived = (bool)state;
        gameObject.SetActive(!giftReceived);
    }
}
