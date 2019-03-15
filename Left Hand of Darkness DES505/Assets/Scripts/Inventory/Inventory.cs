using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Variables
    protected bool changeMade;
    [Header("Inventory contents:")]
    public int money;

    [HideInInspector]
    public List<Item> items;
    [HideInInspector]
    public List<int> quantities;

    [SerializeField]
    protected List<Item> startingItems;
    [SerializeField]
    protected List<int> startingQuantities;

    //protected List<Sprite> itemImages;
    [Space(10)]
    [Header("Reference:")]
    public ItemsBin itemsBin;
    #endregion

    protected void InitialiseStartingInventory()
    {
        int startingInvCount;

        if (startingItems.Count > startingQuantities.Count)
            startingInvCount = startingQuantities.Count; //Set starting count to reflect information fed into inspector without mismatching
        else
            startingInvCount = startingItems.Count;

        for (int i = 0; i < startingInvCount; i++)
        {
            items.Add(startingItems[i]);
            quantities.Add(startingQuantities[i]);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        changeMade = true;
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        changeMade = true;
    }

    public void AddItem(int itemID, int quantity)
    {
        int index = 0; //Find target inventory slot
        bool alreadyOwned = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemID) //Check if slot is instance of this object
            {
                index = i;
                alreadyOwned = true;
                break;
            }
        }

        if (alreadyOwned) //There is already at least one of these in the inventory
        {
            quantities[index] += quantity; //Set quantity
        }
        else if (items.Count < 30)//There are none of these in the inventory
        {
            Item itemToAdd = itemsBin.items[itemID];
            items.Add(itemToAdd); //Clone item to inventory
            quantities.Add(quantity); //Set quantity
        }

        changeMade = true;
    }

    public void RemoveItem(int itemID, int quantity)
    {
        int index = 0; //Find target inventory slot
        bool alreadyOwned = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemID) //Check if slot is instance of this object
            {
                index = i;
                alreadyOwned = true;
                break;
            }
        }

        if (alreadyOwned) //There is already at least one of these in the inventory
        {
            if (quantities[index] == quantity)
            {
                items.RemoveAt(index); //Remove from the inventory if target quantity is equal to current number in inventory
                //itemImages.RemoveAt(index);
                quantities.RemoveAt(index);
            }
            else if (quantities[index] > quantity)
                quantities[index] -= quantity; //Reduce quantity by the number requested for removal
            else
                print("ERROR in Inventory.cs: " + name + " is trying to drop/sell more of item number " + itemID + " than it currently owns.");
        }
        else //There are none of these in the inventory
        {
            print("ERROR in Inventory.cs: " + name + " is trying to drop/sell item number " + itemID + ", but it does not currently possess any of this item.");
        }

        changeMade = true;
    }
}
