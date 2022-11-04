using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveableEntity))]
public class DestroyableObject : MonoBehaviour, IInteractable, ISaveable
{
    SpriteRenderer spriteRenderer;
    [SerializeField] string standardMessage;
    [SerializeField] string moveAvailable;
    const string pokemonAttackedMessage = "The Wild Pokemon Attacked!";
    const float animationSpeed = 0.125f;

    [Range(0, 100)]
    [Tooltip("Chances of finding a pokemon inside of the object")]
    [SerializeField] int wildEncounterChance = 15;
    [Tooltip("If this is null/empty, it will go to the level managers list and get a pokemon from there")]
    [SerializeField] WildPokemon specifiedPokemon;
    [SerializeField] WildPokemonEncounterTypes tableType;

    [SerializeField] Sprite standard;
    [SerializeField] MoveBase moveToDestroy;
    [SerializeField] List<Sprite> destroyedAnimationSprites;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.SnapToGrid();
    }

    public void RestoreBreakableObject()
    {
        if (gameObject.activeInHierarchy == true)
        {
            return;
        }

        Debug.Log("Breakable Object Restored", gameObject);

        spriteRenderer.sprite = standard;
        gameObject.SetActive(true);
    }

    public IEnumerator OnInteract(Vector2 vector2)
    {
        Pokemon pokemon = GameManager.instance.GetPlayerController.pokemonParty.ContainsMove(moveToDestroy.MoveName);
        DialogManager dialogManager = GameManager.instance.GetDialogSystem;

        if (pokemon == null)
        {
            yield return dialogManager.ShowDialogBox(new Dialog(standardMessage));
        }
        else
        {
            bool cutUsed = false;
            string pokemonUse = $"{pokemon.currentName} used {moveToDestroy.MoveName}!";
            dialogManager.ActivateDialog(true);
            yield return dialogManager.TypeDialog(standardMessage, true);
            yield return dialogManager.TypeDialog(moveAvailable);
            yield return dialogManager.SetChoiceBox(() =>
            {
                cutUsed = true;
            });

            if (cutUsed == true)
            {
                yield return dialogManager.TypeDialog(pokemonUse);
                dialogManager.ActivateDialog(false);
                GameManager.SetGameState(GameState.Dialog);
                yield return GameManager.instance.GetPlayerController.PlayHMAnimation(pokemon);
                yield return DestroyAnimation();
                
                if (Random.Range(0, 100) <= wildEncounterChance)
                {
                    yield return dialogManager.ShowDialogBox(new Dialog(pokemonAttackedMessage));
                    GameManager.SetGameState(GameState.Overworld);
                    if(specifiedPokemon.PokemonBase != null)
                    {
                        GameManager.instance.StartWildPokemonBattle(tableType, specifiedPokemon);
                    }
                    else
                    {
                        GameManager.instance.StartWildPokemonBattle(tableType);
                    }
                    yield break;
                }
                GameManager.SetGameState(GameState.Overworld);
            }
            else
            {
                dialogManager.ActivateDialog(false);
            }
            gameObject.SetActive(false);
        }
    }

    IEnumerator DestroyAnimation()
    {
        for (int i = 0; i < destroyedAnimationSprites.Count; i++)
        {
            yield return new WaitForSeconds(animationSpeed);
            spriteRenderer.sprite = destroyedAnimationSprites[i];
        }
    }

    public object CaptureState(bool playerSave = false)
    {
        return (gameObject.activeInHierarchy);
    }

    public void RestoreState(object state)
    {
        gameObject.SetActive((bool)state);
    }
}
