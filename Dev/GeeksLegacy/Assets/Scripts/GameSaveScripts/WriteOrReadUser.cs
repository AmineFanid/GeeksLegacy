using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


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
                Debug.Log(json);
                File.WriteAllText(filePath, json);
                Debug.Log("Nouvel usager, saving");
            }
            else
            {
                Debug.Log("Existe deja, not saving");
            }
        }
        else
        {
            Debug.Log("password corresponf po");
        }

    }

    public void loadUserFromJson()
    {
        UserData user = getUser(); //Ici, utilisé pour obtenir l'usager 
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
                Debug.LogError("Error saving sessionUser: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Existe pas, dans la fonction loadUserFromJson");
        }
    }

    public void updateUserInJSON(UserData currentUser)
    {
        UserDataList userDataList = loadUserDataScript.loadData();
        Debug.Log("before : " + userDataList.users[0].username);
        Debug.Log("before : " + userDataList.users[0].map);

        if (userDataList != null)
        {
            for (int i = 0; i < userDataList.users.Count; i++)
            {
                if (userDataList.users[i].username == currentUser.username)
                {
                    Debug.Log(currentUser);
                    userDataList.users[i] = currentUser; // Overwrite the user with the new data
                    break;
                }
            }
            Debug.Log("before : " + userDataList.users[0].username);
            Debug.Log("before : " + userDataList.users[0].map);
            string json = JsonUtility.ToJson(userDataList, true);
            File.WriteAllText(filePath, json);
            Debug.Log("User data updated in JSON file.");
        }
    }
}
