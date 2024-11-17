
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameResultOkayBtn : BaseButtonUI
{

    private GameResultUI gameResultUI;
    protected override void Awake()
    {
        base.Awake();
        RegisterOnClickEvent(GoToLobby);
    }

    public void BtnInit(GameResultUI gameResultUI)
    {
        btn.interactable = true;
        this.gameResultUI = gameResultUI;
    }

    private void GoToLobby()
    {
        btn.interactable = false;
        ButtonClickAnimation(gameResultUI.OnClickEvent);
    }
}
