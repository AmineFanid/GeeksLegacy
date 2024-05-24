// Dirt
// Permet d'instancier un objet Dirt
// Auteurs: Amine Fanid et Henrick Baril
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