using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Obi;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;

    public static ItemManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public enum State
    {
        None,
        Trade,
        WantMore,
        Deny
    }

    public State playerState;
    public Transform playerThrowPoint, AIThrowPoint, table;
    public AIController aiController;

    public Transform playerContent, aiContent;
    public GameObject CanvasItemPrefab;

    public List<ItemData> itemList;
    public List<CanvasItem> playerCanvasItems, AICanvasItems;
    public List<Item> playerItemsOnTable, AIItemsOnTheTable;
    public List<ItemData> copyforItemData;

    private void Start()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            copyforItemData.Add(itemList[i]);
        }
        for (int i = 0; i < playerCanvasItems.Count; i++)
        {
            int rand = Random.Range(0, copyforItemData.Count);
            playerCanvasItems[i].myItem = copyforItemData[rand];

            copyforItemData.RemoveAt(rand);

            playerCanvasItems[i].Initialize();
        }
    }

    public void ThrowCurrentObject(GameObject itemPrefab)
    {
        playerState = State.None;
        GameObject item = Instantiate(itemPrefab, playerThrowPoint.position, Quaternion.identity, table);
        item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, 10), ForceMode.VelocityChange);
        playerItemsOnTable.Add(item.GetComponent<Item>());
        StartCoroutine(WaitAndMakeMove());
    }

    IEnumerator WaitAndMakeMove()
    {
        yield return new WaitForSeconds(.5f);
        aiController.DecideNextMove();
    }

    public void ThrowRandomAIObject()
    {
        if (AICanvasItems.Count > 0)
        {
            aiController.currentAIState = State.None;
            int random = Random.Range(0, AICanvasItems.Count);
            GameObject item = Instantiate(AICanvasItems[random].myItem.ItemPrefab, AIThrowPoint.position, Quaternion.identity, table);
            item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, -10), ForceMode.VelocityChange);
            AIItemsOnTheTable.Add(item.GetComponent<Item>());
            GameObject go = AICanvasItems[random].gameObject;
            AICanvasItems.Remove(AICanvasItems[random]);
            Destroy(go);
        }
        else
        {
            aiController.currentAIState = State.None;
            int random = Random.Range(0, itemList.Count);
            GameObject item = Instantiate(itemList[random].ItemPrefab, AIThrowPoint.position, Quaternion.identity, table);
            item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, -10), ForceMode.VelocityChange);
            AIItemsOnTheTable.Add(item.GetComponent<Item>());
        }
    }


    public IEnumerator TurnTable()
    {
        yield return new WaitForSeconds(.5f);
        table.transform.DORotate(new Vector3(0, table.transform.eulerAngles.y + 180, 0), 1).OnComplete(() =>
          {
              aiController.currentAIState = State.None;
              playerState = State.None;
              GameManager.Instance.AIHandsAnimator.SetTrigger("TakeAll");
              GameManager.Instance.PlayerHandsAnimator.SetTrigger("TakeAll");
          }
        );
    }

    bool isMatched = false;
    public void MatchItemsForPlayer()
    {
        List<CanvasItem> tempCanvasItem = new List<CanvasItem>();
        foreach (var AIItem in AIItemsOnTheTable)
        {
            foreach (var playerCanvasItem in playerCanvasItems)
            {
                if (AIItem.me == playerCanvasItem.myItem)
                {
                    tempCanvasItem.Add(playerCanvasItem);
                    GameManager.Instance.PlayerPoints += 10;
                    isMatched = true;
                }
            }
            if (!isMatched)
            {
                CreateCanvasItem(playerContent, AIItem.me);
            }
            isMatched = false;
        }
        foreach (var item in tempCanvasItem)
        {
            playerCanvasItems.Remove(item);
            Destroy(item.gameObject);
        }
        tempCanvasItem.Clear();
        foreach (var item in AIItemsOnTheTable)
        {
            Destroy(item.gameObject);
        }
        AIItemsOnTheTable.Clear();
        if (playerCanvasItems.Count == 0)
        {
            StartCoroutine(GameManager.Instance.WaitAndGameWin());
        }
    }

    public void MatchItemsForAI()
    {
        List<CanvasItem> tempCanvasItem = new List<CanvasItem>();
        foreach (var playerItem in playerItemsOnTable)
        {
            foreach (var AICanvasItem in AICanvasItems)
            {
                if (AICanvasItem.myItem == playerItem.me)
                {
                    tempCanvasItem.Add(AICanvasItem);
                    GameManager.Instance.AIPoints += 10;
                    isMatched = true;
                }
            }
            if (!isMatched)
            {
                CreateCanvasItem(aiContent, playerItem.me);
            }
            isMatched = false;
        }
        foreach (var item in tempCanvasItem)
        {
            AICanvasItems.Remove(item);
            Destroy(item.gameObject);
        }
        tempCanvasItem.Clear();
        foreach (var item in playerItemsOnTable)
        {
            Destroy(item.gameObject);
        }
        playerItemsOnTable.Clear();
    }

    public void TakeItemsBackForPlayer()
    {
        foreach (var playerItem in playerItemsOnTable)
        {
            CreateCanvasItem(playerContent, playerItem.me);
        }
        foreach (var item in playerItemsOnTable)
        {
            Destroy(item.gameObject);
        }
        playerItemsOnTable.Clear();
    }

    public void TakeItemsBackForAI()
    {
        foreach (var AIItem in AIItemsOnTheTable)
        {
            CreateCanvasItem(aiContent, AIItem.me);
        }
        foreach (var item in AIItemsOnTheTable)
        {
            Destroy(item.gameObject);
        }
        AIItemsOnTheTable.Clear();
    }

    public void CreateCanvasItem(Transform parent, ItemData data)
    {
        CanvasItem canvasItem = Instantiate(CanvasItemPrefab, parent).GetComponent<CanvasItem>();
        canvasItem.myItem = data;
        canvasItem.Initialize();
        if (parent == playerContent)
        {
            playerCanvasItems.Add(canvasItem);
        }
        else
        {
            AICanvasItems.Add(canvasItem);
        }
    }
}
