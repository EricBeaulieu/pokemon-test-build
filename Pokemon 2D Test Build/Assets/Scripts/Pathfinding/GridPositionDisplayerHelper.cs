using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridPositionDisplayerHelper : MonoBehaviour
{
    [SerializeField] int xPos;
    [SerializeField] int yPos;
    [SerializeField] bool isWalkable;
    [SerializeField] SpriteRenderer spriteRenderer;

    public int XPos { get { return xPos; } set { xPos = value; } }
    public int YPos { get { return yPos; } set { yPos = value; } }
    public bool IsWalkable { get { return isWalkable; } 
        set {
            isWalkable = value; 
            if(value  == false)
            {
                spriteRenderer.color = Color.black;
            }
        } }


    //public GridPositionDisplayerHelper(int x,int y)
    //{
    //    gameObject.name += $"{x},{y}";
    //    Debug.Log($"{x},{y}", gameObject);
    //}

    //[SerializeField] Text positionDisplay;
}
