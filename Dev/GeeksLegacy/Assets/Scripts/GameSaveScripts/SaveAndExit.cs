using UnityEngine;

public class SaveAndExit : MonoBehaviour
{
    ProceduralGeneration proceduralGeneration;
    Player player;
    CharacterInventory inventory;
    GameDataManager gameDataManager;
    UserData currentUser;
    ControlCharacters controlCharacters;


    int[,] map = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameDataManager = FindFirstObjectByType<GameDataManager>();
        proceduralGeneration = FindFirstObjectByType<ProceduralGeneration>();
        controlCharacters = FindFirstObjectByType<ControlCharacters>();

    }

    public void saveGame()
    {
        currentUser = gameDataManager.getDataUser();
        map = proceduralGeneration.getMap();
        player = controlCharacters.findPlayerObject();
        //inventory = player.GetPlayerInventory();
        gameDataManager.updateCurrentUser(player, map);
        _ExitGame();
    }

    private void _ExitGame()
    {
        Application.Quit();
    }
}
