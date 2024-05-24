// GeeksLegacyLauncher
// Permet d'instancier une partie de Geek's Legacy, et communique avec les autres éléments du jeu pour la gestion de la partie.
// Auteur: Amine Fanid 
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
