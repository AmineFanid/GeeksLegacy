using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject ironPrefab;
    public GameObject woodPrefab; 

    public Item CreateItem(string itemType)
    {
        switch (itemType)
        {
            case "Dirt":
                return new Dirt(dirtPrefab);
            case "Iron":
                return new Iron();
            case "Wood":
                return new Wood();
            default:
                return null;
        }
    }

    public GameObject getPrefab(string itemType)
    {
        switch (itemType)
        {
            case "Dirt":
                return dirtPrefab;
            case "Iron":
                return ironPrefab;
            case "Wood":
                return woodPrefab;
            default:
                return null;
        }
    }
}
