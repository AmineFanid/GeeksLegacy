using UnityEngine;

public class SaveAndExit : MonoBehaviour
{
    ProceduralGeneration proceduralGeneration;
    Player player;
    GameDataManager gameDataManager;
    UserData currentUser;
    ControlCharacters controlCharacters;
    int[,] map = null;

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
        gameDataManager.updateCurrentUser(player, map);
        _ExitGame();
    }

    private void _ExitGame()
    {
        Application.Quit();
    }
}
