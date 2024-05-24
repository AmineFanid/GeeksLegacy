using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    UserData currentUser;
    WriteOrReadUser worUser;
    void Start()
    {
        worUser = FindFirstObjectByType<WriteOrReadUser>();
        try
        {
            string json = PlayerPrefs.GetString("sessionUser");
            if (!string.IsNullOrEmpty(json))
            {
                currentUser = JsonUtility.FromJson<UserData>(json);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading UserData: " + e.Message);
        }
    }

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
