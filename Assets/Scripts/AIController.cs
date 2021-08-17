using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public ItemManager.State currentAIState;

    int matchCount = 0;
    public IEnumerator WaitAndMakeMoveAfterPlayer()
    {
        if (GameManager.Instance.isTableTurning || GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            yield break;
        }
        yield return new WaitForSeconds(.5f);
        foreach (var aiCanvasItem in ItemManager.Instance.AICanvasItems)
        {
            foreach (var playerItemOnTable in ItemManager.Instance.playerItemsOnTable)
            {
                if (aiCanvasItem.myItem == playerItemOnTable.me)
                {
                    //we have a match
                    //we can press trade
                    matchCount++;
                }
            }
        }
        if (ItemManager.Instance.playerItemsOnTable.Count == 1 && ItemManager.Instance.AIItemsOnTheTable.Count == 0 && matchCount == 1)
        {
            //Direkt kabule basabiliriz
            StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "TRADE"));
            currentAIState = ItemManager.State.Trade;
        }
        else if (ItemManager.Instance.playerItemsOnTable.Count == 1 && ItemManager.Instance.AIItemsOnTheTable.Count == 1 && matchCount == 0)
        {
            //50 fazlasını iste - 50 kabul
            if (Random.Range(0, 10) > 4)
            {
                //50 kabul
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "TRADE"));
                currentAIState = ItemManager.State.Trade;
            }
            else
            {
                //50 fazlasını iste
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "WANTMORE"));
                currentAIState = ItemManager.State.WantMore;
            }
        }
        else if (ItemManager.Instance.playerItemsOnTable.Count == 0 && ItemManager.Instance.AIItemsOnTheTable.Count == 1)
        {
            // fazlasını ister
            StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "WANTMORE"));
            currentAIState = ItemManager.State.WantMore;
        }
        else if (ItemManager.Instance.playerItemsOnTable.Count > 1 && matchCount == 0)
        {
            //reddet
            StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
            currentAIState = ItemManager.State.Deny;
        }
        else if (ItemManager.Instance.playerItemsOnTable.Count > 1 && ItemManager.Instance.playerCanvasItems.Count == 1)
        {
            //reddet
            StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
            currentAIState = ItemManager.State.Deny;
        }
        else
        {
            if (matchCount > 0)
            {
                //Kabul edebilir
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "TRADE"));
                currentAIState = ItemManager.State.Trade;
            }
            else
            {
                //daha fazlasını iste
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "WANTMORE"));
                currentAIState = ItemManager.State.WantMore;
            }
        }
        matchCount = 0;
    }

    public IEnumerator WaitAndThrowRandomAIObject()
    {
        yield return new WaitForSeconds(1f);
        ItemManager.Instance.ThrowRandomAIObject();
    }

    public IEnumerator WaitAndTriggerAnimation(Animator animator, string key)
    {
        yield return new WaitForSeconds(.5f);
        animator.SetTrigger(key);
    }
}
