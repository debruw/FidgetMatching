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
                break;
            case ButtonType.WANTMORE:
                GameManager.Instance.ButtonPressed(true, buttonType);
                ItemManager.Instance.playerState = ItemManager.State.WantMore;
                break;
            case ButtonType.TRADE:
                GameManager.Instance.ButtonPressed(true, buttonType);
                ItemManager.Instance.playerState = ItemManager.State.Trade;
                break;
            default:

                break;
        }
    }
}
