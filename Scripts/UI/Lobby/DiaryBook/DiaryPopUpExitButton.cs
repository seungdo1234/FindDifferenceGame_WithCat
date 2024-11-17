
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPopUpExitButton : BaseButtonUI
{

    private DiaryPopUpUI popUp;
    protected override void Awake()
    {
        base.Awake();
        popUp = GetComponentInParent<DiaryPopUpUI>();
        RegisterOnClickEvent(ExitPopUpUI);
    }
    
    private void ExitPopUpUI()
    {
        ButtonClickAnimation(ClosePopUp);
    }

    private void ClosePopUp()
    {
        popUp.ClosePopUp();
    }
}
