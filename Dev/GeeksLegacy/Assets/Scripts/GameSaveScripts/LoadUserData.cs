using System.IO;
using UnityEngine;

public class LoadUserData : MonoBehaviour
{
    private string _FilePath;

    void Start()
    {
        _FilePath = Application.dataPath + "/UserDataFile.json";
    }

    public UserDataList loadData()
    {
        if (File.Exists(_FilePath))
        {
            try
            {
                string json = File.ReadAllText(_FilePath);
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
