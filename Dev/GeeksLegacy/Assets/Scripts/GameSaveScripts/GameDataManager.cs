using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    UserData currentUser;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}
