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
        gameDataManager = FindFirstObjectByType<GameDataManager>();
        currentUser = gameDataManager.getDataUser();

        controlCharacters = FindFirstObjectByType<ControlCharacters>();
        proceduralGeneration = FindFirstObjectByType<ProceduralGeneration>();
        proceduralGeneration.setMap(ArrayUtils.ConvertToIntArray(currentUser.map, currentUser.mapRows, currentUser.mapCols));


        proceduralGeneration.prepareProceduralGeneration();
        proceduralGeneration.Generation();
        proceduralGeneration.SpawnCharacter();
    }

    public Player getPlayerFromDB()
    {
        return currentUser.player;
    }
}
