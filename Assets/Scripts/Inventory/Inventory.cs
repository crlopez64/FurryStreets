using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of everything that is Inventory.
/// </summary>
public class Inventory : MonoBehaviour
{
    //TODO: Adjust equipped inventory based on Selected character.
    private List<Item> currentInventory;
    private List<Item> storageInventory;
    private Item[] equippedInventoryWolf;
    private Item[] equippedInventoryFox;
    private Item[] equippedInventoryBunny;
    private Item[] equippedInventoryFour;

    private void Awake()
    {
        currentInventory  = new List<Item>(12);
        storageInventory  = new List<Item>(80);
        equippedInventoryWolf = new Item[5];
    }

    /// <summary>
    /// Use and consume an item. Returns the item reference to use. Returns null if the item is equippable.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Item UseItem(Item item)
    {
        if (!currentInventory.Contains(item))
        {
            return null;
        }
        if (item.GetEquipTo() == 0)
        {
            return null;
        }
        Item consumedItem = RemoveItem(item);
        return consumedItem;
    }
    /// <summary>
    /// Add an item to inventory. Return true if there's space to do so; return false if not.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToInventory(Item item)
    {
        if (currentInventory.Count < currentInventory.Capacity)
        {
            currentInventory.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Add an item to storage. Return true if there's space to do so; return false if not.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToStorage(Item item)
    {
        if (storageInventory.Count < storageInventory.Capacity)
        {
            storageInventory.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Equip an item into inventory. If cannot equip, or something is already equipped, return false.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool EquipItem(Item item)
    {
        if (item.GetEquipTo() == 0)
        {
            return false;
        }
        if (equippedInventoryWolf[item.GetEquipTo()] != null)
        {
            Item toEquip = RemoveItem(item);
            equippedInventoryWolf[toEquip.GetEquipTo()] = toEquip;
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Return if the equip spot is taken.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool EquipSpotTaken(int position)
    {
        return equippedInventoryWolf[position] != null;
    }
    /// <summary>
    /// Unequip an item. Return -1 if item does not exist, 0 if current inventory is maxed out, or 1 if successful.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int UnEquipItem(Item item)
    {
        if (equippedInventoryWolf[item.GetEquipTo()] == null)
        {
            return -1;
        }
        if (currentInventory.Count >= currentInventory.Capacity)
        {
            return 0;
        }
        Item unequipped = equippedInventoryWolf[item.GetEquipTo()];
        equippedInventoryWolf[item.GetEquipTo()] = null;
        currentInventory.Add(unequipped);
        return 1;
    }
    /// <summary>
    /// Swap an equipped item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool SwapEquip(Item item)
    {
        if (equippedInventoryWolf[item.GetEquipTo()] == null)
        {
            equippedInventoryWolf[item.GetEquipTo()] = item;
            currentInventory.Remove(item);
            return true;
        }
        Item unequipped = equippedInventoryWolf[item.GetEquipTo()];
        equippedInventoryWolf[item.GetEquipTo()] = item;
        currentInventory.Remove(item);
        currentInventory.Add(unequipped);
        return true;
    }
    /// <summary>
    /// How many things is the Player holding?
    /// </summary>
    /// <returns></returns>
    public int InventoryCurrentSize()
    {
        return currentInventory.Count;
    }
    /// <summary>
    /// Remove an item from Inventory. If does not exist, return null.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Item RemoveItem(Item item)
    {
        Item toRemove;
        foreach(Item i in currentInventory)
        {
            if (i == item)
            {
                toRemove = i;
                currentInventory.Remove(i);
                return toRemove;
            }
        }
        return null;
    }
    /// <summary>
    /// Remove an item from Storage. If does not exist, return null.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Item RemoveFromStorage(Item item)
    {
        Item toRemove;
        foreach (Item i in storageInventory)
        {
            if (i == item)
            {
                toRemove = i;
                storageInventory.Remove(i);
                return toRemove;
            }
        }
        return null;
    }
    /// <summary>
    /// Get the current inventory.
    /// </summary>
    /// <returns></returns>
    public List<Item> GetCurrentInventory()
    {
        return currentInventory;
    }
}
