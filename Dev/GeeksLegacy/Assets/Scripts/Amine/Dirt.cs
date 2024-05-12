

public class Dirt : Item
{

    public Dirt(UnityEngine.GameObject itemPrefab)
    {
        this.itemPrefab = itemPrefab;
        itemName = "Dirt";
        itemType = "Material";
        itemDurability = 25.0f;
    }


}