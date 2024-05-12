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
                Debug.LogError("Unknown item type: " + itemType);
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
                Debug.LogError("Unknown item type: " + itemType);
                return null;
        }
    }


    /*
    public void InstantiateItemPrefab(Item item, Vector3 position, Quaternion rotation)
    {
        if (item is Dirt && dirtPrefab != null)
        {
            Instantiate(dirtPrefab, position, rotation);
        }
        // Add similar checks for other item types
    }
    */
}
