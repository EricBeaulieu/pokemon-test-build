using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GlobalTools
{
    public static IEnumerator SmoothTransitionToPosition(Transform curTransform, Vector3 endPos, float duration)
    {
        Vector3 startingPos = curTransform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            curTransform.localPosition = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        curTransform.localPosition = endPos;
    }

    public static string SplitCamelCase(string inputCamelCaseString)
    {
        string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
        return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
    }

    public static string ReplaceUserWithPokemonName(string original,Pokemon user)
    {
        if(original.Contains("<user>") == true)
        {
            return original.Replace("<user>", user.currentName);
        }
        return original;
    }

    public static Vector3 SnapToGrid(Vector3 currentPos)
    {
        currentPos.x = Mathf.FloorToInt(currentPos.x) + Entity.TILE_CENTER_OFFSET;
        currentPos.y = Mathf.FloorToInt(currentPos.y) + Entity.TILE_CENTER_OFFSET;
        currentPos.z = 0;

        return currentPos;
    }

    public static FacingDirections CurrentDirectionFacing(Animator entityAnim)
    {
        Vector2 facingDirection = new Vector2(entityAnim.GetFloat("moveX"), entityAnim.GetFloat("moveY"));

        switch (facingDirection)
        {
            case Vector2 v when v.Equals(Vector2.up):
                return FacingDirections.Up;
            case Vector2 v when v.Equals(Vector2.down):
                return FacingDirections.Down;
            case Vector2 v when v.Equals(Vector2.left):
                return FacingDirections.Left;
            default:
                return FacingDirections.Right;
        }
    }

    public static string FacingDirectionEditorHelper(FacingDirections direction)
    {
        switch (direction)
        {
            case FacingDirections.Up:
                return "Up_Idle";
            case FacingDirections.Down:
                return "Down_Idle";
            case FacingDirections.Left:
                return "Left_Idle";
            default:
                return "Right_Idle";
        }
    }

    public static FacingDirections GetDirectionFacingOnStart(Entity entity)
    {
        string facing = entity.GetComponentInChildren<SpriteRenderer>().sprite.name;

        switch (facing)
        {
            case "Up_Idle":
                return FacingDirections.Up;
            case "Left_Idle":
                return FacingDirections.Left;
            case "Right_Idle":
                return FacingDirections.Right;
            default:
                return FacingDirections.Down;//Make down the default
        }
    }

    public static string ReplacePokemonWithPokemonName(string original, PokemonBase pokemon)
    {
        if (original.Contains("<pokemon>") == true)
        {
            return original.Replace("<pokemon>", pokemon.GetPokedexName());
        }
        return original;
    }

    public static string ReplacePlayerWithTrainerName(string original, PlayerController player)
    {
        if (original.Contains("<player>") == true)
        {
            return original.Replace("<player>", player.TrainerName);
        }
        return original;
    }
}
