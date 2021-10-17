using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionEggUI : MonoBehaviour
{
    DialogManager dialogSystem;
    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite evolutionBackground;
    [SerializeField] Sprite eggBackground;

    [SerializeField] Image pokemonEvolving;
    [SerializeField] Image overlay;
    [SerializeField] DialogBox dialogBox;
    LearnNewMoveUI learnNewMoveUI;
    List<Sprite> pokemonSpriteAnimations;
    List<LearnableMove> newMovesLearned = new List<LearnableMove>();

    float tempAlpha;
    float animationTime = 1.5f;

    public void Initialization()
    {
        dialogSystem = GameManager.instance.GetDialogSystem;
        learnNewMoveUI = GameManager.instance.GetLearnNewMoveSystem;
        gameObject.SetActive(false);
    }


    public IEnumerator PokemonEvolving(Pokemon pokemon,PokemonBase newEvolution)
    {
        gameObject.SetActive(true);
        dialogSystem.SetCurrentDialogBox(dialogBox);
        backgroundImage.sprite = evolutionBackground;
        overlay.color = overlay.color.SetAlpha(0);
        pokemonSpriteAnimations = pokemon.pokemonBase.GetFrontSprite(pokemon.isShiny, pokemon.gender).ToList();
        pokemonSpriteAnimations.AddRange(newEvolution.GetFrontSprite(pokemon.isShiny, pokemon.gender));

        pokemonEvolving.sprite = pokemonSpriteAnimations[0];

        dialogSystem.SetDialogText($"What?! \n{pokemon.currentName} is evolving!");

        yield return new WaitForSeconds(BattleUnit.ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonEvolving.sprite = pokemonSpriteAnimations[1];
        yield return new WaitForSeconds(BattleUnit.ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonEvolving.sprite = pokemonSpriteAnimations[0];

        yield return dialogSystem.AfterDialogWait();

        yield return FadeOverlay(false);

        //Make the image big and small
        Debug.Log("Evolution happening");
        pokemonEvolving.sprite = pokemonSpriteAnimations[2];

        yield return FadeOverlay(true);
        
        yield return new WaitForSeconds(BattleUnit.ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonEvolving.sprite = pokemonSpriteAnimations[3];
        yield return new WaitForSeconds(BattleUnit.ENTRY_SPRITE_ANIMATION_SPEED);
        pokemonEvolving.sprite = pokemonSpriteAnimations[2];

        yield return dialogSystem.TypeDialog($"Congratulations! {pokemon.currentName} evolved into {newEvolution.GetPokedexName()}",true);

        pokemon.NewEvolution(newEvolution);

        newMovesLearned.Clear();
        newMovesLearned.AddRange(pokemon.GetLearnableMoveUponEvolution());
        newMovesLearned.AddRange(pokemon.GetLearnableMoveAtCurrentLevel());

        yield return learnNewMoveUI.PokemonWantsToLearnNewMoves(pokemon, newMovesLearned);

        gameObject.SetActive(false);
    }

    IEnumerator FadeOverlay(bool isOn)
    {
        if(isOn == true)
        {
            yield return FadeInOverlay();
        }
        else
        {
            yield return FadeOutOverlay();
        }
    }

    IEnumerator FadeInOverlay()
    {
        tempAlpha = 1;

        while (tempAlpha > 0)
        {
            tempAlpha -= (0.01f * animationTime);

            overlay.color = overlay.color.SetAlpha(tempAlpha);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeOutOverlay()
    {
        tempAlpha = 0;

        while (tempAlpha < 1)
        {
            tempAlpha += (0.01f * animationTime);

            overlay.color = overlay.color.SetAlpha(tempAlpha);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
