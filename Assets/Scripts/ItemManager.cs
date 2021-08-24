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

    public GameObject StarEffect;
    public State playerState;
    public Transform playerThrowPoint, AIThrowPoint, tableToTurn, tableToParent;
    public AIController aiController;

    public Transform playerContent, aiContent;
    public GameObject CanvasItemPrefab;

    public List<ItemData> itemList;
    public List<CanvasItem> playerCanvasItems, AICanvasItems;
    public List<Item> playerItemsOnTable, AIItemsOnTheTable;

    private void OnEnable()
    {
        StartCoroutine(aiController.WaitAndThrowRandomAIObject());
    }

    public void SetCanvasObjects(List<GameObject> collectedObjects)
    {
        for (int i = 0; i < collectedObjects.Count; i++)
        {
            CreateCanvasItem(playerContent, collectedObjects[i].GetComponent<Item>().me);
        }
    }

    public void ThrowCurrentObject(GameObject itemPrefab)
    {
        playerState = State.None;
        GameObject item = Instantiate(itemPrefab, playerThrowPoint.position, itemPrefab.transform.rotation, tableToTurn);
        item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, 7), ForceMode.VelocityChange);
        playerItemsOnTable.Add(item.GetComponent<Item>());
        StartCoroutine(aiController.WaitAndMakeMoveAfterPlayer());
    }

    public void ThrowRandomAIObject()
    {
        if (playerCanvasItems.Count == 1 && playerItemsOnTable.Count == 0 && playerState == State.WantMore)
        {
            aiController.currentAIState = State.None;
            GameObject item = Instantiate(playerCanvasItems[0].myItem.ItemPrefab, AIThrowPoint.position, itemList[0].ItemPrefab.transform.rotation, tableToTurn);
            item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, -7), ForceMode.VelocityChange);
            AIItemsOnTheTable.Add(item.GetComponent<Item>());
        }
        else
        {
            if (Random.Range(0, 100) < 75)
            {
                aiController.currentAIState = State.None;
                int random = Random.Range(0, playerCanvasItems.Count);
                GameObject item = Instantiate(playerCanvasItems[random].myItem.ItemPrefab, AIThrowPoint.position, itemList[random].ItemPrefab.transform.rotation, tableToTurn);
                item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, -7), ForceMode.VelocityChange);
                AIItemsOnTheTable.Add(item.GetComponent<Item>());
            }
            else
            {
                if (AICanvasItems.Count > 0)
                {
                    aiController.currentAIState = State.None;
                    int random = Random.Range(0, AICanvasItems.Count);
                    GameObject item = Instantiate(AICanvasItems[random].myItem.ItemPrefab, AIThrowPoint.position, itemList[random].ItemPrefab.transform.rotation, tableToTurn);
                    item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, -7), ForceMode.VelocityChange);
                    AIItemsOnTheTable.Add(item.GetComponent<Item>());
                    GameObject go = AICanvasItems[random].gameObject;
                    AICanvasItems.Remove(AICanvasItems[random]);
                    Destroy(go);
                }
                else
                {
                    aiController.currentAIState = State.None;
                    int random = Random.Range(0, itemList.Count);
                    GameObject item = Instantiate(itemList[random].ItemPrefab, AIThrowPoint.position, itemList[random].ItemPrefab.transform.rotation, tableToTurn);
                    item.GetComponent<ObiSoftbody>().AddForce(new Vector3(Random.Range(-3f, 3f), 0, -7), ForceMode.VelocityChange);
                    AIItemsOnTheTable.Add(item.GetComponent<Item>());
                }
            }
        }
        GameManager.Instance.isPlayersTurn = true;
    }

    public IEnumerator WaitAndTurnTable()
    {
        foreach (var item in AIItemsOnTheTable)
        {
            item.GetComponent<ObiSoftbody>().enabled = false;
        }
        foreach (var item in playerItemsOnTable)
        {
            item.GetComponent<ObiSoftbody>().enabled = false;
        }
        yield return new WaitForSeconds(.5f);
        GameManager.Instance.isTableTurning = true;
        tableToTurn.transform.DORotate(new Vector3(0, tableToTurn.transform.eulerAngles.y + 180, 0), 1).OnComplete(() =>
          {
              aiController.currentAIState = State.None;
              playerState = State.None;
              GameManager.Instance.AIHandsAnimator.SetTrigger("TakeAll");
              GameManager.Instance.PlayerHandsAnimator.SetTrigger("TakeAll");
              GameManager.Instance.isTableTurning = false;
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
                    Destroy(Instantiate(StarEffect, playerCanvasItem.ItemImage.transform.position, Quaternion.identity, playerContent), 1f);
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
