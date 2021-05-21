using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBaseSO : ScriptableObject
{
    [Header("EntityBase")]
    [SerializeField] CharacterArtSO characterArt;

    public virtual void Initialization()
    {
        if (characterArt == null)
        {
            Debug.LogError("This character SO is missing its character art", characterArt);
        }
    }

    public CharacterArtSO GetCharacterArt
    {
        get { return characterArt; }
    }
}
