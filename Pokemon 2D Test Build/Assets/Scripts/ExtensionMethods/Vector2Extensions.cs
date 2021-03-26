using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 GetDirection(this Vector2 original,FacingDirections facingDir)
    {
        switch (facingDir)
        {
            case FacingDirections.Up:
                return Vector2.up;
            case FacingDirections.Down:
                return Vector2.down;
            case FacingDirections.Left:
                return Vector2.left;
            default://Right
                return Vector2.right;
        }
    }
}
