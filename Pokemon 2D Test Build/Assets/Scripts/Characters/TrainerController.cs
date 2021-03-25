using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PokemonParty))]
public class TrainerController : Entity,IInteractable
{
    PokemonParty pokemonParty;

    public override void HandleUpdate()
    {
        
    }

    void IInteractable.OnInteract(Vector2 vector2)
    {
        Debug.Log("Test");
    }
}
