using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AIDecision
{
    public Vector2 movement;
    public Vector2 directionToFace;
    public float specificTimeUniltNextExecution;

    public AIDecision(Vector2 path)
    {
        movement = path;
        directionToFace = Vector2.zero;
        specificTimeUniltNextExecution = 0;
    }

    public AIDecision(FacingDirections direction)
    {
        movement = Vector2.zero;
        directionToFace = new Vector2().GetDirection(direction);
        specificTimeUniltNextExecution = 0;
    }

    public AIDecision(float TimeUniltNextExecution)
    {
        movement = Vector2.zero;
        directionToFace = Vector2.zero;
        specificTimeUniltNextExecution = TimeUniltNextExecution;
    }
}
