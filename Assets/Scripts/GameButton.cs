using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    public enum ButtonType
    {
        DENY,
        WANTMORE,
        TRADE
    }

    public ButtonType buttonType;

    private void OnMouseDown()
    {
        switch (buttonType)
        {
            case ButtonType.DENY:
                GameManager.Instance.ButtonPressed(true, buttonType);
                ItemManager.Instance.playerState = ItemManager.State.Deny;
                if (ItemManager.Instance.aiController.currentAIState != ItemManager.State.Deny)
                {
                    ItemManager.Instance.aiController.currentAIState = ItemManager.State.Deny;
                    StartCoroutine(ItemManager.Instance.aiController.WaitAndTriggerAnimation(GameManager.Instance.AIHandsAnimator, "DENY"));
                }                
                break;
            case ButtonType.WANTMORE:
                GameManager.Instance.ButtonPressed(true, buttonType);
                ItemManager.Instance.playerState = ItemManager.State.WantMore;
                ItemManager.Instance.aiController.DecideNextMove();
                break;
            case ButtonType.TRADE:
                GameManager.Instance.ButtonPressed(true, buttonType);
                ItemManager.Instance.playerState = ItemManager.State.Trade;
                if (ItemManager.Instance.aiController.currentAIState != ItemManager.State.Trade)
                {
                    ItemManager.Instance.aiController.DecideNextMove();
                }
                break;
            default:

                break;
        }
    }
}
