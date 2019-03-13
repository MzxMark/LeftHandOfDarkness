using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    #region Variables
    public static TradeManager instance;
    PlayerInventory player;
    VendorInventory currentVendor;

    public GameObject tradeDisplay;
    public GameObject inventoryDisplay;
    public Text moneyDisplay;
    public Sprite emptySlotImg;
    public Image[] itemImageDisplays;
    public Text[] quantityDisplays;

    public Button[] buyButtons;
    public Button[] sellButtons;

    private bool changeMade;
    #endregion

    void Awake()
    {
        instance = this;

        try
        {
            player = GameObject.Find("Player").GetComponent<PlayerInventory>();
        }
        catch (Exception)
        {
            print("ERROR in TradeManager.cs: No object \"Player\" exists in the current scene or it does not contain a PlayerInventory.cs script component.");
        }
    }

    void Update()
    {
        if (tradeDisplay.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            tradeDisplay.SetActive(false);
            inventoryDisplay.SetActive(false);
        }

        if (changeMade)
            UpdateDisplay();
    }

    public void SellItem(int itemIndex)
    {
        Item itemToSell = player.items[itemIndex]; //Get reference to chosen item
        int value = itemToSell.GetValue();

        if (currentVendor.money >= value) //Check if vendor can afford it
        {
            player.AddMoney(value);
            currentVendor.RemoveMoney(value); //Adjust money for both parties

            currentVendor.AddItem(itemToSell.id, 1);
            player.RemoveItem(itemToSell.id, 1); //Adjust inventory for both parties

            changeMade = true;
        }
    }

    public void BuyItem(int itemIndex)
    {
        Item itemToBuy = currentVendor.items[itemIndex]; //Get reference to chosen item
        int value = itemToBuy.GetValue();

        if (player.money >= value) //Check if player can afford it
        {
            currentVendor.AddMoney(value);
            player.RemoveMoney(value); //Adjust money for both parties

            player.AddItem(itemToBuy.id, 1);
            currentVendor.RemoveItem(itemToBuy.id, 1); //Adjust inventory for both parties

            changeMade = true;
        }
    }

    public void OpenTrade(VendorInventory vendor)
    {
        player.OpenTrade();
        currentVendor = vendor;

        UpdateDisplay();
        tradeDisplay.SetActive(true);
    }

    void UpdateDisplay()
    {
        changeMade = false;

        int sellableItems = player.items.Count;
        int buyableItems = currentVendor.items.Count;

        moneyDisplay.text = currentVendor.money.ToString();

        #region Images
        for (int i = 0; i < currentVendor.items.Count; i++)
        {
            //Update quantities and newly filled slots
            itemImageDisplays[i].sprite = currentVendor.items[i].GetImage();
            quantityDisplays[i].text = "x" + currentVendor.quantities[i].ToString();
        }

        for (int i = currentVendor.items.Count; i < 30; i++)
        {
            //Update images and quantities for newly empty slots
            itemImageDisplays[i].sprite = emptySlotImg;
            quantityDisplays[i].text = "";
        }
        #endregion

        #region Sell buttons
        for (int i = 0; i < sellableItems; i++)
        {
            //Activate necessary buttons on sell side of trade
            //sellButtons[i].interactable = true;
            sellButtons[i].gameObject.SetActive(true);
        }
        for (int i = sellableItems; i < sellButtons.Length; i++)
        {
            //Deactivate surplus buttons on sell side of trade
            //sellButtons[i].interactable = false;
            sellButtons[i].gameObject.SetActive(false);
        }
        #endregion

        #region Buy buttons
        for (int i = 0; i < buyableItems; i++)
        {
            //Activate necessary buttons on buy side of trade
            //buyButtons[i].interactable = true;
            buyButtons[i].gameObject.SetActive(true);
        }
        for (int i = buyableItems; i < buyButtons.Length; i++)
        {
            //Deactivate surplus buttons on buy side of trade
            //buyButtons[i].interactable = false;
            buyButtons[i].gameObject.SetActive(false);
        }
        #endregion
    }
}
