using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public int playerPosX;
    public int playerPosY;
    public string trainerName;
    public List<PokemonSaveData> savedParty;
    public FacingDirections savedDirection;
}