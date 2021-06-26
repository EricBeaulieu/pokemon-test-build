using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Color Scheme")]
public class ItemColorScheme : ScriptableObject
{
    [SerializeField] Color standard;
    [SerializeField] Color fadeA;
    [SerializeField] Color fadeB;
    [SerializeField] Color missingItemFade;

    public Color GetStandardColor
    {
        get { return standard; }
    }

    public Color GetFadeAColor
    {
        get { return fadeA; }
    }

    public Color GetFadeBColor
    {
        get { return fadeB; }
    }

    public Color GetMissingItemFadeColor
    {
        get { return missingItemFade; }
    }
}
