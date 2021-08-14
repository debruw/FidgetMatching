using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasItem : MonoBehaviour
{
    public ItemData myItem;
    public Image ItemImage;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (myItem != null)
        {
            ItemImage.sprite = myItem.ItemSprite;
        }
    }

    public void ButtonClick()
    {
        ItemManager.Instance.ThrowCurrentObject(myItem.ItemPrefab);
        ItemManager.Instance.playerCanvasItems.Remove(this);
        Destroy(gameObject);
    }
}
