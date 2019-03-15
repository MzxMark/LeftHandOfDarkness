using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsBin", menuName = "ItemsBinSO", order = 0)]
public class ItemsBin : ScriptableObject {

    public List<Item> items;

    void Awake()
    {
        for (int i = 0; i < items.Count; i++)
            items[i].SetID(i);
    }
}
