using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldBed : MonoBehaviour
{
    [SerializeField] Color matress;
    [SerializeField] Color sheets;
    [SerializeField] Color sheetsOverlay;

    [SerializeField] List<SpriteRenderer> matressList;
    [SerializeField] List<SpriteRenderer> sheetsList;
    [SerializeField] List<SpriteRenderer> sheetsOverlayList;

    public void UpdateColor()
    {
        foreach (SpriteRenderer spriteRenderer in matressList)
        {
            spriteRenderer.color = matress;
        }

        foreach (SpriteRenderer spriteRenderer in sheetsList)
        {
            spriteRenderer.color = sheets;
        }

        foreach (SpriteRenderer spriteRenderer in sheetsOverlayList)
        {
            spriteRenderer.color = sheetsOverlay;
        }
    }
}
