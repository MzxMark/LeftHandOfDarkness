using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour {

    #region Variables
    public int id;
    [SerializeField]
    protected int value;
    [SerializeField]
    protected float weight;
    [SerializeField]
    protected string itemName;
    [SerializeField]
    protected Sprite image;

    public int quantity;

    public GameObject pickupPrompt;

    public Inventory inventory;
    protected Transform player;
    #endregion

    void Start() {
        try
        {
            player = GameObject.Find("Player").transform;
        }
        catch (Exception)
        {
            print("ERROR in Item.cs: Could not find object \"Player\" in the current scene.");
        }
    }

    void Update() {
        if (Vector3.Distance(transform.position, player.position) < 5)
        {
            if (!pickupPrompt.activeSelf)
                pickupPrompt.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
                Pickup();
        }
        else if (pickupPrompt.activeSelf)
                pickupPrompt.SetActive(false);
    }

    void Pickup()
    {
        player.GetComponent<Inventory>().AddItem(id, quantity);
        Destroy(gameObject);
    }

    public void SetID(int newID)
    {
        id = newID;
    }

    public int GetID()
    {
        return id;
    }

    public int GetValue()
    {
        return value;
    }

    public float GetWeight()
    {
        return weight;
    }

    public string GetName()
    {
        return itemName;
    }

    public Sprite GetImage()
    {
        return image;
    }
}
