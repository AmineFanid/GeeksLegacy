using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ItemFactory 
{
    public static Item CreateItem(string itemName)
    {
        Type resourceType = Type.GetType(itemName);
        if (resourceType == null || !typeof(Item).IsAssignableFrom(resourceType))
        {
            throw new ArgumentException("Invalid resource name: " + itemName);
        }

        Debug.Log((Item)Activator.CreateInstance(resourceType));
        return (Item)Activator.CreateInstance(resourceType);
    }
}
