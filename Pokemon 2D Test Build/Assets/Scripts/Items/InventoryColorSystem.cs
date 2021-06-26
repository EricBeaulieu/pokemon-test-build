using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryColorSystem : MonoBehaviour
{
    [SerializeField] Image standard;
    [SerializeField] Image topInnerBorder;
    [SerializeField] Image topOuterBorder;
    [SerializeField] Image bottomInnerBorder;
    [SerializeField] Image bottomOuterBorder;

    public void SetNewScheme(ItemColorScheme colorScheme)
    {
        standard.color = colorScheme.GetStandardColor;
        topInnerBorder.color = colorScheme.GetFadeAColor;
        topOuterBorder.color = colorScheme.GetFadeBColor;
        bottomInnerBorder.color = colorScheme.GetFadeAColor;
        bottomOuterBorder.color = colorScheme.GetFadeBColor;
    }
}
