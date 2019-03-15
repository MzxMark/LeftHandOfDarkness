using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorInventory : Inventory {

    TradeManager tradeManager;
    bool currentlyTrading;

	void Start () {
        items = new List<Item>();
        quantities = new List<int>();

        InitialiseStartingInventory();
    }
}
