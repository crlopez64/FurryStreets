using System.Text;
using System.Collections.Generic;

/// <summary>
/// Class that creates a simple Item.
/// </summary>
public class Item
{
    private readonly int id;
    private readonly byte equipTo;
    private readonly string itemName;
    private readonly string itemDescription;
    private readonly Dictionary<string, byte> statValues;

    /// <summary>
    /// Constructor for a basic Item. For Equip: 1: Head, 2: Body, 3: Leggings, 4: Footwear, 5: Bra, 6: Underwear, 7: Socks
    /// </summary>
    /// <param name="id"></param>
    /// <param name="equipTo"></param>
    /// <param name="itemName"></param>
    /// <param name="itemDescription"></param>
    /// <param name="statValues"></param>
    public Item(int id, byte equipTo, string itemName, string itemDescription, Dictionary<string, byte> statValues)
    {
        this.id = id;
        this.equipTo = equipTo;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.statValues = statValues;
    }
    /// <summary>
    /// Can this item be equipped to the Player?
    /// </summary>
    /// <returns></returns>
    public bool CanEquip()
    {
        return equipTo == 0;
    }
    /// <summary>
    /// Return where this item can equip to, if any. A value of zero means this item is not equippable.
    /// </summary>
    /// <returns></returns>
    public byte GetEquipTo()
    {
        return equipTo;
    }
    /// <summary>
    /// Get the ID of this item.
    /// </summary>
    /// <returns></returns>
    public int GetID()
    {
        return id;
    }
    /// <summary>
    /// Get the name of this item.
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return itemName;
    }
    /// <summary>
    /// Get the description of this item.
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return itemDescription;
    }
    /// <summary>
    /// Get the pathway to the sprite of this item (in a path specifically named "Resources > ItemSprites > itemName).
    /// </summary>
    /// <returns></returns>
    public string GetSpritePath()
    {
        StringBuilder builder = new StringBuilder();
        foreach(char c in itemName)
        {
            if (!char.IsWhiteSpace(c))
            {
                builder.Append(c);
            }
        }
        return "ItemSprites/" + builder.ToString();
    }
    /// <summary>
    /// Get all the stat changes that concern the item.
    /// </summary>
    /// <returns></returns>
    public byte[] GetStatValues()
    {
        byte[] values = new byte[8];
        for(int i = 0; i < 8; i++)
        {
            string currentStat = "";
            switch(i)
            {
                case 0:
                    currentStat = "attack";
                    break;
                case 1:
                    currentStat = "defense";
                    break;
                case 2:
                    currentStat = "charm";
                    break;
                case 3:
                    currentStat = "purity";
                    break;
                case 4:
                    currentStat = "currentHealth";
                    break;
                case 5:
                    currentStat = "currentLust";
                    break;
                case 6:
                    currentStat = "maxHealth";
                    break;
                case 7:
                    currentStat = "maxLust";
                    break;
            }
            byte valueReceived;
            if (!statValues.TryGetValue(currentStat, out valueReceived))
            {
                continue;
            }
            values[i] = valueReceived;
        }
        return values;
    }
}
