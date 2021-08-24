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
        if (GameManager.Instance.isPlayersTurn)
        {
            if (GameManager.Instance.currentLevel == 1)
            {
                if (GameManager.Instance.Tutorial2.activeSelf)
                {
                    GameManager.Instance.Tutorial2.SetActive(false);
                }
            }
            ItemManager.Instance.ThrowCurrentObject(myItem.ItemPrefab);
            ItemManager.Instance.playerCanvasItems.Remove(this);
            Destroy(gameObject);
            GameManager.Instance.isPlayersTurn = false;
        }
    }
}
