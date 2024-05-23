using System.Collections.Generic;

[System.Serializable]

public class UserData
{
    public string username;
    public string password;
    public Player player;
    public CharacterInventory inventory;
    public int mapRows;
    public int mapCols;
    public string map;
}

[System.Serializable]
public class UserDataList
{
    public List<UserData> users = new List<UserData>();
}