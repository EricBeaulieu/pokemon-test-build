using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCParty : MonoBehaviour
{
    [SerializeField] PCPokemon[] partyPokemon;
    public static bool isOn { get; private set; } = false;

    const int Y_ON_SCREEN = 5;
    const int Y_OFF_SCREEN = 455;
    const float BOX_SPEED_ANIMATION = 0.25f;

    public IEnumerator EnableParty()
    {
        Vector3 endPosition = transform.localPosition;
        if (isOn == true)
        {
            endPosition.y = Y_OFF_SCREEN;
        }
        else
        {
            endPosition.y = Y_ON_SCREEN;
        }

        isOn = !isOn;
        yield return GlobalTools.SmoothTransitionToPositionUsingLocalPosition(transform, endPosition, BOX_SPEED_ANIMATION);
    }

    public GameObject ReturnFirstSelection()
    {
        return partyPokemon[0].gameObject;
    }

    public void SetupData()
    {
        List<Pokemon> playerParty = GameManager.instance.GetPlayerController.pokemonParty.CurrentPokemonList();
        for (int i = 0; i < partyPokemon.Length; i++)
        {
            if(i < playerParty.Count)
            {
                partyPokemon[i].DepositPokemon(playerParty[i]);
                continue;
            }
            partyPokemon[i].DepositPokemon(null);
        }
    }
}
