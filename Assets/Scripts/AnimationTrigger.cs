using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public bool isMainScript;
    public void OnDenyEnd()
    {
        if (isMainScript)
        {
            if (ItemManager.Instance.aiController.currentAIState == ItemManager.State.Deny)
            {
                //ikiside deny etti                
                GameManager.Instance.AIHandsAnimator.SetTrigger("TakeAll");
                GameManager.Instance.PlayerHandsAnimator.SetTrigger("TakeAll");
            }
            else
            {
                //ai ı deny ettir
                StartCoroutine(ItemManager.Instance.aiController.WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
                ItemManager.Instance.aiController.currentAIState = ItemManager.State.Deny;
            }
        }
        else
        {
            if (ItemManager.Instance.playerState == ItemManager.State.Deny)
            {
                //ikiside deny etti
                GameManager.Instance.AIHandsAnimator.SetTrigger("TakeAll");
                GameManager.Instance.PlayerHandsAnimator.SetTrigger("TakeAll");
            }
            else
            {

            }
        }
    }

    public void OnWantMoreEnd()
    {
        if (isMainScript)
        {
            //tell ai give more
            if (ItemManager.Instance.AIItemsOnTheTable.Count < 2)
            {
                ItemManager.Instance.ThrowRandomAIObject();
            }
            else
            {
                StartCoroutine(ItemManager.Instance.aiController.WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
                ItemManager.Instance.aiController.currentAIState = ItemManager.State.Deny;
            }
        }
        else
        {

        }
    }

    public void OnTradeEnd()
    {
        if (isMainScript)
        {
            if (ItemManager.Instance.aiController.currentAIState == ItemManager.State.Trade)
            {
                //ikiside kabul etti
                StartCoroutine(ItemManager.Instance.WaitAndTurnTable());
            }
            else
            {
                StartCoroutine(ItemManager.Instance.aiController.WaitAndMakeMoveAfterPlayer());
            }
        }
        else
        {
            if (ItemManager.Instance.playerState == ItemManager.State.Trade)
            {
                //ikiside kabul etti
                StartCoroutine(ItemManager.Instance.WaitAndTurnTable());
            }
            else
            {

            }
        }
    }


    public void OnTakeAllEnd()
    {
        if (isMainScript)
        {
            if (ItemManager.Instance.playerState == ItemManager.State.Deny)
            {
                 ItemManager.Instance.TakeItemsBackForAI();
                ItemManager.Instance.TakeItemsBackForPlayer();
            }
            else
            {
                ItemManager.Instance.MatchItemsForAI();
                ItemManager.Instance.MatchItemsForPlayer();
            }
        }
        else
        {

        }
    }

    public void TakeAllHalf()
    {
        if (isMainScript)
        {
            if (ItemManager.Instance.playerState == ItemManager.State.Deny)
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
        else
        {

        }
    }
}
