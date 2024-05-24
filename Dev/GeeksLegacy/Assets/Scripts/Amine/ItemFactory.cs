// ItemFactory
// Patron de conception Fabrique, permet d'instancier des item d'à peu près partout dans les autres scripts, selon le nom de l'item en string. Permet aussi, d'obtenir le Prefab d'un item selon un nom en string
// Auteur: Amine Fanid 
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
