using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private Sprite itemSprite;
    [SerializeField]
    private GameObject itemPrefab;

    public Sprite ItemSprite
    {
        get
        {
            return itemSprite;
        }
    }

    public GameObject ItemPrefab
    {
        get
        {
            return itemPrefab;
        }
    }
}
