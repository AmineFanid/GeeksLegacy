using System.IO;
using UnityEngine;

public class LoadUserData : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/UserDataFile.json";
    }

    public UserDataList loadData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<UserDataList>(json);
            }
            catch (IOException e)
            {
                Debug.LogError("Error reading user data file: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.Log("User data file not found, starting new.");
            return null;
        }
    }
}
