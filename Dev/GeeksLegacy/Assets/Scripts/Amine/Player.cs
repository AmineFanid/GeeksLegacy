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
        this._LifePoint = lifePoint;
        this._Inventory = inventory;
    }

    public float GetLifePoint()
    {
        return this._LifePoint;
    }

    public void SetLifePoint(float NewLifePoint)
    {
        this._LifePoint = NewLifePoint;
    }

    public CharacterInventory GetPlayerInventory()
    {
        return this._Inventory;
    }

    public void SetPlayerInventory(CharacterInventory NewInventory)
    {
        this._Inventory = NewInventory;
    }

    public void TakeDamage(float damage)
    {
        this._LifePoint -= damage;
        EventManager.TriggerEvent(EventManager.PossibleEvent.eVieJoueurChange, this._LifePoint);
    }
}
