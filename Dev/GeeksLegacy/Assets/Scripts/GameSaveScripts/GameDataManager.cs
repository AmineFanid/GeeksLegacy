using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    UserData currentUser;
    WriteOrReadUser worUser;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worUser = FindFirstObjectByType<WriteOrReadUser>();
        try
        {
            string json = PlayerPrefs.GetString("sessionUser");
            if (!string.IsNullOrEmpty(json)) // string.IsNullOrEmpty() = Indicates whether the specified string is null or an empty string ("").
            {
                currentUser = JsonUtility.FromJson<UserData>(json);
                
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading UserData: " + e.Message);
        }
    }

    // Update is called once per frame

    public UserData getDataUser()
    {
        if(currentUser != null)
        {
            return currentUser;
        }
        return null;
    }

    public void updateCurrentUser(Player updatedPlayer, int[,] newMap)
    {
        currentUser.player = updatedPlayer;
        currentUser.inventory = updatedPlayer.GetPlayerInventory();
        currentUser.mapRows = newMap.GetLength(0);
        currentUser.mapCols = newMap.GetLength(1);
        currentUser.map = ArrayUtils.ConvertToString(newMap);
        worUser.updateUserInJSON(currentUser);
        Debug.Log("gamedatamanagerrrr");
    }

}
