using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    [Header("Inventory display:")]
    public GameObject inventoryDisplay;
    public Text headerText;
    public Transform inventoryPosition;
    public Transform tradePosition;
    public Text moneyDisplay;
    public Sprite emptySlotImg;
    public Image[] itemImageDisplays;
    public Text[] quantityDisplays;

    void Start()
    {
        items = new List<Item>();
        quantities = new List<int>();

        InitialiseStartingInventory();
        UpdateDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryDisplay.activeSelf)
                inventoryDisplay.SetActive(false);
            else
            {
                UpdateDisplay();
                inventoryDisplay.transform.position = inventoryPosition.position;
                headerText.text = "INVENTORY";
                inventoryDisplay.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
            RemoveItem(0, 1);

        if (changeMade)
            UpdateDisplay();
    }

    void UpdateDisplay()
    {
        changeMade = false;

        moneyDisplay.text = money.ToString();

        for (int i = 0; i < items.Count; i++)
        {
            itemImageDisplays[i].sprite = items[i].GetImage();
            quantityDisplays[i].text = "x" + quantities[i].ToString();
        }

        for (int i = items.Count; i < 30; i++)
        {
            itemImageDisplays[i].sprite = emptySlotImg;
            quantityDisplays[i].text = "";
        }
    }

    public void OpenTrade()
    {
        inventoryDisplay.transform.position = tradePosition.position;
        headerText.text = "SELL";
        inventoryDisplay.SetActive(true);
    }
}
