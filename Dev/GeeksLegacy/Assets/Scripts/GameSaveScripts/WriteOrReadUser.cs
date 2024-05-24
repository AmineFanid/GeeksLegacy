using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class WriteOrReadUser : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    private string _FilePath;
    LoadUserData loadUserDataScript;
    void Start()
    {
        loadUserDataScript = FindFirstObjectByType<LoadUserData>();
        _FilePath = Application.dataPath + "/UserDataFile.json";
    }

    public bool checkPassword(string password)
    {
        string pattern = @"^(?=.*[A-Z])(?=.*\d).+$";
        return Regex.IsMatch(password, pattern);
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
                        return user;
                    }
                }
            }
            Debug.Log("Pas d'usager de ce nom dans la database");
        }
        return null;

    }

    public void saveToJson()
    {
        if (checkPassword(passwordInputField.text))
        {
            UserData temp = getUser();
            if (temp == null)
            {
                CharacterInventory newInventory = new CharacterInventory();
                Player newPlayer = new Player(usernameInputField.text, 100, newInventory);
                int[,] emptyMap = new int[0, 0];
                UserData newUser = new UserData
                {
                    username = usernameInputField.text,
                    password = passwordInputField.text,
                    player = newPlayer,
                    inventory = newInventory,
                    mapRows = 0,
                    mapCols = 0,
                    map = ArrayUtils.ConvertToString(emptyMap)
                };

                UserDataList userDataList = loadUserDataScript.loadData() ?? new UserDataList(); // ?? = null-coalescing operator
                userDataList.users.Add(newUser);
                string json = JsonUtility.ToJson(userDataList, true);
                File.WriteAllText(_FilePath, json);
            }
            else
            {
                Debug.Log("Existe deja, on n'enregistre pas l'usager");
            }
        }
        else
        {
            Debug.Log("Le password ne correspond pas");
        }

    }

    public void loadUserFromJson()
    {
        UserData user = getUser(); 
        if(user != null)
        {
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
                Debug.LogError("Erreur dans l'enregistrement du PlayerPrefs sessionUser: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Usager n'existe pas, dans la fonction loadUserFromJson");
        }
    }

    public void updateUserInJSON(UserData currentUser)
    {
        UserDataList userDataList = loadUserDataScript.loadData();
        if (userDataList != null)
        {
            for (int i = 0; i < userDataList.users.Count; i++)
            {
                if (userDataList.users[i].username == currentUser.username)
                {
                    Debug.Log(currentUser);
                    userDataList.users[i] = currentUser;
                    break;
                }
            }
            string json = JsonUtility.ToJson(userDataList, true);
            File.WriteAllText(_FilePath, json);
        }
    }
}
