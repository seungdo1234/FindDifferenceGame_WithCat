using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPopUpButton : BaseButtonUI
{
    private DiaryPopUpUI diaryPopUpUI;
    public void DiaryButtonInit()
    {
        diaryPopUpUI = UIManager.Instance.CanvasLocator.GetCanvas(E_CanvasName.DiaryBookCanvas).GetComponent<DiaryPopUpUI>();
        RegisterOnClickEvent(OpenDiaryPopUpUI);
    }

    private void OpenDiaryPopUpUI()
    {
        ButtonClickAnimation(OpenDiaryEvent);
    }

    private void OpenDiaryEvent()
    {
        diaryPopUpUI.OpenPopUp();
    }
}
