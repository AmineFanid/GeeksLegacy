using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static GameObject _playerController;

    public static void SetPlayerController(GameObject controller)
    {
        _playerController = controller;
    }

    public static GameObject GetPlayer() { 
        return _playerController;
    }
}