// Player
// Classe encapsulant des informations sur le joueur
// Auteurs: Amine Fanid et Henrick Baril
[System.Serializable]

public class Player
{
    public string username;
    private float _LifePoint;
    private CharacterInventory _Inventory;
    public int[,] map;

    public Player(string username, float lifePoint, CharacterInventory inventory)
    {
        this.username = username;
        _LifePoint = lifePoint;
        _Inventory = inventory;
    }

    public float GetLifePoint()
    {
        return this._LifePoint;
    }

    public void SetLifePoint(float NewLifePoint)
    {
        _LifePoint = NewLifePoint;
    }

    public CharacterInventory GetPlayerInventory()
    {
        return _Inventory;
    }

    public void SetPlayerInventory(CharacterInventory NewInventory)
    {
        _Inventory = NewInventory;
    }

    public void TakeDamage(float damage)
    {
        _LifePoint -= damage;
        EventManager.TriggerEvent(EventManager.PossibleEvent.eVieJoueurChange, _LifePoint); // Utilisation de l'observer
    }
}
