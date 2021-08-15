using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public ItemManager.State currentAIState;

    public void DecideNextMove()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (ItemManager.Instance.AIItemsOnTheTable.Count == 0)
        {
            StartCoroutine(WaitAndThrowRandomAIObject());
        }
        else
        {
            switch (ItemManager.Instance.playerState)
            {
                case ItemManager.State.None:
                    MakeRandomMove();
                    break;
                case ItemManager.State.Trade:
                    if (Random.Range(0, 2) == 0)
                    {
                        currentAIState = ItemManager.State.Trade;
                        StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "TRADE"));
                    }
                    else
                    {
                        currentAIState = ItemManager.State.WantMore;
                        StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "WANTMORE"));
                    }
                    break;
                case ItemManager.State.WantMore:
                    StartCoroutine(WaitAndThrowRandomAIObject());
                    break;
                case ItemManager.State.Deny:
                    currentAIState = ItemManager.State.Deny;
                    StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
                    break;
                default:
                    break;
            }
        }
    }

    public void MakeRandomMove()
    {
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                StartCoroutine(WaitAndThrowRandomAIObject());
                break;
            case 1:
                currentAIState = ItemManager.State.Deny;
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
                break;
            case 2:
                currentAIState = ItemManager.State.Trade;
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "TRADE"));
                break;
            case 3:
                currentAIState = ItemManager.State.WantMore;
                StartCoroutine(WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "WANTMORE"));
                break;
            default:
                break;
        }
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
