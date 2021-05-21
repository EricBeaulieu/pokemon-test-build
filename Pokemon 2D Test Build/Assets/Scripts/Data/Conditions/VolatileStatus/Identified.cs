using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Identified : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Identified; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Identified(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} was Identified";
    }
    public override ElementType IdentifiedAndRemovesImmunityFromType(Pokemon pokemon)
    {
        pokemon.CureVolatileStatus(Id);
        return typeIdentified;
    }
    ElementType typeIdentified;
    public void SetIdentifiedImmunityRemoval(ElementType removedImmunity)
    {
        typeIdentified = removedImmunity;
    }
}