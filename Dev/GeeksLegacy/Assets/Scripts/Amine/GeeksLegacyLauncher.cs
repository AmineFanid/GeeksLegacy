using UnityEngine;
[DefaultExecutionOrder(15)]

public class GeeksLegacyLauncher : MonoBehaviour
{
    ProceduralGeneration proceduralGeneration;
    ControlCharacters controlCharacters;
    GameDataManager gameDataManager;
    UserData currentUser;
    public CharacterInventory inventory;
    public Player player;
    int[,] map = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            gameDataManager = FindFirstObjectByType<GameDataManager>();
            currentUser = gameDataManager.getDataUser();
            setUpUser();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading UserData un ControlCharacters SCRIPT : " + e.Message);

        }
        controlCharacters = FindFirstObjectByType<ControlCharacters>();
        proceduralGeneration = FindFirstObjectByType<ProceduralGeneration>();
        proceduralGeneration.prepareProceduralGeneration();
        proceduralGeneration.Generation();
        proceduralGeneration.SpawnCharacter();
        //controlCharacters.setPlayer(currentUser.player);
    }

    // Update is called once per frame
    void Update()
    {
        //proceduralGeneration.RenderMap();
    }

    public void setUpUser()
    {
        if(currentUser != null)
        {
            inventory = currentUser.inventory != null ? currentUser.inventory : new CharacterInventory();
            player = currentUser.player != null ? currentUser.player : new Player(currentUser.username, 100.0f, inventory);
            if(currentUser.map != null)
            {
                map = currentUser.map;
                proceduralGeneration.setMap(map);
            }
        }      
    }

    public Player getPlayerFromDB()
    {
        return player;
    }

    //public
}
