using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class WriteOrReadUser : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    private string filePath;
    LoadUserData loadUserDataScript;
    void Start()
    {
        loadUserDataScript = FindFirstObjectByType<LoadUserData>();
        filePath = Application.dataPath + "/UserDataFile.json";
    }


    public UserData getUser()
    {
        if (loadUserDataScript != null)
        {
            string username = usernameInputField.text;
            UserDataList userDataList = loadUserDataScript.loadData();

            if (userDataList != null)
            {
                foreach (UserData user in userDataList.users)
                {
                    if (user.username == username)
                    {
                        Debug.Log("User already exists,  dans la fct getUser");
                        return user;
                    }
                }
            }
            Debug.Log("pas d'usager de ce nom dans la database, dans la fct getUser");
        }
        return null;

    }

    public void saveToJson()
    {
        UserData temp = getUser(); //Ici, utilisé pour modifier le booléen isNewUser
        if (temp == null)
        {
            UserData newUser = new UserData
            {
                username = usernameInputField.text,
                password = passwordInputField.text,
                player = null,
                inventory = null,
                map = null
            };

            UserDataList userDataList = loadUserDataScript.loadData() ?? new UserDataList(); // ?? = null-coalescing operator
            userDataList.users.Add(newUser);

            string json = JsonUtility.ToJson(userDataList, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Nouvel usager, saving");
        }
        else
        {
            Debug.Log("Existe deja, not saving");
        }

    }

    public void loadUserFromJson()
    {
        UserData user = getUser(); //Ici, utilisé pour obtenir l'usager 
        if(user != null)
        {
            /*
            Debug.Log(user.username);
            PlayerPrefs.SetString("Username", user.username);
            */
            try
            {
                string json = JsonUtility.ToJson(user);
                PlayerPrefs.SetString("sessionUser", json);
                PlayerPrefs.Save();
                Debug.Log(json);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MapGenTemp");

            }
            catch (System.Exception e)
            {
                Debug.LogError("Error saving sessionUser: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Existe pas, dans la fonction loadUserFromJson");
        }
    }
}
