using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public bool isMainScript;
    public void OnDenyEnd()
    {
        if (ItemManager.Instance.playerState == ItemManager.State.Deny && ItemManager.Instance.aiController.currentAIState == ItemManager.State.Deny)
        {
            GameManager.Instance.AIHandsAnimator.SetTrigger("TakeAll");
            GameManager.Instance.PlayerHandsAnimator.SetTrigger("TakeAll");
        }
    }

    public void OnWantMoreEnd()
    {

    }

    public void OnTradeEnd()
    {
        if (ItemManager.Instance.aiController.currentAIState == ItemManager.State.Trade && ItemManager.Instance.playerState == ItemManager.State.Trade)
        {
            ItemManager.Instance.aiController.currentAIState = ItemManager.State.None;
            ItemManager.Instance.playerState = ItemManager.State.None;
            StartCoroutine(ItemManager.Instance.TurnTable());
            isTakeAllWorked = false;
        }
    }

    bool isTakeAllWorked = false;
    public void OnTakeAllEnd()
    {
        if (ItemManager.Instance.playerState == ItemManager.State.Deny && ItemManager.Instance.aiController.currentAIState == ItemManager.State.Deny)
        {
            ItemManager.Instance.TakeItemsBackForAI();
            ItemManager.Instance.TakeItemsBackForPlayer();
            ItemManager.Instance.playerState = ItemManager.State.None;
            ItemManager.Instance.aiController.currentAIState = ItemManager.State.None;
        }
        else
        {
            if (!isTakeAllWorked)
            {
                ItemManager.Instance.MatchItemsForAI();
                ItemManager.Instance.MatchItemsForPlayer();
                isTakeAllWorked = true;
                ItemManager.Instance.playerState = ItemManager.State.None;
                ItemManager.Instance.aiController.currentAIState = ItemManager.State.None;
                if (isMainScript)
                {
                    StartCoroutine(WaitAndNextMove());
                }
            }
        }

    }

    IEnumerator WaitAndNextMove()
    {
        yield return new WaitForSeconds(.5f);
        ItemManager.Instance.aiController.DecideNextMove();
        isTakeAllWorked = true;
    }

    public void TakeAllHalf()
    {
        if (isMainScript)
        {
            if (ItemManager.Instance.playerState == ItemManager.State.Deny && ItemManager.Instance.aiController.currentAIState == ItemManager.State.Deny)
            {
                foreach (var item in ItemManager.Instance.playerItemsOnTable)
                {
                    item.transform.parent = GameManager.Instance.PlayerHandsAnimator.transform.GetChild(0).transform;
                }
                foreach (var item in ItemManager.Instance.AIItemsOnTheTable)
                {
                    item.transform.parent = GameManager.Instance.AIHandsAnimator.transform.GetChild(0).transform;
                }
            }
            else
            {
                foreach (var item in ItemManager.Instance.playerItemsOnTable)
                {
                    item.transform.parent = GameManager.Instance.AIHandsAnimator.transform.GetChild(0).transform;
                }
                foreach (var item in ItemManager.Instance.AIItemsOnTheTable)
                {
                    item.transform.parent = GameManager.Instance.PlayerHandsAnimator.transform.GetChild(0).transform;
                }
            }
        }
    }
}
