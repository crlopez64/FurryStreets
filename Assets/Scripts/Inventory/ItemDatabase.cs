using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of all the items in the gme.
/// </summary>
public class ItemDatabase : MonoBehaviour
{
    private List<Item> database;

    public void Awake()
    {
        CreateDatabase();
    }

    /// <summary>
    /// Get an item from the normal Item database via the asking ID.
    /// </summary>
    /// <param name="askingID"></param>
    /// <returns></returns>
    public Item GetItem(int askingID)
    {
        if ((askingID >= database.Count) || (askingID == 0))
        {
            return null;
        }
        return database[askingID];
    }
    /// <summary>
    /// Get an item via the asking name.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public Item GetItem(string askingName)
    {
        foreach(Item item in database)
        {
            if (item.GetName() == askingName)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// Creates all the items in the game.
    /// </summary>
    private void CreateDatabase()
    {
        //Make all the items
        database = new List<Item>()
        {
            new Item(0, 0, "Null Item", "An empty item that is secret to the Shadows of the Game. Heals 0 HP.",
            new Dictionary<string, byte>
            {
                {"currentHealth", 0 }
            }),
            new Item(1, 0, "Taco", "A nice hard shelled taco. Heals 5 HP.",
            new Dictionary<string, byte>
            {
                {"currentHealth", 5 }
            }),
            new Item(2, 1, "Test_Winter Cap", "A simple cap made of wool. Increases Defense.",
            new Dictionary<string, byte>
            {
                {"defense", 2 }
            }),
            new Item(3, 1, "Rusty Brass Horn Rings", "Very gently used rings to be placed on horns. Adds some slight Defense and increases Charm.",
            new Dictionary<string, byte>
            {
                {"defense", 2 },
                {"charm", 1 }
            }),
            new Item(4, 1, "Test_Baseball Cap", "A nice cap with less than ample shade. Adds some slight Attack as well as Defense.",
            new Dictionary<string, byte>
            {
                {"attack", 2 },
                {"defense", 1 }
            })
        };
        database.TrimExcess();
    }
}
